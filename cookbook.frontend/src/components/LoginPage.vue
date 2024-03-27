<template>
  <div>
    <v-img
      class="mx-auto my-6"
      max-width="228"
      src="https://cdn.vuetifyjs.com/docs/images/logos/vuetify-logo-v3-slim-text-light.svg"
    ></v-img>

    <v-card
      class="mx-auto pa-12 pb-8"
      max-width="800"
      elevation="8"
      rounded="lg"
    >
      <v-card-title>
        {{ t("header.login") }}
      </v-card-title>
      <v-card-text class="mt-4">
        <form>
          <div class="text-subtitle-1 text-medium-emphasis">
            {{ t("model.account") }}
          </div>
          <v-text-field
            v-model="state.email"
            :error-messages="v$.email.$errors.map((e) => e.$message)"
            required
            @input="v$.email.$touch"
            @blur="v$.email.$touch"
            density="compact"
            :placeholder="t('message.placeholder.email')"
            prepend-inner-icon="mdi-email-outline"
            variant="outlined"
          ></v-text-field>

          <div
            class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
          >
            {{ t("model.password") }}

            <a
              class="text-caption text-decoration-none text-blue"
              href="https://www.google.de"
            >
              {{ t("message.info.forgotPassword") }}</a
            >
          </div>
          <v-text-field
            v-model="state.password"
            :error-messages="v$.password.$errors.map((e) => e.$message)"
            @input="v$.password.$touch"
            @blur="v$.password.$touch"
            :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
            :type="visible ? 'text' : 'password'"
            density="compact"
            :placeholder="t('message.placeholder.password')"
            prepend-inner-icon="mdi-lock-outline"
            variant="outlined"
            @click:append-inner="visible = !visible"
            required
          ></v-text-field>
          <v-btn class="me-4" color="green" @click="submitForm">
            {{ t("button.login") }}
          </v-btn>
          <v-btn @click="clear"> {{ t("button.clear") }} </v-btn>
        </form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup>
import { ref, reactive } from "vue";
import { useVuelidate } from "@vuelidate/core";
import { email, required } from "@vuelidate/validators";
import store from "@/store";
//import { store } from "vue";
import { useI18n } from "vue-i18n";
import useNotification from "@/hooks/useNotification.js";

const { setNotification } = useNotification();
const { t } = useI18n();
const visible = ref(false);

const initialState = {
  email: "",
  password: "",
};

const state = reactive({
  ...initialState,
});

const rule = {
  email: { required, email },
  password: { required },
};

const v$ = useVuelidate(rule, state);

const submitForm = async () => {
  let isValid = await v$.value.$validate();

  if (isValid) {
    await store.dispatch("signin", {
      email: state.email,
      password: state.password,
    });

    if (!store.getters.isAuthenticated) {
      let message = {
        message: t("message.error.errorLogin"),
        icon: "mdi-alert-circle-outline",
        color: "error",
      };
      setNotification(message);
  }
};

const clear = () => {
  v$.value.$reset();

  for (const [key, value] of Object.entries(initialState)) {
    state[key] = value;
  }
};
</script>
