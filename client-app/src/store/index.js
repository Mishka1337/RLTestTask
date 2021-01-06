import Vue from 'vue';
import Vuex from 'vuex';

import users from './models/Users';
import roles from './models/Roles';


Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        users,
        roles
    }
});