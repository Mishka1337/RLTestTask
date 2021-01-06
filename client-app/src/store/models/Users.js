import Axios from '@/utils/AxiosInstance';
import JwtHeader from '@/utils/JwtHeader';


const userPath = 'api/user';

const state = {
    users: []
};

const getters = {
    USERS: state => state.users
}

const actions = {
    async USERS_REQUEST({commit}, pageNumber) {
        await Axios.get(userPath, {
            ...JwtHeader(),
            params: {
                page: pageNumber,
            },
        })
            .then(resp => {
                if(resp && resp.status === 200) {
                    commit('USERS_SET', resp.data);
                }
            });
    },

    async USER_ADD({ commit }, user) {
        return new Promise((resolve, reject) => {
            Axios.post(userPath, user, JwtHeader())
                .then(resp => {
                    if (resp && resp.status === 200) {
                        commit('USER_ADD', resp.data);
                        resolve();
                    } else {
                        reject('Ошибочка вышла...');
                    }
                })
                .catch(err => {
                    if(err.response) {
                        reject(err.response.data);
                    } else {
                        reject('Неизвестная ошибочка вышла...');
                    }
                });
        });
    },

    async USER_UPDATE({commit}, user) {
        return new Promise((resolve, reject) => {
            Axios.patch(userPath, user, JwtHeader())
                .then(resp => {
                    if (resp && resp.status === 200) {
                        commit('USER_UPDATE', resp.data);
                        resolve();
                    } else {
                        reject('Ошибочка вышла...');
                    }
                })
                .catch(err =>{
                    if (err.response) {
                        reject(err.response.data);
                    } else {
                        reject('Неизвестная ошибочка вышла...');
                    }
                });
        });
    },

    async USER_DELETE({commit}, user) {
        return new Promise((resolve, reject) => {
            Axios.delete(userPath + '/' + user.id, JwtHeader())
                .then(resp => {
                    if (resp && resp.status === 200) {
                        commit('USER_DELETE', user);
                        resolve();
                    } else {
                        reject('Ошибочка вышла...');
                    }
                })
                .catch(err => {
                    if (err.response) {
                        reject(err.response.data);
                    } else {
                        reject('Неизвестная ошибочка вышла')
                    }
                });
        });
    }
}

const mutations = {
    USERS_SET(state, users) {
        state.users = users;
    },
    USER_ADD(state, user) {
        state.users.push(user);
    },
    USER_UPDATE(state, user) {
        let i = state.users.findIndex(u => u.id === user.id);
        state.users[i] = user;
    },
    USER_DELETE(state, user) {
        let i = state.users.findIndex(u => u.Iid === user.id);
        state.splice(i,1);
    }
}

export default {
    state,
    getters,
    actions,
    mutations
}