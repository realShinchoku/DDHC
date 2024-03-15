import {makeAutoObservable, reaction, runInAction} from "mobx";
import {Form, FormStatus} from "src/types.ts";
import api from "src/api.ts";

export default class FormStore {
    itemRegistry = new Map<string, Form>();
    loading = false;
    initialLoading = true;
    detailedItem: Form | undefined = undefined;
    totalForms = 0;
    count = 0;
    // approvedForms = 0;
    page = 0;
    rowsPerPage = 10;

    constructor() {
        makeAutoObservable(this);
        reaction(() => this.page, () => this.getList());
    }

    get items() {
        return Array.from(this.itemRegistry.values());
    }
    
    get approvedForms() {
        return this.items.filter(item => item.status !== FormStatus.Pending).length;
    }

    setInitialLoading = (loading: boolean) => this.initialLoading = loading;

    setPage = (page: number) => this.page = page;
    setRowsPerPage = (rowsPerPage: number) => {
        this.rowsPerPage = rowsPerPage
        this.page = 0;
    };

    getList = async () => {
        this.setLoading(true);
        this.itemRegistry.clear();
        try {
            const {items, totalForms, count} = await api.Forms.list(this.page + 1, this.rowsPerPage);
            items.forEach(item => this.setItem(item));
            runInAction(() => {
                this.totalForms = totalForms;
                this.count = count;
                // this.approvedForms = approvedForms;
            })
        } catch (e) {
            console.log(e)
        } finally {
            this.setLoading(false);
        }
    }

    getDetail = async (id: string) => {
        if (this.detailedItem?.id === id) return;

        this.setLoading(true);
        try {
            const item = await api.Forms.detail(id);
            runInAction(() => this.detailedItem = item);
        } catch (e) {
            console.log(e)
        } finally {
            this.setLoading(false);
        }
    }

    update = async (id: string, body: any) => {
        this.setLoading(true);
        try {
            const item = await api.Forms.update(id, body);
            runInAction(() => {
                this.setItem(item);
                this.detailedItem = item;
            });
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    clearSelectedItem = () => this.detailedItem = undefined;

    private setLoading = (loading: boolean) => this.loading = loading;

    private setItem = (item: Form) => this.itemRegistry.set(item.id, item);

}