import {SelectElement, TimePickerElement} from "react-hook-form-mui";
import {Button, Stack, Typography} from "@mui/material";
import {AddRounded, CloseRounded} from "@mui/icons-material";
import {useState} from "react";

const LessonsCreate = () => {
    const [count, setCount] = useState<string[]>([]);
    return (
        <>
            <Stack direction={'row'} alignItems={'center'} px={2}>
                <Typography variant={'subtitle1'}>Thông tin tiết học</Typography>
                <Button color={'primary'} variant={'text'} size={'small'} onClick={() => {
                    setCount([...count, `${count.length}`]);
                }}><AddRounded/></Button>
            </Stack>
            {count.map((value, index) =>
                <Stack key={value} spacing={2} direction={'row'} alignItems={'center'}>
                    <SelectElement
                        label="Thứ"
                        name={`lessons[${index}].dayOfWeek`}
                        options={daysOfWeek}
                        sx={{width: '250px'}}
                        required
                    />
                    <TimePickerElement
                        name={`lessons[${index}].startTime`}
                        label={'Thời gian tiết học bắt đầu'}
                        views={['hours', 'minutes']}
                        format={'HH:mm'}
                        sx={{width: '250px'}}
                        required
                    />
                    <TimePickerElement
                        name={`lessons[${index}].endTime`}
                        label={'Thời gian tiết học kết thúc'}
                        views={['hours', 'minutes']}
                        format={'HH:mm'}
                        sx={{width: '250px'}}
                        required
                    />
                    <Button
                        color={'error'}
                        variant={'text'}
                        size={'small'}
                        onClick={() => setCount([...count.filter(v => v !== value)])}
                    >
                        <CloseRounded/>
                    </Button>
                </Stack>
            )}
        </>
    );
}
export default LessonsCreate;

const daysOfWeek = [
    {
        id: 1,
        label: 'Thứ 2'
    },
    {
        id: 2,
        label: 'Thứ 3'
    },
    {
        id: 3,
        label: 'Thứ 4'
    },
    {
        id: 4,
        label: 'Thứ 5'
    },
    {
        id: 5,
        label: 'Thứ 6'
    },
    {
        id: 6,
        label: 'Thứ 7'
    },
    {
        id: 0,
        label: 'Chủ nhật'
    }
];