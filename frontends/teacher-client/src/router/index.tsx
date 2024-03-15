import {createBrowserRouter, Navigate, RouteObject} from "react-router-dom";
import App from "src/layout/App";
import RequireAuth from "src/router/RequireAuth.tsx";
import Home from "src/components/home/Home.tsx";
import SubjectCreate from "src/components/create/SubjectCreate.tsx";
import SubjectDetail from "src/components/detail/SubjectDetail.tsx";

export const route = {};

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            {
                element: <RequireAuth/>, children: [
                    {path: '', element: <Home/>},
                    {path: '/subject/create', element: <SubjectCreate/>},
                    {path: '/subject/:id', element: <SubjectDetail/>},
                ]
            },

            {path: '*', element: <Navigate replace to={'/not-found'}/>},
        ],
    }
]
export const router = createBrowserRouter(routes);