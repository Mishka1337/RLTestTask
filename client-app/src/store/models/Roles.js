import Axios from '@/utils/AxiosInstance';
import JwtHeader from '@/utils/JwtHeader';

const state = {
    roles: [],
};

const rolesPath = 'api/role';

const getters = {
    ROLES: state => state.roles,
};

const actions = {
    async ROLES_REQUEST({
        commit
    }) {
        await Axios.get(rolesPath, JwtHeader())
            .then(resp => {
                if (resp && resp.status === 200) {
                    commit('ROLES_SET', resp.data);
                }
            });
    }
}

const mutations = {
    ROLES_SET(state, roles) {
        state.roles = roles;
    }
}

export default {
    state,
    getters,
    actions,
    mutations,
}