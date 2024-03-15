import {defineConfig, splitVendorChunkPlugin} from 'vite'
import react from '@vitejs/plugin-react-swc'
import million from 'million/compiler';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [million.vite({auto: true, mute: true}), react(), splitVendorChunkPlugin()],
    server: {
        port: 3001,
    },
    resolve: {
        alias: {
            src: "/src",
        },
    },
})
