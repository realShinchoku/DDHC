import {createBrowserRouter, Navigate, RouteObject} from "react-router-dom";
import App from "src/layout/App";
import Home from "src/components/home/Home.tsx";


export const route = {};

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            {path: '', element: <Home/>},
            {path: '*', element: <Navigate replace to={'/not-found'}/>},
        ],
    }
]
export const router = createBrowserRouter(routes);