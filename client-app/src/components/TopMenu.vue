<template>
  <div>
    <b-navbar
      toggleable="lg"
      type="dark"
      variant="primary"
    >
    <router-link
      tag="b-navbar-brand"
      :to="{ name: 'Index' }"
      :style="{ cursor: 'pointer' }" 
    >
      Управление пользователями
    </router-link>
    <b-navbar-toggle
      target="nav-collapse"
    />
    <b-collapse
      id="nav-collapse"
      is-nav
    >
      <b-navbar-nav v-if="isAdmin">
        <router-link
          tag="b-nav-item"
          active-class="active"
          :to="{ name: 'Users' }"
          exact
        >
          Пользователи
        </router-link>
      </b-navbar-nav>
    </b-collapse>
    </b-navbar>
  </div>
</template>
<script>
import AuthUtils from '@/utils/AuthUtils';
export default {
  data() {
    return {
      isAdmin: AuthUtils.isAdmin,
    }
  }
}
</script>