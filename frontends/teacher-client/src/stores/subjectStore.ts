import {makeAutoObservable, runInAction} from "mobx";
import {AttendanceStudent, Subject} from "src/types.ts";
import api from "src/api.ts";
import {router} from "src/router";

export default class SubjectStore {
    itemRegistry = new Map<string, Subject>();
    loading = false;
    initialLoading = true;
    attendanceLoading = false;
    detailedItem: Subject | undefined = undefined;
    attendances: AttendanceStudent[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    get items() {
        return Array.from(this.itemRegistry.values());
    }

    setInitialLoading = (loading: boolean) => this.initialLoading = loading;

    getList = async () => {
        this.setLoading(true);
        try {
            const items = await api.Subjects.list();
            items.forEach(item => this.setItem(item));
        } catch (e) {
            console.log(e)
        } finally {
            this.setLoading(false);
        }
    }

    create = async (item: any) => {
        this.setLoading(true);
        try {
            const createdItem = await api.Subjects.create(item);
            this.setItem(createdItem);
            await router.navigate(`/subject/${createdItem.id}`);
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    getDetail = async (id: string) => {
        if (this.detailedItem?.id === id) return;

        this.setLoading(true);
        try {
            const item = await api.Subjects.detail(id);
            item.lessons = item.lessons.sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime());
            runInAction(() => this.detailedItem = item);
        } catch (e) {
            console.log(e)
        } finally {
            this.setLoading(false);
        }
    }

    delete = async (id: string) => {
        this.setLoading(true);
        try {
            await api.Subjects.delete(id);
            this.itemRegistry.delete(id);
            await router.navigate("/");
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    clearSelectedItem = () => this.detailedItem = undefined;

    startAttendance = async (id: string, openAttendanceTime: string, closeAttendanceTime: string) => {
        this.setLoading(true);
        try {
            await api.Subjects.startAttendance(id, openAttendanceTime, closeAttendanceTime);
        } catch (e) {
            throw e;
        } finally {
            this.setLoading(false);
        }
    }

    clearAttendances = () => this.attendances = [];

    getAttendances = async (id: string) => {
        this.setAttendanceLoading(true);
        try {
            const attendances = await api.Subjects.getAttendances(id);
            runInAction(() => {
                this.attendances = attendances.map(attendance => {
                    attendance.lessons = attendance.lessons.sort((a, b) => new Date(a.startTime).getTime() - new Date(b.startTime).getTime());
                    return attendance;
                });
            });
        } catch (e) {
            console.log(e);
        } finally {
            this.setAttendanceLoading(false);
        }
    }

    private setLoading = (loading: boolean) => this.loading = loading;

    private setItem = (item: Subject) => this.itemRegistry.set(item.id, item);

    private setAttendanceLoading = (loading: boolean) => this.attendanceLoading = loading;
}