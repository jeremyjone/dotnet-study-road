<template>
  <div>callback</div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { manager as extManager } from "../utils/extManager";

export default defineComponent({
  name: "CallBack",

  mounted() {
    extManager
      .signinRedirectCallback()
      .then(async result => {
        window.localStorage.setItem("token", result.access_token);
        window.localStorage.setItem("id_token", result.id_token);
        console.log("set token:", result.access_token);

        window.history.replaceState(
          {},
          window.document.title,
          window.location.origin + window.location.pathname
        );

        window.location.href = "/about";
      })
      .catch(err => {
        console.log("err:", err);
      });
  }
});
</script>

<style lang="stylus" scoped></style>
