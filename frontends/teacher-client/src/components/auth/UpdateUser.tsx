import {Paper, Stack, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import {FormContainer, TextFieldElement} from "react-hook-form-mui";
import {useStore} from "src/stores";
import {LoadingButton} from "@mui/lab";

const UpdateUser = () => {
    const {userStore: {user, update, loading}, snackbarStore} = useStore();
    return (
        <Stack component={Paper} alignItems={'center'} p={10} spacing={5} elevation={4}>
            <Stack alignItems={'center'}>
                <Typography variant='h4'>Lần đầu đăng nhập</Typography>
                <Typography variant='h6'>vui lòng cập nhập thông tin của bạn để tiếp tục</Typography>
            </Stack>
            <FormContainer
                reValidateMode={'onBlur'}
                defaultValues={user}
                onSuccess={data => update(data)
                    .then(() => snackbarStore.success('Cập nhập thông tin thành công'))
                    .catch(() => snackbarStore.error('Cập nhập thông tin thất bại'))
                }
            >
                <Stack spacing={2} alignItems={'center'} justifyContent={'center'}>
                    <TextFieldElement name={'email'} label={'Email'} disabled type={'email'} fullWidth required/>
                    <TextFieldElement name={'name'} label={'Tên'} fullWidth required/>
                    <TextFieldElement name={'phone'} label={'Số điện thoại'} fullWidth required/>
                    <TextFieldElement name={'department'} label={'Bộ môn'} fullWidth required/>
                    <TextFieldElement name={'faculty'} label={'Khoa'} fullWidth required/>

                    <LoadingButton loading={loading} variant="contained" color="primary" type={'submit'}
                                   fullWidth={false} size={'large'}
                                   sx={{mt: '2rem!important'}}>
                        Xác nhận
                    </LoadingButton>
                </Stack>
            </FormContainer>
        </Stack>
    );
}
export default observer(UpdateUser);