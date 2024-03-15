import {Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack} from "@mui/material";
import {FormContainer, SelectElement, TimePickerElement} from "react-hook-form-mui";
import {Subject} from "src/types.ts";
import {format} from "date-fns";
import {useStore} from "src/stores";
import {observer} from "mobx-react-lite";
import {LoadingButton} from "@mui/lab";

type Props = {
    open: boolean;
    handleClose: () => void;
    subject: Subject;
}

const AttendanceStart = ({open, handleClose, subject}: Props) => {
    const {subjectStore: {startAttendance, loading}, snackbarStore} = useStore();
    return (
        <Dialog
            open={open}
            onClose={handleClose}
        >
            <FormContainer
                onSuccess={data => startAttendance(data.lessonId, data.openAttendanceTime, data.closeAttendanceTime)
                    .then(() => snackbarStore.success('Bắt đầu điểm danh thành công'))
                    .catch(() => snackbarStore.error('Bắt đầu điểm danh thất bại'))
                    .finally(handleClose)
                }
            >
                <DialogTitle>Bắt đầu điểm danh</DialogTitle>
                <Stack spacing={2} component={DialogContent} dividers minWidth={450}>
                    <SelectElement
                        name={'lessonId'}
                        label={'Tiết học'}
                        fullWidth
                        options={subject.lessons
                            .filter(lesson => new Date(lesson.endTime) > new Date())
                            .map(lesson => ({
                                id: lesson.id,
                                label: format(new Date(lesson.endTime), 'dd/MM/yyyy')
                            }))}
                    />
                    <TimePickerElement
                        name={'openAttendanceTime'}
                        label={'Thời gian điểm danh bắt đầu'}
                        views={['hours', 'minutes']}
                        format={'hh:mm'}
                        sx={{width: '100%'}}
                    />
                    <TimePickerElement
                        name={'closeAttendanceTime'}
                        label={'Thời gian điểm danh kết thúc'}
                        views={['hours', 'minutes']}
                        format={'hh:mm'}
                        sx={{width: '100%'}}
                    />
                </Stack>

                <DialogActions sx={{p: 2}}>
                    <Button onClick={handleClose} variant={'contained'} color={'error'}>Hủy</Button>
                    <LoadingButton loading={loading} type="submit" variant={'contained'}>Bắt đầu</LoadingButton>
                </DialogActions>
            </FormContainer>
        </Dialog>
    );
}
export default observer(AttendanceStart);