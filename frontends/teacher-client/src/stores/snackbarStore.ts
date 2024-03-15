import {makeAutoObservable} from "mobx";
import {enqueueSnackbar} from "notistack";

export default class SnackbarStore {

    constructor() {
        makeAutoObservable(this);
    }

    success = (message: string) => {
        enqueueSnackbar(message, {variant: 'success',});
    }
    warning = (message: string) => {
        enqueueSnackbar(message, {variant: 'warning',});
    }
    info = (message: string) => {
        enqueueSnackbar(message, {variant: 'info',});
    }
    error = (message: string) => {
        enqueueSnackbar(message, {variant: 'error',});
    }
}