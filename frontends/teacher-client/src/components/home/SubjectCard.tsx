import {Subject} from "src/types.ts";
import {Divider, Link, Paper, Stack, Typography} from "@mui/material";
import {format} from "date-fns";
import {PersonRounded, SchoolRounded} from "@mui/icons-material";
import {Link as LinkRouter} from "react-router-dom";

type SubjectCardProps = {
    subject: Subject;
};


const SubjectCard = ({subject}: SubjectCardProps) => {
    return (
        <Stack direction={'row'} component={Paper} alignItems={'center'} p={2}>
            <Stack flexGrow={1}>
                <Typography gutterBottom variant={'h5'}><b>{subject.name}</b></Typography>
                <Typography gutterBottom variant={'body1'}>{subject.room}</Typography>
                <Typography gutterBottom variant={'body1'}>{subject.code}</Typography>
                <Typography gutterBottom variant={'body1'}>
                    {format(new Date(subject.dateStart), 'dd/MM/yyyy')} - {format(new Date(subject.dateEnd), 'dd/MM/yyyy')}
                </Typography>
            </Stack>


            <Divider orientation={'vertical'} flexItem/>
            <Stack p={4} alignItems={'center'} justifyContent={'center'}>
                <PersonRounded sx={{width: 60, height: 60, color: theme => theme.palette.grey.A700}}/>
                <Typography variant={'body1'}>Số sinh viên: {subject.studentsCount}</Typography>
            </Stack>
            <Divider orientation={'vertical'} flexItem/>
            <Stack p={4} alignItems={'center'} justifyContent={'center'}>
                <SchoolRounded sx={{width: 60, height: 60, color: theme => theme.palette.grey.A700}}/>
                <Typography variant={'body1'}>Số tiết
                    học: {subject.currentLessonsCount}/{subject.lessonsCount}</Typography>
            </Stack>
            <Divider orientation={'vertical'} flexItem/>
            <Stack p={4} alignItems={'center'} justifyContent={'center'}>
                <Link component={LinkRouter} underline={"hover"} to={`/subject/${subject.id}`}>Xem thêm</Link>
            </Stack>
        </Stack>
    );
}
export default SubjectCard;