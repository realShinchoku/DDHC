import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import {RouterProvider} from 'react-router-dom';
import {router} from "src/router";
import {MsalProvider} from "@azure/msal-react";
import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import {LocalizationProvider, viVN as pickerViVn} from "@mui/x-date-pickers";
import {AdapterDateFns} from "@mui/x-date-pickers/AdapterDateFnsV3";
import {msalInstance} from "src/authConfig.ts";
import {vi} from "date-fns/locale/vi";
import {closeSnackbar, SnackbarProvider} from "notistack";
import {CloseRounded} from "@mui/icons-material";
import {viVN as gridViVN} from '@mui/x-data-grid';
import {viVN} from '@mui/material/locale';

const theme = createTheme(undefined,
    gridViVN, // x-data-grid translations
    pickerViVn, // x-date-pickers translations
    viVN, // core translations
)

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <ThemeProvider theme={theme}>
            <LocalizationProvider
                dateAdapter={AdapterDateFns}
                localeText={pickerViVn.components.MuiLocalizationProvider.defaultProps.localeText}
                adapterLocale={vi}
            >
                <MsalProvider instance={msalInstance}>
                    <CssBaseline/>
                    <SnackbarProvider
                        maxSnack={5}
                        anchorOrigin={{
                            vertical: 'top',
                            horizontal: 'right',
                        }}
                        autoHideDuration={5000}
                        action={(snackbarId) => (<CloseRounded onClick={() => closeSnackbar(snackbarId)}/>)}
                    >
                        <RouterProvider router={router}/>
                    </SnackbarProvider>
                </MsalProvider>
            </LocalizationProvider>
        </ThemeProvider>
    </React.StrictMode>,
)
