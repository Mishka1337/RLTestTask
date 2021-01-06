<template>
  <div class="body">
    <div class="vue-template">
      <div 
        v-show="errorMessage" 
        class="alert alert-danger failed"
      >
        {{ errorMessage }}
      </div>
      <b-form @submit.stop.prevent="onSubmit">
        <h3>Управление пользователями</h3>
        <b-form-input
          v-model="form.login"
          class="form-group"
          type="text"
          placeholder="Email"
          size="lg"
        />

        <b-form-input
          v-model="form.password"
          class="form-group"
          type="password"
          placeholder="Пароль"
          size="lg"
        />
        <b-button 
          type="submit" 
          class="btn btn-lg btn-block"
        >
          Войти
        </b-button>
      </b-form>
    </div>
  </div>
</template>
<script>
import router from '@/router/router';
import AuthUtils from '@/utils/AuthUtils';
export default {
  data() {
    return {
      form: {
        login: null,
        password: null,
      },
      errorMessage: null,
    }
  },
  created() {
    if (AuthUtils.currentUser) {
      return router.push('/');
    }

    this.returnUrl = this.$route.query.returnUrl || '/';
  },
  methods: {
    onSubmit() {
      let details = {
        login: this.form.login,
        password: this.form.password,
      }

      AuthUtils.login(details)
        .then(() => {
          this.$router.push(this.returnUrl);
        })
        .catch((error) => {
          console.log(error.response);
          if(error.response.status === 400) {
            this.errorMessage = 'Неверный логин или пароль';
            this.form.password = '';
          } else {
            this.errorMessage = 'Произошла какая-то ошибочка';
          }
        });
    }
  }
}
</script>
<style scoped>
.body {
  background: #858585;
  min-height: 100vh;
  display: flex;
  font-weight: 500;
}

.vue-template {
  width: 450px;
  margin: auto;
  background: #ebebeb;
  border-radius: 15px;
  transition: all 0.3s;
  box-shadow: 0px 14px 80px rgba(34,46,58,0.2);
  padding: 40px 40px 40px 40px;
}
</style>