import {Outlet} from "react-router-dom";
import AppBar from "./AppBar";
import Footer from "./Footer";
import {Divider, Stack} from "@mui/material";
import Login from "src/components/auth/Login.tsx";
import {useStore} from "src/stores";
import UpdateUser from "src/components/auth/UpdateUser.tsx";
import {UserStatus} from "src/types.ts";
import {observer} from "mobx-react-lite";
import {useEffect} from "react";
import {LoadingCircular} from "src/layout/LoadingCircular.tsx";

const App = () => {
    const {userStore: {user, auth}, commonStore} = useStore();

    useEffect(() => {
        if (commonStore.token)
            auth().finally(() => commonStore.setAppLoaded());
        else
            commonStore.setAppLoaded();
    }, [commonStore, auth]);

    if (!commonStore.appLoaded) return <LoadingCircular/>;

    return (
        <Stack minHeight={'100vh'}>
            <AppBar/>
            <Stack px={35} flexGrow={1}>
                <Divider variant="middle"/>
                {!!user ?
                    (user.status === UserStatus.Active
                            ? <Outlet/>
                            : <Stack flexGrow={1} alignItems={"center"} justifyContent={'center'}>
                                <UpdateUser/>
                            </Stack>
                    )
                    :
                    <Stack flexGrow={1} alignItems={"center"} justifyContent={'center'}>
                        <Login/>
                    </Stack>
                }
            </Stack>
            <Footer/>
        </Stack>
    )
};
export default observer(App);