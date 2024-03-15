import {Form, FormStatus, FormType} from "src/types.ts";
import {useStore} from "src/stores";
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack} from "@mui/material";
import {
    DateTimePickerElement,
    FormContainer,
    RadioButtonGroup,
    TextareaAutosizeElement,
    TextFieldElement,
} from "react-hook-form-mui";
import {LoadingButton} from "@mui/lab";
import {observer} from "mobx-react-lite";

type Props = {
    open: boolean;
    handleClose: () => void;
    form?: Form
}
const ApproveForm = ({form, open, handleClose}: Props) => {
    const {formStore: {loading, update}, snackbarStore} = useStore();
    if (!form) return null;
    return (
        <Dialog
            open={open}
            onClose={handleClose}
            keepMounted
        >
            <FormContainer
                onSuccess={data => update(form.id, data)
                    .then(() => snackbarStore.success('Duyệt thành công'))
                    .catch(() => snackbarStore.error('Duyệt thất bại'))
                    .finally(handleClose)
                }
                mode={'onChange'}
            >
                <DialogTitle>Duyệt thủ tục</DialogTitle>
                <Stack spacing={2} component={DialogContent} dividers minWidth={550}>
                    {form.type === FormType.Card && <TextFieldElement name={'code'} label={'Số vào sổ'} required/>}
                    <RadioButtonGroup
                        name="status"
                        options={[
                            {
                                id: FormStatus.Approved,
                                label: 'Chấp thuận'
                            },
                            {
                                id: FormStatus.Rejected,
                                label: 'Từ chối'
                            }
                        ]}
                        row
                        required
                        type={'number'}
                    />

                    <DateTimePickerElement
                        name={'dateToGetResult'}
                        label={'Ngày đến nhận kết quả'}
                        sx={{width: '100%'}}
                    />

                    <TextareaAutosizeElement
                        label="Nội dung (sẽ gửi thông báo về cho người yêu cầu, không cần ghi ngày nhận kết quả ở đây)"
                        name="note"
                        rows={5}
                    />
                </Stack>

                <DialogActions sx={{p: 2}}>
                    <Button onClick={handleClose} variant={'contained'} color={'error'}>Hủy</Button>
                    <LoadingButton loading={loading} type="submit" variant={'contained'}>Lưu</LoadingButton>
                </DialogActions>
            </FormContainer>
        </Dialog>
    )
};
export default observer(ApproveForm);