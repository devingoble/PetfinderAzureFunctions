import Vue from "vue";
import VueRouter from "vue-router";
import Pets from "../views/Pets.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "pets",
    component: Pets,
    props: true,
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
  scrollBehavior(to, from, savedPosition) {
    return { x: 0, y: 0 };
  },
});

export default router;
