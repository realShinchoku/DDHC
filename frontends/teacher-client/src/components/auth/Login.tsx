import {useMsal} from "@azure/msal-react";
import {Paper, Stack, SvgIcon, Typography} from "@mui/material";
import {loginRequest} from "src/authConfig.ts";
import {useStore} from "src/stores";
import {observer} from "mobx-react-lite";
import {LoadingButton} from "@mui/lab";

const MicrosoftIcon = () => <svg xmlns="http://www.w3.org/2000/svg" width="21" height="21" viewBox="0 0 21 21">
    <title>MS-SymbolLockup</title>
    <rect x="1" y="1" width="9" height="9" fill="#f25022"/>
    <rect x="1" y="11" width="9" height="9" fill="#00a4ef"/>
    <rect x="11" y="1" width="9" height="9" fill="#7fba00"/>
    <rect x="11" y="11" width="9" height="9" fill="#ffb900"/>
</svg>;
const Login = () => {
    const {instance} = useMsal();
    const {commonStore: {setToken, setAppLoaded}, userStore: {auth, loading}, snackbarStore} = useStore();

    const handleLogin = () =>
        instance.loginPopup(loginRequest)
            .then((r) => {
                setToken(r.accessToken);
                auth()
                    .then(() => snackbarStore.success('Đăng nhập thành công'))
                    .catch(instance.clearCache)
                    .finally(() => setAppLoaded());
            })
            .catch(e => console.log(e))
            .finally(() => setAppLoaded());

    return (
        <Stack component={Paper} alignItems={'center'} p={10} spacing={5} elevation={4}>
            <Stack alignItems={'center'}>
                <Typography variant='h4'>Đăng nhập</Typography>
                <Typography variant='h6'>với tài khoản Outlook của bạn để tiếp tục</Typography>
            </Stack>
            <LoadingButton
                loading={loading}
                variant="outlined"
                color="inherit"
                onClick={handleLogin}
                startIcon={<SvgIcon inheritViewBox component={MicrosoftIcon}/>}
                size={'large'}
            >
                Sign in with Microsoft
            </LoadingButton>
        </Stack>
    )
};
export default observer(Login);