import {Stack, Typography} from "@mui/material";
import SubjectCard from "src/components/home/SubjectCard.tsx";
import {useStore} from "src/stores";
import {useEffect} from "react";
import {LoadingCircular} from "src/layout/LoadingCircular.tsx";
import {observer} from "mobx-react-lite";

const Home = () => {
    const {subjectStore: {items, itemRegistry, loading, getList}, userStore: {user}} = useStore();

    useEffect(() => {
        if (itemRegistry.size <= 1) getList();
    }, []);

    return (
        <Stack minHeight={'100%'} p={2}>
            <Typography variant={"h6"} gutterBottom>
                <b>Xin chào {user?.name}, Dưới đây là danh sách các lớp học hiện có của bạn.</b>
            </Typography>
            <Stack spacing={2} p={2}>
                {loading ? <LoadingCircular/> : items.map((subject) => <SubjectCard key={subject.id}
                                                                                    subject={subject}/>)}
            </Stack>
        </Stack>
    )
};
export default observer(Home);