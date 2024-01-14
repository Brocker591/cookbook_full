import { createRouter, createWebHistory } from "vue-router";
import HomePage from "@/pages/HomePage.vue";
import RecipeListPage from "@/pages/RecipeListPage.vue";
import NewRecipePage from "@/pages/NewRecipePage.vue";
import UpdateRecipePage from "@/pages/UpdateRecipePage.vue";
//import { setupLayouts } from "virtual:generated-layouts";

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      alias: "/home",
      component: HomePage,
    },
    {
      path: "/newrecipe",
      name: "newrecipe",
      alias: "/newrecipe",
      component: NewRecipePage,
    },
    {
      path: "/recipelist",
      name: "recipelist",
      alias: "/recipelist",
      component: RecipeListPage,
    },
    {
      path: "/updaterecipe",
      name: "updaterecipe",
      alias: "/updaterecipe",
      component: UpdateRecipePage,
    },
  ],
});

export default router;
