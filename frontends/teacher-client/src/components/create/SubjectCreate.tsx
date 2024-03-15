import {Stack, Typography} from "@mui/material";
import {ArrowBackRounded, SaveRounded} from "@mui/icons-material";
import {DatePickerElement, FormContainer, TextFieldElement} from "react-hook-form-mui";
import {Link} from "react-router-dom";
import StudentUpload from "src/components/create/StudentUpload.tsx";
import {useStore} from "src/stores";
import {useState} from "react";
import {Student} from "src/types.ts";
import LessonsCreate from "src/components/create/LessonsCreate.tsx";
import {LoadingButton} from "@mui/lab";

const SubjectCreate = () => {
    const {subjectStore: {create, loading}, snackbarStore} = useStore();
    const [students, setStudents] = useState<Student[]>([]);

    const handleStudentsChange = (students: Student[]) => setStudents(students);

    return (
        <FormContainer
            reValidateMode={'onBlur'}
            onSuccess={data => create({...data, students})
                .then(() => snackbarStore.success('Thêm môn học thành công'))
                .catch(() => snackbarStore.error('Thêm môn học thất bại'))
            }
        >
            <Stack p={2} spacing={2}>
                <Stack direction={'row'} alignItems={'center'} px={2} spacing={2}>
                    <Link to={'/'}><ArrowBackRounded color={'action'}/></Link>
                    <Typography variant={'h6'} flexGrow={1}>Nhập môn học mới</Typography>
                    <LoadingButton loading={loading} variant={'outlined'} size={'large'} type={'submit'}
                                   startIcon={<SaveRounded/>}
                                   color={'success'}>Lưu</LoadingButton>
                </Stack>
                <Typography variant={'subtitle1'}>Thông tin môn học</Typography>
                <Stack spacing={2} direction={'row'} alignItems={'center'}>
                    <TextFieldElement name={'name'} label={'Tên môn học'} fullWidth required/>
                    <TextFieldElement name={'code'} label={'Mã môn học'} fullWidth required/>
                    <TextFieldElement name={'room'} label={'Phòng'} fullWidth required/>
                    <DatePickerElement name={'dateStart'} label={'Ngày bắt đầu'} sx={{width: '100%'}} required/>
                    <DatePickerElement name={'dateEnd'} label={'Ngày kết thúc'} sx={{width: '100%'}} required/>
                </Stack>
                <LessonsCreate/>
                <StudentUpload students={students} onChange={handleStudentsChange}/>
            </Stack>
        </FormContainer>

    );
}
export default SubjectCreate;