import {Button, Stack, Typography} from "@mui/material";
import {format} from "date-fns";
import {Link, Navigate, useParams} from "react-router-dom";
import {AccessTimeRounded, ArrowBackRounded, DeleteRounded} from "@mui/icons-material";
import StudentTable from "src/components/detail/StudentTable.tsx";
import AttendanceStart from "src/components/detail/AttendanceStart.tsx";
import {useEffect, useState} from "react";
import {useStore} from "src/stores";
import {LoadingCircular} from "src/layout/LoadingCircular.tsx";
import {observer} from "mobx-react-lite";
import {LoadingButton} from "@mui/lab";

const SubjectDetail = () => {
    const {
        subjectStore: {
            getDetail,
            detailedItem,
            initialLoading,
            delete: deleteSubject,
            loading,
            setInitialLoading,
            clearSelectedItem
        },
        snackbarStore
    } = useStore();
    const {id} = useParams();
    const [open, setOpen] = useState(false);

    useEffect(() => {
        if (id) getDetail(id);
        setInitialLoading(false);
        return () => clearSelectedItem();
    }, [id, getDetail, clearSelectedItem, setInitialLoading]);

    if (!id) return <Navigate to={'/'}/>;

    if (initialLoading || !detailedItem) return <LoadingCircular/>;

    return (
        <>
            <Stack p={2} spacing={4}>
                <Stack direction={'row'} alignItems={'center'} spacing={1}>
                    <Link to={'/'}><ArrowBackRounded color={'action'}/></Link>
                    <Stack flexGrow={1} spacing={1}>
                        <Typography
                            variant={'h6'}>{detailedItem.name} - {detailedItem.code} - {detailedItem.room}</Typography>
                        <Typography variant={"body1"}>
                            {format(new Date(detailedItem.dateStart), 'dd/MM/yyyy')} - {format(new Date(detailedItem.dateEnd), 'dd/MM/yyyy')}
                        </Typography>
                    </Stack>
                    <Button
                        variant={'outlined'}
                        startIcon={<AccessTimeRounded/>}
                        size={'large'}
                        onClick={() => setOpen(true)}
                    >
                        Bắt đầu điểm danh
                    </Button>
                    <LoadingButton
                        loading={loading}
                        variant={'outlined'}
                        startIcon={<DeleteRounded/>}
                        color={'error'}
                        size={'large'}
                        onClick={() => deleteSubject(id)
                            .then(() => snackbarStore.success('Xóa thành công'))
                            .catch(() => snackbarStore.error('Xóa thất bại'))
                        }
                    >
                        Xóa
                    </LoadingButton>
                </Stack>
                <StudentTable subject={detailedItem}/>
            </Stack>
            <AttendanceStart open={open} handleClose={() => setOpen(false)} subject={detailedItem}/>
        </>
    );
}
export default observer(SubjectDetail)