<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />

    <div>
      <button @click="login">登录</button>
      <button @click="logout">登出</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { manager as extManager } from "../utils/extManager";

export default defineComponent({
  name: "App",

  setup() {
    function login() {
      extManager.login();
    }

    function logout() {
      extManager.logout();
      window.localStorage.clear();
    }

    return { login, logout };
  }
});
</script>
