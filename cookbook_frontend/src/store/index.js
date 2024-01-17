import axios from "axios";
import { createStore } from "vuex";
let clientUrl = import.meta.env.VITE_CLIENT_URL;

let timer;

const store = createStore({
  state: {
    userId: null,
    token: null,
  },
  mutations: {
    setUser(state, payload) {
      state.userId = payload.userId;
      state.token = payload.token;
    },
  },
  actions: {
    signin(context, payload) {
      const authDO = {
        username: payload.username,
        password: payload.password,
        returnSecureToken: true,
      };

      return axios
        .post(clientUrl, authDO)
        .then((response) => {
          const expiresIn = Number(response.data.expiresIn) * 3000;
          const expDate = new Date().getTime() + expiresIn;

          //Daten im LocalStorage speichern
          localStorage.setItem("token", response.data.access_token);
          localStorage.setItem("userId", response.data.userId);
          localStorage.setItem("expiresIn", expDate);

          timer = setTimeout(() => {
            context.dispatch("autoSignout");
          }, expiresIn);

          context.commit("setUser", {
            userId: response.data.localId,
            token: response.data.idToken,
          });
        })
        .catch((error) => {
          const errorMessage = new Error(
            error.response.data.error.message || "UNKNOWN_ERROR"
          );
          throw errorMessage;
        });
    },
    autoSignin(context) {
      const token = localStorage.getItem("token");
      const userId = localStorage.getItem("userId");
      const expiresIn = localStorage.getItem("expiresIn");
      const timeLeft = expiresIn - new Date().getTime();

      if (timeLeft < 0) {
        return;
      }

      timer = setTimeout(() => {
        context.dispatch("autoSignout");
      }, timeLeft);

      if (token && userId) {
        context.commit("setUser", {
          token: token,
          userId: userId,
        });
      }
    },
    signout(context) {
      localStorage.removeItem("token");
      localStorage.removeItem("userId");
      localStorage.removeItem("expiresIn");

      clearTimeout(timer);

      context.commit("setUser", {
        token: null,
        userId: null,
      });
    },
    autoSignout(context) {
      // Server-Kommunikation, falls notwendig
      context.dispatch("signout");
    },
  },
  getters: {
    isAuthenticated: (state) => !!state.token,
    token: (state) => state.token,
  },
});

export default store;
