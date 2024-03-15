import {createContext, useContext} from "react";
import SnackbarStore from "src/stores/snackbarStore.ts";
import FormStore from "src/stores/formStore.ts";

interface Store {
    snackbarStore: SnackbarStore;
    formStore: FormStore;
}

export const store: Store = {
    snackbarStore: new SnackbarStore(),
    formStore: new FormStore(),
}

export const StoreContext = createContext(store);

export const useStore = () => useContext(StoreContext);