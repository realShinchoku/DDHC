import {useIsAuthenticated} from "@azure/msal-react";
import {Navigate, Outlet, useLocation} from "react-router-dom";

const RequireAuth = () => {
    const isAuthenticated = useIsAuthenticated();
    const location = useLocation();

    if (!isAuthenticated)
        return <Navigate to={'/'} state={{from: location}}/>

    return <Outlet/>

};
export default RequireAuth;