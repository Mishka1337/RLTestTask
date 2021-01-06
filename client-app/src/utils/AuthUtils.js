import axios from '@/utils/AxiosInstance';
import router from '@/router/router';

var currentUser = JSON.parse(localStorage.getItem('token'));

class AuthUtils {
    login(details) {
        return new Promise((resolve, reject) => {
            axios.post('api/auth', details)
                .then( data => {
                    this.fillTokenInfo(data);
                    resolve(data);
                }).catch((error) => {
                    reject(error);
                });
        });
    }

    async beforSendData() {
        const currentUser = this.currentUser;
        
        if (!currentUser || !this.isTokenFresh) {
            this.logout('#');
        }
    }

    fillTokenInfo({ data }) {
        localStorage.removeItem('token');
        currentUser = null;
        let jwtInfo = JSON.parse(atob(data.access_token.split('.')[1]));
        if (jwtInfo) {
            let info = {
                accessToken: data.access_token,
                user: {
                    login: data.login,
                    roles: data.roles
                },
                expiresAt: jwtInfo.exp
            };

            localStorage.setItem('token',JSON.stringify(info));
            currentUser = info
        }
    }

    logout(url) {
        localStorage.removeItem('token');
        currentUser = null;
        router.push({
            name: 'Login',
            query: {
                returnUrl: url
            }
        });
    }

    get currentUser() {
        return currentUser;
    }

    get isTokenFresh(){
        return this.currentUser !== null &&
            this.currentUser.expiresAt > Date.now() / 1000;
    }

    get isAdmin() {
        let index = currentUser.user.roles.findIndex(u => u === 'admin');
        return index >= 0;
    }
}

export default new AuthUtils();