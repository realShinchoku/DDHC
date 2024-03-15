import {Divider, Stack, Typography} from "@mui/material";

const Footer = () => (
    <Stack px={35} spacing={2.5} py={2.5}>
        <Divider variant="middle"/>
        <Typography variant='h6'><b>Hệ thống hành chính một cửa - Trường Đại Học Thủy Lợi</b></Typography>
        <Typography variant='h6'><b>Địa chỉ</b>: 175 Tây sơn, Đống Đa, Hà Nội</Typography>
        <Typography variant='h6'/>
    </Stack>
);
export default Footer