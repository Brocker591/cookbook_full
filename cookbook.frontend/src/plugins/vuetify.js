import { en, de } from "vuetify/locale";

const getLocaleObject = () => {
  let locale = navigator.language;
  if (locale == "en-US") {
    return {
      locale: "en",
      fallback: "de",
      messages: { en, de },
    };
  }
  return {
    locale: "de",
    fallback: "en",
    messages: { en, de },
  };
};

/**
 * plugins/vuetify.js
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import "@mdi/font/css/materialdesignicons.css";
import "vuetify/styles";

// Composables
import { createVuetify } from "vuetify";

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  locale: getLocaleObject(),
});
