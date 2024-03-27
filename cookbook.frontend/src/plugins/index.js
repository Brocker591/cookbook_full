/**
 * plugins/index.js
 *
 * Automatically included in `./src/main.js`
 */

// Plugins
import vuetify from "./vuetify";
import router from "@/router";
import store from "@/store";
import i18n from "@/i18n.js";

export function registerPlugins(app) {
  app.use(vuetify).use(i18n).use(router).use(store);
}
