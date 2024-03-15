import {createContext, useContext} from "react";
import UserStore from "src/stores/userStore.ts";
import CommonStore from "src/stores/commonStore.ts";
import SubjectStore from "src/stores/subjectStore.ts";
import SnackbarStore from "src/stores/snackbarStore.ts";

interface Store {
    userStore: UserStore;
    commonStore: CommonStore;
    subjectStore: SubjectStore;
    snackbarStore: SnackbarStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    subjectStore: new SubjectStore(),
    snackbarStore: new SnackbarStore(),
}

export const StoreContext = createContext(store);

export const useStore = () => useContext(StoreContext);