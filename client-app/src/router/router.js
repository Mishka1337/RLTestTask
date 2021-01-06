import Vue from 'vue';
import VueRouter from 'vue-router';
import {BootstrapVue, IconsPlugin} from 'bootstrap-vue';
import AuthUtils from '@/utils/AuthUtils';
import Error404 from '@/pages/Error404';
import LoginPage from '@/pages/LoginPage';
import UserPage from '@/pages/UserPage';


Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.use(VueRouter);

let router = new VueRouter({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'Index',
            redirect: '/users',
        },
        {
            path: '/login',
            name: 'Login',
            component: LoginPage
        },
        {
            path: '/users',
            name: 'Users',
            meta: {
                authorize: ['admin']
            },
            component: UserPage,
        },
        {
            path: '*',
            name: '404',
            component: Error404,
            meta: {
                authorize: ['admin']
            }
        }
    ]
});

router.beforeEach( async (to, from, next) => {
    const {
        authorize
    } = to.meta;
    const currentUser = AuthUtils.currentUser;

    if (authorize) {
        if (!currentUser || !AuthUtils.isTokenFresh) {
            AuthUtils.logout(to.path);
        }

        if (authorize.length && !authorize.some(r => currentUser.user.roles.includes(r)))
            return next({
                path: '/'
            });
    }
    next();
});

export default router;
