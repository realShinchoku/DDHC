import {Stack, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import StatusCard from "src/components/home/StatusCard.tsx";
import FormsTable from "src/components/home/FormsTable.tsx";
import {useEffect} from "react";
import {useStore} from "src/stores";
import {LoadingCircular} from "src/layout/LoadingCircular.tsx";

const Home = () => {
    const {formStore: {initialLoading, itemRegistry, getList, setInitialLoading}} = useStore();

    useEffect(() => {
        if (itemRegistry.size < 1) getList();
        setInitialLoading(false);
    }, []);

    if (initialLoading) return <LoadingCircular/>;

    return (
        <Stack minHeight={'100%'} p={2} spacing={4}>
            <Typography variant={"body1"} gutterBottom>
                Chào mừng bạn đến với với Hành Chính Một Cửa
            </Typography>
            <StatusCard/>
            <FormsTable/>
        </Stack>
    )
};
export default observer(Home);