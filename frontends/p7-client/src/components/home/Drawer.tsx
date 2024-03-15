import {Divider, Drawer as MuiDrawer, Link, Stack, Typography} from "@mui/material";
import {Form, FormType, getFormType, getPurpose, getReason, Reason} from "src/types.ts";
import {observer} from "mobx-react-lite";
import {Image} from 'mui-image';
import {Link as LinkRouter} from "react-router-dom";

type Props = {
    open: boolean;
    handleClose: () => void;
    form?: Form;
}

interface Translations {
    [key: string]: string;
}


const Drawer = ({open, handleClose, form}: Props) => {
    if (!form) return null;

    const translations: Translations = {
        "FullName": "Họ và tên",
        "BirthDay": "Ngày sinh",
        "CurrentClass": "Lớp hiện tại",
        "FirstClass": "Lớp đầu tiên",
        "StudentCode": "Mã sinh viên",
        "Course": "Khóa học",
        "Reason": "Lý do",
        "StudentType": "Loại sinh viên",
        "Photo3X4": "Ảnh 3x4",
        "FrontIdPhoto": "Ảnh mặt trước CMND/CCCD",
        "BackIdPhoto": "Ảnh mặt sau CMND/CCCD",
        "Code": "Số vào sổ",
        "Sex": "Giới tính",
        "Class": "Lớp",
        "PhoneNumber": "Số điện thoại",
        "Faculty": "Khoa",
        "IdNumber": "Số CMND/CCCD",
        "IdDateIssued": "Ngày cấp CMND/CCCD",
        "Purpose": "Mục đích",
        "CreatedDate": "Ngày làm thủ tục",
        "CardReturnDate": "Ngày trả thẻ",
        // Add more translations here
    };


    return (
        <MuiDrawer
            anchor={'right'}
            open={open}
            onClose={handleClose}
            ModalProps={{
                keepMounted: true,
            }}
        >
            <Stack sx={{width: '35vw'}} role="presentation" p={2}>
                <Typography variant={'h5'} gutterBottom>
                    {getFormType(form.type).replace('_', '') + '-' + form.email.split('@')[0]}
                </Typography>
                <Divider/>
                <Stack spacing={2} mt={2}>
                    {!!form.body && Object.entries(form.body)
                        .filter(([key, value]) => !key.toLowerCase().includes('photo') && typeof value !== "number")
                        .map(([key, value]) => (
                            <Stack key={key} direction={'row'} justifyContent={'space-between'}>
                                <Typography variant={'body1'}><b>{translations[key] || key}:</b></Typography>
                                <Typography variant={'body1'}>{value}</Typography>
                            </Stack>
                        ))
                    }
                    {form.type === FormType.Card &&
                        <>
                            <Stack direction={'row'} justifyContent={'space-between'}>
                                <Typography variant={'body1'}><b>Lý do làm lại thẻ:</b></Typography>
                                <Typography variant={'body1'}>
                                    {'Reason' in form.body && getReason(form.body.Reason)}
                                </Typography>
                            </Stack>
                            <Stack direction={'row'} justifyContent={'space-between'}>
                                <Typography variant={'body1'}><b>Mẫu thủ tục:</b></Typography>
                                <Link
                                    component={LinkRouter}
                                    underline={"hover"}
                                    to={`${import.meta.env.VITE_BACKEND_URL}/files/docs/${form.id}`}
                                    target="_blank"
                                >
                                    📄{form.email.split('@')[0]}.docx
                                </Link>
                            </Stack>
                            {'Reason' in form.body
                                && form.body.Reason === Reason.FirstCreate
                                &&
                                <>
                                    <Stack justifyContent={'space-between'}>
                                        <Typography variant={'body1'}><b>Ảnh 3x4:</b></Typography>
                                        {'Photo3X4' in form.body &&
                                            <Image
                                                showLoading
                                                src={`${import.meta.env.VITE_BACKEND_URL}/files/images/${form.body.Photo3X4}`}
                                                errorIcon={null}
                                            />
                                        }
                                    </Stack>
                                    <Stack>
                                        <Typography variant={'body1'}><b>Ảnh CMND/CCCD mặt trước:</b></Typography>
                                        {'FrontIdPhoto' in form.body &&
                                            <Image
                                                showLoading
                                                src={`${import.meta.env.VITE_BACKEND_URL}/files/images/${form.body.FrontIdPhoto}`}
                                                errorIcon={null}
                                            />
                                        }
                                    </Stack>

                                    <Stack>
                                        <Typography variant={'body1'}><b>Ảnh CMND/CCCD mặt sau:</b></Typography>
                                        {'BackIdPhoto' in form.body &&
                                            <Image
                                                showLoading
                                                src={`${import.meta.env.VITE_BACKEND_URL}/files/images/${form.body.BackIdPhoto}`}
                                                errorIcon={null}
                                            />
                                        }
                                    </Stack>
                                </>
                            }
                        </>
                    }
                    {form.type === FormType.Verify &&
                        <>
                            <Stack direction={'row'} justifyContent={'space-between'}>
                                <Typography variant={'body1'}><b>Mục đích</b></Typography>
                                <Typography variant={'body1'}>
                                    {'Purpose' in form.body && getPurpose(form.body.Purpose)}
                                </Typography>
                            </Stack>
                        </>
                    }
                </Stack>
            </Stack>
        </MuiDrawer>
    )
};
export default observer(Drawer);