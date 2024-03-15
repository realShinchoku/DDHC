import {makeAutoObservable, reaction} from "mobx";
import Cookies from "js-cookie";

export default class CommonStore {
    token: string | undefined = Cookies.get('jwt')
    appLoaded: boolean = false;

    constructor() {
        makeAutoObservable(this);

        reaction(() => this.token,
            token => {
                if (token)
                    Cookies.set('jwt', token);
                else
                    Cookies.remove('jwt');
            })
    }

    setToken = (token: string | undefined) => {
        this.token = token;
    }

    setAppLoaded = () => this.appLoaded = true;
}