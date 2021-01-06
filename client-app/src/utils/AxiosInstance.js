import Axios from 'axios';
import Vue from 'vue';

const instance = Axios.create({
    baseUrl: process.env.BASEURL,
    headers: {
        "Content-Type": "application/json"
    }
});

instance.interceptors.response.use(
    response => response,
    error => {
        const vm = new Vue();
        if (!error.response) {
            vm.$bvToast.toast('Проверьте соединение с сетью',{
                title: 'Ошибка соединения',
                variant: 'danger'
            });
        } else {
            const message = error.response.data.message || error.response.statusText;
            vm.$bvToast.toast(`${message}`, {
                title: `Ошибка ${error.response.status}`,
                variant: 'danger',
            });
            return Promise.reject(error);
        }
    },
)

export default instance;