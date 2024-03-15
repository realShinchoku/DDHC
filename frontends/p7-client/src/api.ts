import axios, {AxiosError, AxiosResponse} from "axios";
import {router} from "src/router";
import {store} from "src/stores";
import {Form, Response} from "src/types.ts";


axios.defaults.baseURL = import.meta.env.VITE_BACKEND_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.response.use(response => response
    , async (error: AxiosError) => {
        const {data, status, config} = error.response as AxiosResponse;
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

const Forms = {
    list: (page: number, rowsPerPage: number) => requests.get<Response>('/forms?page=' + page + '&pageSize=' + rowsPerPage),
    detail: (id: string) => requests.get<Form>(`/forms/${id}`),
    update: (id: string, body: any) => requests.put<Form>(`/forms/${id}`, body),
}


const agent = {
    Forms
}

export default agent;