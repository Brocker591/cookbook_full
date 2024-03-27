/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from "@/plugins";

// Components
import App from "./App.vue";
//import i18n from "./i18n.js";
// Composables
import { createApp } from "vue";

const app = createApp(App);
//app.use(i18n);

registerPlugins(app);

app.mount("#app");
