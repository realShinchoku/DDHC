import {Paper, Stack, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import {useStore} from "src/stores";

const StatusCard = () => {
    const {formStore: {totalForms, approvedForms}} = useStore();

    return (
        <Stack direction={'row'} component={Paper} variant={'outlined'} p={5} width={'fit-content'} spacing={5}>
            <Stack spacing={4} alignItems={'center'} justifyContent={'center'}>
                <Typography variant={"h4"} gutterBottom color={theme => theme.palette.primary.light}>
                    {approvedForms}
                </Typography>
                <Typography variant={"body1"} gutterBottom>
                    Tài liệu đã duyệt
                </Typography>
            </Stack>
            <Stack spacing={4} alignItems={'center'} justifyContent={'center'}>
                <Typography variant={"h4"} gutterBottom color={theme => theme.palette.primary.light}>
                    {totalForms - approvedForms}
                </Typography>
                <Typography variant={"body1"} gutterBottom>
                    Tài liệu chưa duyệt
                </Typography>
            </Stack>
        </Stack>
    );
}

export default observer(StatusCard);