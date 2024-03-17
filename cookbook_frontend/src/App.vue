<template>
  <v-app>
    <v-main>
      <v-card class="ma-5">
        <v-tabs v-model="tab" color="deep-purple-accent-4" align-tabs="center">
          <v-tab :value="0" @click="changepage(navigation[0])"
            >{{ getNavigationName(0) }}
          </v-tab>
          <v-tab :value="1" @click="changepage(navigation[1])">{{
            getNavigationName(1)
          }}</v-tab>
          <v-tab :value="2" @click="changepage(navigation[2])">{{
            getNavigationName(2)
          }}</v-tab>
        </v-tabs>
        <v-window v-model="tab">
          <v-window-item
            v-for="navi in navigation"
            :key="navi.index"
            :value="navi.index"
          >
            <v-container fluid>
              <router-view></router-view>
            </v-container>
          </v-window-item>
        </v-window>
      </v-card>
    </v-main>
    <dev>
      <Notification
        :notification="notification"
        :toggleNotification="toggleNotification"
      />
    </dev>
  </v-app>
</template>

<script setup>
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import Notification from "@/components/Notification.vue";
import useNotification from "@/hooks/useNotification.js";

const { notification, toggleNotification } = useNotification();

const router = useRouter();
const tab = ref(null);
const navigation = reactive([
  {
    index: 0,
    name: "Home",
    nav: "home",
  },
  {
    index: 1,
    name: "Rezept liste",
    nav: "recipelist",
  },
  {
    index: 2,
    name: "Neues Rezept",
    nav: "newrecipe",
  },
]);

const getNavigationName = (index) => {
  return navigation[index].name;
};

const changepage = (navi) => {
  router.push({ name: navi.nav });
};
</script>
