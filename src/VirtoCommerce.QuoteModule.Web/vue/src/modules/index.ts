import * as blades from "./pages";
import * as locales from "./locales";
import { defineAppModule } from "@vc-shell/framework";
import type { App } from "vue";
import { registerQuotesCounter } from "./menuCounter";

const appModule = defineAppModule({
  blades,
  locales,
});

export default {
  install(app: App) {
    appModule.install(app);
    // quotes waiting for a proposal on the menu item
    registerQuotesCounter(app);
  },
};
