<template>
  <b-modal
    id="UserModalForm"
    centered
    ref="user-modal-form"
    :title="title"
    size="lg"
    @hidden="resetFields"
    @ok="onSubmit"
  >
    <b-overlay
      :show="isWaitingResponse"
      spinner-variant="primary"
    >
      <form
        ref="form"
        @submit.stop.prevent="handleSubmit"
      >
        <div>
          <b>Логин</b>
        </div>
        <b-form-input
          id="login-input"
          type="text"
          placeholder="Логин пользователя"
          v-model="login"
          trim
          :disabled="user !== null"
        />

        <div style="margin: 10px" />

        <div>
          <b>Имя пользователя</b>
        </div>
        <b-form-input
          id="name-input"
          type="text"
          placeholder="Имя пользователя"
          trim
          v-model="name"
        />

        <div style="margin: 10px" />

        <div>
          <b>Email</b>
        </div>
        <b-form-input
          id="email-input"
          type="text"
          placeholder="Email адресс пользователя"
          trim
          v-model="email"
        />

        <div style="margin: 10px" />

        <div>
          <b>{{user !== null ? 'Новый пароль' : 'Пароль' }}</b>
        </div>
        <b-form-input
          id="password-input"
          type="password"
          v-model="password"
          :disabled="!isNewPassword"
        />
        <b-form-checkbox
          v-model="isNewPassword"
          switch
          v-if="user !== null"
          class="mr-n2"
          @change="newPasswordStateChanged"
        >
          Я <b>точно</b> хочу поменять пароль
        </b-form-checkbox>

        <div style="margin: 10px" />

        <div>
          <b>Роли пользователя</b>
        </div>
        <b-form-group>
          <b-form-checkbox-group
            v-model="selectedRoles"
            :options="availableRoles"
            switches
          />
        </b-form-group>
      </form>
    </b-overlay>
  </b-modal>
</template>
<script>
import { mapGetters, mapActions } from 'vuex';
export default {
  data(){
    return {
      user: null,
      isWaitingResponse: false,
      login: null,
      name: null,
      email: null,
      password: null,
      availableRoles: [],
      selectedRoles: [],
      isNewPassword: false,
    }
  },
  
  async created() {
    await this.ROLES_REQUEST();
  },
  computed: {
    ...mapGetters({
      ROLES: 'ROLES'
    }),
    title() {
      return this.user !== null ? 'Редактирование' : 'Новый пользователь';
    }
  },
  methods: {
    ...mapActions({
      ROLES_REQUEST: 'ROLES_REQUEST',
      USER_UPDATE: 'USER_UPDATE',
      USER_ADD: 'USER_ADD'
    }),
    edit(){
      this.$nextTick(() => {
        this.id = this.user.id;
        this.login = this.user.login;
        this.name = this.user.name;
        this.email = this.user.email;
        this.password = this.user.password;
        this.isNewPassword = false;
        this.loadRoles();

        this.user.roles.forEach(r => {
          this.selectedRoles.push(r.id);
        });

        this.$bvModal.show('UserModalForm');
      });
    },
    new(){
      this.$nextTick(() => {
        this.resetFields();
        this.isNewPassword = true;
        this.loadRoles();
        this.$bvModal.show('UserModalForm');
      });
    },
    resetFields() {
      this.user = null;
      this.login = null;
      this.name = null;
      this.email = null;
      this.password = null;
      this.availableRoles = [];
      this.selectedRoles = [];
    },
    loadRoles(){
      this.ROLES.forEach(r => {
        this.availableRoles.push({
          text: r.name,
          value: r.id
        });
      });
    },
    newPasswordStateChanged(checked) {
      if (!checked) {
        this.password = null;
      } else {
        this.password = '';
      }
    },
    async onSubmit(event) {
      event.preventDefault();
      if(this.user !== null) {
        this.user.id = this.id;
        this.user.login = this.login;
        this.user.name = this.name;
        this.user.email = this.email;
        this.user.password = this.password;
        this.user.roles = [];

        this.selectedRoles.forEach(r => {
          this.user.roles.push({
            id: r,
            name: this.availableRoles.find(ar => ar.value === r).text,
          });
        });

        try {
          this.isWaitingResponse = true;
          await this.USER_UPDATE(this.user);
          this.isWaitingResponse = false;
          this.showSuccessMessage();
        } catch (error) {
          this.isWaitingResponse = false;
          this.showErrorMessage(error.errorMessage);
        }
      } else {
        let newUser = {
          id: this.id,
          login: this.login,
          name: this.name,
          email: this.email,
          password: this.password,
          roles: [],
        };
        this.selectedRoles.forEach(r => {
          newUser.roles.push({
            id: r,
            name: this.availableRoles.find(ar => ar.value === r).text,
          });
        });

        try {
          this.isWaitingResponse = true;
          await this.USER_ADD(newUser);
          this.isWaitingResponse = false;
          this.showSuccessMessage();
        } catch(error) {
          this.isWaitingResponse = false;
          this.showErrorMessage(error.errorMessage);
        }
      }
    },
    showSuccessMessage() {
      this.$bvModal
        .msgBoxOk('Данные успешно сохранены',{
          title: 'Успех',
          size: 'md',
          buttonSize: 'md',
          okVariant: 'success',
          centered: true,
        })
        .then(() => {});
    },
    showErrorMessage(error) {
      this.$bvModal
        .msgBoxOk('Произошла ошибка при отправке данных...', {
          title: 'Ошибка',
          size: 'md',
          buttonSize: 'md',
          okVariant: 'success',
          centered: true,
        })
        .then(() => {});
    }
  }
}
</script>