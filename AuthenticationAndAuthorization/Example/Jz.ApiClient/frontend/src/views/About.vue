<template>
  <div>
    <h1>About Page</h1>
    <h1 v-if="unauthorization">{{ info }}</h1>

    <div
      v-else
      v-for="(weather, index) in weathers"
      :key="index"
      style="margin-top: 20px"
    >
      <span style="margin-left: 20px">{{ weather.date }}</span>
      <span style="margin-left: 20px">{{ weather.temperatureC }}</span>
      <span style="margin-left: 20px">{{ weather.temperatureF }}</span>
      <span style="margin-left: 20px">{{ weather.summary }}</span>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios";

interface IWeather {
  date: Date;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export default defineComponent({
  name: "About",

  mounted() {
    const token = window.localStorage.getItem("token");

    if (token) {
      axios
        .get("/api/WeatherForecast", {
          headers: {
            Authorization: `Bearer ${token}`,
            ["Content-Type"]:
              "application/json;application/x-www-form-urlencoded;charset=UTF-8"
          }
        })
        .then(res => {
          console.log("success", res);
          this.weathers = res.data as IWeather[];
          this.unauthorization = false;
        })
        .catch(err => {
          console.log("err:", err);
          this.unauthorization = true;
          this.info = err;
        });
    } else {
      this.info = "token is null";
      this.unauthorization = true;
    }
  },

  data() {
    return {
      weathers: [] as IWeather[],
      unauthorization: true,
      info: ""
    };
  }
});
</script>
