import { App } from 'vue';
import JnpfUniver from './Design/index.vue';
import JnpfUniverPrint from './Print/Render/index.vue';

JnpfUniver.install = (app: App) => app.component(JnpfUniver.name as string, JnpfUniver);
JnpfUniverPrint.install = (app: App) => app.component(JnpfUniverPrint.name as string, JnpfUniver);

export { JnpfUniver, JnpfUniverPrint };

export default {
  install(app: App) {
    app.component(JnpfUniver.name as string, JnpfUniver);
    app.component(JnpfUniverPrint.name as string, JnpfUniverPrint);
  },
};
