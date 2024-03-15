import {Outlet} from "react-router-dom";
import AppBar from "./AppBar";
import Footer from "./Footer";
import {Divider, Stack} from "@mui/material";
import {observer} from "mobx-react-lite";

const App = () => {

    return (
        <Stack minHeight={'100vh'}>
            <AppBar/>
            <Stack px={35} flexGrow={1}>
                <Divider variant="middle"/>
                <Outlet/>
            </Stack>
            <Footer/>
        </Stack>
    )
};
export default observer(App);