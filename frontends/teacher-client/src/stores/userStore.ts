import {Teacher} from "src/types.ts";
import {makeAutoObservable, runInAction} from "mobx";
import api from "src/api.ts";

export default class UserStore {
    user: Teacher | null = null;
    loading = false;

    constructor() {
        makeAutoObservable(this);
    }

    auth = async () => {
        this.setLoading(true);
        try {
            const user = await api.Users.auth();
            runInAction(() => this.user = user);
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    update = async (user: Teacher | null) => {
        if (!user) return;
        this.setLoading(true);
        try {
            const updatedUser = await api.Users.update(user);
            runInAction(() => {
                this.user = updatedUser;
            });
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    private setLoading = (loading: boolean) => this.loading = loading;
}