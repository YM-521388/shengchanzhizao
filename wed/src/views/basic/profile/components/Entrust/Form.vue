<template>
  <BasicModal v-bind="$attrs" @register="registerModal" :title="getTitle" @ok="handleSubmit">
    <BasicForm @register="registerForm" :schemas="getSchemas">
      <template #toUserId="{ model, field }">
        <JnpfUserSelect
          v-model:value="model[field]"
          multiple
          @change="onToUserIdChange"
          v-if="getSysConfig[state.dataForm.type === 0 ? 'delegateScope' : 'proxyScope'] === 1" />
        <UserSelect
          v-model:value="model[field]"
          multiple
          @change="onToUserIdChange"
          :query="{ type: getSysConfig[state.dataForm.type === 0 ? 'delegateScope' : 'proxyScope'] }"
          :api="getReceiveUserList"
          v-else />
      </template>
      <template #flowId>
        <flow-select
          v-model:value="state.flowId"
          popupTitle="委托流程"
          :entrustType="state.dataForm.type"
          :toUserId="state.dataForm.toUserId"
          placeholder="全部流程"
          @change="onFlowIdChange" />
      </template>
    </BasicForm>
  </BasicModal>
</template>
<script lang="ts" setup>
  import { getInfo, create, update } from '@/api/workFlow/flowDelegate';
  import { computed, reactive } from 'vue';
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { BasicForm, useForm } from '@/components/Form';
  import { useMessage } from '@/hooks/web/useMessage';
  import dayjs, { Dayjs } from 'dayjs';
  import FlowSelect from '@/views/workFlow/components/FlowSelect.vue';
  import { useI18n } from '@/hooks/web/useI18n';
  import { JnpfUserSelect } from '../../../../../components/Jnpf/Organize/index';
  import { useAppStore } from '@/store/modules/app';
  import { UserSelect } from '@/components/CommonModal';
  import { getReceiveUserList } from '@/api/permission/user';

  const emit = defineEmits(['register', 'reload']);
  const disabledDate = (current: Dayjs) => current && current < dayjs().endOf('day').subtract(1, 'day');
  const checkStartTime = async (_rule, value) => {
    if (!getFieldsValue().endTime) return Promise.resolve();
    if (getFieldsValue().endTime < value) return Promise.reject('开始时间应该小于结束时间');
    validate(['endTime']);
    return Promise.resolve();
  };
  const checkEndTime = async (_rule, value) => {
    if (!getFieldsValue().startTime) return Promise.resolve();
    if (getFieldsValue().startTime > value) return Promise.reject('结束时间应该大于开始时间');
    return Promise.resolve();
  };
  const [registerForm, { setFieldsValue, resetFields, getFieldsValue, validate }] = useForm({
    labelWidth: 90,
  });
  const [registerModal, { closeModal, changeLoading, changeOkLoading }] = useModalInner(init);
  const state = reactive({
    dataForm: {
      id: '',
      toUserName: '',
      toUserId: [],
      userName: '',
      type: 0,
      flowId: '',
      flowName: '全部流程',
    },
    flowId: [],
  });
  const { createMessage } = useMessage();
  const { t } = useI18n();
  const appStore = useAppStore();

  const getSysConfig = computed(() => appStore.getSysConfigInfo);
  const getTitle = computed(() => (!state.dataForm.id ? t('common.addText') : t('common.editText')));
  const getSchemas = computed(() => {
    const title = state.dataForm.type === 0 ? '委托' : '代理';
    const schemas: any[] = [
      {
        field: 'toUserId',
        label: state.dataForm.type === 0 ? '受委托人' : '代理人',
        component: 'UserSelect',
        slot: 'toUserId',
        rules: [{ required: true, trigger: 'blur', message: '必填', type: 'array' }],
      },
      {
        field: 'flowId',
        label: `${title}流程`,
        helpMessage: `未选择${title}流程默认全部流程进行${title}`,
        component: 'Input',
        slot: 'flowId',
      },
      {
        field: 'startTime',
        label: '开始时间',
        component: 'DatePicker',
        componentProps: { format: 'YYYY-MM-DD HH:mm:ss', disabledDate },
        rules: [
          { required: true, message: '必填', trigger: 'change' },
          { validator: checkStartTime, trigger: 'change' },
        ],
      },
      {
        field: 'endTime',
        label: '结束时间',
        component: 'DatePicker',
        componentProps: { format: 'YYYY-MM-DD HH:mm:ss', disabledDate },
        rules: [
          { required: true, message: '必填', trigger: 'change' },
          { validator: checkEndTime, trigger: 'change' },
        ],
      },
      {
        field: 'description',
        label: `${title}说明`,
        component: 'Textarea',
        componentProps: { placeholder: '请输入' },
      },
    ];
    return schemas;
  });

  function init(data) {
    changeLoading(true);
    resetFields();
    state.flowId = [];
    state.dataForm = {
      id: '',
      toUserName: '',
      toUserId: [],
      userName: '',
      type: 0,
      flowId: '',
      flowName: '全部流程',
    };
    state.dataForm.type = data.type || 0;
    state.dataForm.id = data.id;
    if (state.dataForm.id) {
      getInfo(state.dataForm.id).then(res => {
        setFieldsValue(res.data);
        state.dataForm = res.data;
        (state.flowId as string[]) = state.dataForm.flowId ? state.dataForm.flowId.split(',') : [];
        changeLoading(false);
      });
    } else {
      changeLoading(false);
    }
  }
  function onToUserIdChange(id, data) {
    if (!id) {
      state.dataForm.toUserId = [];
      state.dataForm.toUserName = '';
      return;
    }
    state.dataForm.toUserId = id;
    state.dataForm.toUserName = data.map(o => o.fullName).join();
  }
  function onFlowIdChange(_ids, data) {
    if (!data || !data.length) return (state.dataForm.flowName = '全部流程');
    state.dataForm.flowName = data.map(o => o.fullName + '/' + o.enCode).join();
  }
  async function handleSubmit() {
    const values = await validate();
    if (!values) return;
    changeOkLoading(true);
    const query = {
      ...values,
      type: state.dataForm.type,
      flowId: state.flowId.join(),
      flowName: state.dataForm.flowName,
      userName: state.dataForm.userName,
      toUserName: state.dataForm.toUserName,
    };
    const formMethod = state.dataForm.id ? update : create;
    formMethod(query)
      .then(res => {
        createMessage.success(res.msg);
        changeOkLoading(false);
        closeModal();
        emit('reload');
      })
      .catch(() => {
        changeOkLoading(false);
      });
  }
</script>
