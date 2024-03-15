import {Box, CircularProgress} from "@mui/material";

export const LoadingCircular = () =>
    <Box sx={{flexGrow: 1, display: 'flex', alignItems: 'center', justifyContent: 'center', height: '20vh'}}>
        <CircularProgress/>
    </Box>
