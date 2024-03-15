import {Table, TableBody, TableCell, TableContainer, TableHead, TableRow} from "@mui/material";
import {AttendanceType, Subject} from "src/types.ts";
import {format} from "date-fns";
import {useStore} from "src/stores";
import {useEffect} from "react";
import {AccessTimeRounded, CheckRounded, CloseRounded} from "@mui/icons-material";
import {observer} from "mobx-react-lite";
import {LoadingCircular} from "src/layout/LoadingCircular.tsx";

type Props = {
    subject: Subject
}
const StudentTable = ({subject: {id, lessons}}: Props) => {
    const {subjectStore: {attendances, getAttendances, attendanceLoading, clearAttendances}} = useStore();

    useEffect(() => {
        if (id) getAttendances(id);
        return () => {
            clearAttendances();
        };
    }, [id, getAttendances, clearAttendances]);

    if (attendanceLoading) return <LoadingCircular/>;

    return (
        <TableContainer>
            <Table sx={{width: 'fit-content'}} size="small" padding={'normal'}>
                <TableHead>
                    <TableRow>
                        <TableCell align={'center'} sx={{width: '60px'}} variant={'head'}>STT</TableCell>
                        <TableCell align="left" sx={{width: '200px'}} variant={'head'}>Họ và tên</TableCell>
                        <TableCell align="left" sx={{width: '200px'}} variant={'head'}>Mã sinh viên</TableCell>
                        {!!lessons && lessons
                            .map(lesson => (
                                <TableCell align="center" key={lesson.id} sx={{width: '60px'}}>
                                    {format(lesson.startTime, 'hh:mm dd/MM')}
                                </TableCell>
                            ))}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {!!attendances && attendances.map((row, index) => (
                        <TableRow key={row.studentId}>
                            <TableCell component="th" align={"center"} scope="row">
                                {index+1}
                            </TableCell>
                            <TableCell component="th" scope="row">
                                {row.studentName}
                            </TableCell>
                            <TableCell component="th" scope="row">
                                {row.studentCode}
                            </TableCell>
                            {row.lessons.map(a =>
                                <TableCell key={row.studentId + a.lessonId} variant={'body'} align={"center"} scope="row">
                                    {a.type !== null ?
                                        (a.type === AttendanceType.Presence
                                                ? <CheckRounded color={'success'}/>
                                                : a.type === AttendanceType.Absent
                                                    ? <CloseRounded color={'error'}/>
                                                    : <AccessTimeRounded color={'warning'}/>
                                        )
                                        : '-'
                                    }
                                </TableCell>
                            )}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
};
export default observer(StudentTable);