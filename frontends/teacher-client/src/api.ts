import axios, {AxiosError, AxiosResponse} from "axios";
import {router} from "src/router";
import {AttendanceStudent, Lesson, Subject, Teacher} from "src/types.ts";
import {store} from "src/stores";
import {msalInstance} from "src/authConfig.ts";


axios.defaults.baseURL = import.meta.env.VITE_BACKEND_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use(async config => {
    const accessToken = store.commonStore.token;
    if (accessToken && config.headers) config.headers.Authorization = `Bearer ${accessToken}`;
    return config;
});

axios.interceptors.response.use(response => response
    , async (error: AxiosError) => {
        const {data, status, config, headers} = error.response as AxiosResponse;
        switch (status) {
            case 400:

                if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                    await router.navigate('/not-found');
                } else if (data.errors) {
                    const modelStateErrors = [];
                    for (const key in data.errors) {
                        if (data.errors[key])
                            modelStateErrors.push(data.errors[key]);
                    }
                    if (modelStateErrors.length > 0)
                        throw modelStateErrors.flat();
                }
                break;
            case 401:
                if (status === 401 && headers['www-authenticate']?.startsWith('Bearer error="invalid_token')) {
                    store.snackbarStore.error('Thời gian đăng nhập đã hết hạn, vui lòng đăng nhập lại');
                    msalInstance.clearCache().catch(e => console.error(e));
                    store.commonStore.setToken(undefined);
                    window.location.reload();
                }
                break;
            case 403:
                store.snackbarStore.error('Tài khoản của bạn không có quyền truy cập');
                break;
            case 404:
                break;
            case 409:
                throw data;
            case 500:
                store.snackbarStore.error('Server error');
                break;
            default:
                store.snackbarStore.error('Server error');
                break;
        }
        return Promise.reject(error);
    });

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Users = {
    auth: () => requests.get<Teacher>('/auth'),
    update: (user: Teacher) => requests.put<Teacher>('/auth', user),
}

const Subjects = {
    list: () => requests.get<Subject[]>('/subjects'),
    create: (item: any) => requests.post<Subject>('/subjects', item),
    detail: (id: string) => requests.get<Subject>(`/subjects/${id}`),
    startAttendance: (id: string, openAttendanceTime: string, closeAttendanceTime: string) => requests.put<Lesson>(`/lessons/${id}`, {
        openAttendanceTime,
        closeAttendanceTime
    }),
    getAttendances: (id: string) => requests.get<AttendanceStudent[]>(`/subjects/${id}/attendances`),
    delete: (id: string) => requests.delete<void>(`/subjects/${id}`),
};

const agent = {
    Users,
    Subjects
}

export default agent;