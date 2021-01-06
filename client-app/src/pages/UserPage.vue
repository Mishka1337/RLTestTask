<template>
  <div>
    <TopMenu />
        <div style="margin: 10px">
      <UserModalForm ref="user-modal-form" />
      <b-button
        variant="success"
        @click="createNewUser"
      >
        Новый пользователь
      </b-button>
      <div style="margin: 10px"/>
      <b-overlay
        :show="isLoadingUsers"
        spinner-variant="primary"
      >
        <b-table
          responsive
          small
          bordered
          fixed
          stripped
          hover
          selectable
          select-mode="single"
          :items="this.USERS"
          :fields="this.fields"
          @row-selected="onRowSelected"
        >
          <template v-slot:cell(roles)="data">
            {{ data.value.length }}
          </template>
        </b-table>
        <b-button
          variant="outline-primary"
          @click="loadPrevPage"
        >
          &larr;
        </b-button>
        <b-button
          variant="outline-primary"
          @click="loadNextPage"
        >
          &rarr;
        </b-button>
      </b-overlay>
    </div>
  </div>
</template>
<script>
import TopMenu from '@/components/TopMenu';
import UserModalForm from '@/components/UserModalForm';
import { mapGetters, mapActions } from 'vuex';

export default {
  data() {
    return {
      isLoadingUsers: false,
      selectedUser: null,
      currentPage: 0,
      fields: [
        {
          key: 'id',
          label: 'id',
        },
        {
          key: 'login',
          label: 'Логин пользователя',
        },
        {
          key: 'name',
          label: 'Имя пользователя'
        },
        {
          key: 'email',
          label: 'Email',
        },
        {
          key: 'roles',
          label: 'Кол-во ролей пользователя'
        }
      ]
    };
  },

  components:{
    TopMenu,
    UserModalForm,
  },

  mounted() {},

  async created() {
    this.isLoadingUsers = true;
    await this.USERS_REQUEST(this.currentPage);
    this.isLoadingUsers = false;
  },

  beforeDestroy() {},

  computed: {
    ...mapGetters({
      USERS: 'USERS'
    }),
  },

  methods: {
    ...mapActions({
      USERS_REQUEST: 'USERS_REQUEST'
    }),
    onRowSelected(items) {
      if (items.length === 1) {
        this.selectedUser = items[0];
        this.$refs['user-modal-form'].user = this.selectedUser;
        this.$refs['user-modal-form'].edit();
      } else {
        this.selectedUser = null;
      }
    },
    createNewUser() {
      this.$refs['user-modal-form'].user = null;
      this.$refs['user-modal-form'].new();
    },
    async loadNextPage() {
      this.currentPage += 1;
      this.isLoadingUsers = true;
      await this.USERS_REQUEST(this.currentPage);
      this.isLoadingUsers = false;
    },
    async loadPrevPage() {
      this.currentPage -= 1;
      if (this.currentPage < 0) {
        this.currentPage = 0;
      }
      this.isLoadingUsers = true;
      await this.USERS_REQUEST(this.currentPage);
      this.isLoadingUsers = false;
    }
  }
}
</script>