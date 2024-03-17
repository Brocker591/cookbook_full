import { createI18n } from "vue-i18n";
import de from "./i18n/de.json";
import en from "./i18n/en.json";

const currentLocale = navigator.language.split("-")[0];

const i18n = createI18n({
  legacy: false,
  locale: currentLocale,
  fallbackLocale: "en",
  messages: {
    en: en,
    de: de,
  },
});

export default i18n;
