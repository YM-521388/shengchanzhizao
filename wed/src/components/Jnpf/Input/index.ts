import { withInstall } from '@/utils';
import Input from './src/Input.vue';
import Textarea from './src/Textarea.vue';
import I18nInput from './src/I18nInput.vue';

export const JnpfInput = withInstall(Input);
export const JnpfTextarea = withInstall(Textarea);
export const JnpfI18nInput = withInstall(I18nInput);
