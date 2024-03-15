import {
    Button,
    Link,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TablePagination,
    TableRow
} from "@mui/material";
import {observer} from "mobx-react-lite";
import {Form, FormStatus, getFormStatus, getFormType} from "src/types.ts";
import {format} from "date-fns";
import {RuleRounded} from "@mui/icons-material";
import {useStore} from "src/stores";
import ApproveForm from "src/components/home/ApproveForm.tsx";
import {useState} from "react";
import Drawer from "./Drawer";


const FormsTable = () => {
    const {formStore: {items, count, setPage, page, setRowsPerPage, rowsPerPage}} = useStore();

    const [form, setForm] = useState<Form>();
    const [openDialog, setOpenDialog] = useState(false);
    const [openDrawer, setOpenDrawer] = useState(false);
    const handleCloseDialog = () => {
        setForm(undefined);
        setOpenDialog(false)
    };
    const handleOpenDialog = (form: Form) => {
        setForm(form);
        setOpenDialog(true)
    };
    const handleCloseDrawer = () => {
        setForm(undefined);
        setOpenDrawer(false)
    };
    const handleOpenDrawer = (form: Form) => {
        setForm(form);
        setOpenDrawer(true)
    };

    return (
        <>
            <TableContainer>
                <Table size="small" padding={'normal'}>
                    <TableHead>
                        <TableRow>
                            <TableCell align="center" sx={{width: '60px'}} variant={'head'}>STT</TableCell>
                            <TableCell align="left" sx={{width: '200px'}} variant={'head'}>Loại</TableCell>
                            <TableCell align="left" sx={{width: '400px'}} variant={'head'}>File</TableCell>
                            <TableCell align="left" variant={'head'}>Tình trạng</TableCell>
                            <TableCell align="left" variant={'head'}>Ngày tạo</TableCell>
                            <TableCell align="left" variant={'head'}>Ngày cập nhật</TableCell>
                            <TableCell align="center" sx={{width: '60px'}} variant={'head'}></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {items.length > 0 && items.map((row, index) => (
                            <TableRow key={row.id}>
                                <TableCell variant={"body"} scope="row">
                                    {index + 1}
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    {getFormType(row.type)}
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    <Button
                                        variant="text"
                                        color="primary"
                                        onClick={() => handleOpenDrawer(row)}
                                        component={Link}
                                    >
                                        {getFormType(row.type).replace('_', '') + '-' + row.email.split('@')[0]}
                                    </Button>
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    {getFormStatus(row.status)}
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    {format(new Date(row.createdAt), 'hh:mm dd/MM/yyyy')}
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    {format(new Date(row.updatedAt), 'hh:mm dd/MM/yyyy')}
                                </TableCell>
                                <TableCell variant={"body"} scope="row">
                                    {row.status === FormStatus.Pending &&
                                        <Button
                                            variant="text"
                                            color="primary"
                                            onClick={() => handleOpenDialog(row)}
                                        >
                                            <RuleRounded/>
                                        </Button>
                                    }
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                rowsPerPageOptions={[10, 25, 50]}
                count={count}
                onPageChange={(_, page) => setPage(page)}
                page={page}
                rowsPerPage={rowsPerPage}
                onRowsPerPageChange={(event) => setRowsPerPage(+event.target.value)}
            />
            <ApproveForm form={form} open={openDialog} handleClose={handleCloseDialog}/>
            <Drawer open={openDrawer} handleClose={handleCloseDrawer} form={form}/>
        </>
    );
}

export default observer(FormsTable);