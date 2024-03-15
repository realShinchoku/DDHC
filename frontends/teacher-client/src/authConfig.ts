import {Configuration, LogLevel, PopupRequest, PublicClientApplication} from "@azure/msal-browser";

export const msalConfig: Configuration = {
    auth: {
        clientId: "CLIENT_ID",
        authority: "https://login.microsoftonline.com/TENANT_ID",
        redirectUri: import.meta.env.VITE_REDIRECT_URI,
    },
    cache: {
        cacheLocation: "localStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: true, // Set this to "true" if you are having issues on IE11 or Edge
    },
    system: {
        loggerOptions: {
            loggerCallback: (level, message, containsPii) => {
                if (containsPii) {
                    return;
                }
                switch (level) {
                    case LogLevel.Error:
                        console.error(message);
                        return;
                    case LogLevel.Info:
                        console.info(message);
                        return;
                    case LogLevel.Verbose:
                        console.debug(message);
                        return;
                    case LogLevel.Warning:
                        console.warn(message);
                        return;
                    default:
                        return;
                }
            }
        }
    }
};

export const loginRequest: PopupRequest = {
    scopes: ["api://CLIENT_ID/SCOPES"],
};

export const msalInstance = new PublicClientApplication(msalConfig);
