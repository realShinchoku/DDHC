import {DataGrid, GridActionsCellItem, GridColDef, GridRowId, GridRowModel} from "@mui/x-data-grid";
import {Delete, DownloadRounded} from "@mui/icons-material";
import {Button, Stack, Typography} from "@mui/material";
import {Student} from "src/types";
import CSVReader from "./CsvReader";


const columns: GridColDef<Student>[] = [
    {field: 'id', headerName: 'STT', width: 60, editable: false},
    {field: 'name', headerName: 'Họ và tên', editable: true, flex: 1},
    {field: 'studentCode', headerName: 'Mã sinh viên', flex: 1, editable: true},
    {field: 'email', headerName: 'Email', flex: 1, editable: true},
    {field: 'class', headerName: 'Lớp', flex: 0.5, editable: true},
    {field: 'faculty', headerName: 'Khoa', flex: 0.5, editable: true},
]

type Props = {
    students: Student[];
    onChange: (students: Student[]) => void
}

const StudentUpload = ({students, onChange}: Props) => {
    const handleDeleteClick = (id: GridRowId) => () => onChange(students.filter(row => row.id !== id));

    const processRowUpdate = (newRow: GridRowModel) => {
        const updatedRow = newRow as Student;
        onChange(students.map(row => (row.id === newRow.id ? updatedRow : row)));
        return updatedRow;
    };

    const baseColumns: GridColDef[] = [
        ...columns,
        {

            field: 'actions',
            type: 'actions',
            headerName: '',
            width: 100,
            cellClassName: 'actions',
            getActions: ({id}) => {
                return [
                    <GridActionsCellItem
                        icon={<Delete/>}
                        label="Delete"
                        onClick={handleDeleteClick(id)}
                        color="inherit"
                    />,
                ];
            },
        },
    ];

    return (
        <>
            <Stack direction={'row'} alignItems={'center'} px={2} spacing={1}>
                <Typography variant={'subtitle1'}>Danh sách sinh viên</Typography>
                <CSVReader
                    parserOptions={{header: true, skipEmptyLines: true}}
                    onFileLoaded={(data: Student[]) => onChange(data)}
                    inputId={'csv-input'}
                />
                <Button href={'/sample.csv'} color={'secondary'} variant={'outlined'}
                        startIcon={<DownloadRounded/>}>Tải tệp mẫu</Button>
            </Stack>
            <DataGrid
                rows={students}
                columns={baseColumns}
                editMode="row"
                processRowUpdate={processRowUpdate}
                autoHeight
                hideFooterSelectedRowCount
            />
        </>
    );
};

export default StudentUpload;