<template>
  <BasicModal v-bind="$attrs" @register="registerModal" title="数据传递" :width="700" @ok="handleSubmit" class="rule-modal">
    <a-tabs size="small" class="node-tabs">
      <a-tab-pane :tab="item.title" v-for="(item, i) in state.assignList" :key="i">
        <div class="common-tip mb-10px">
          {{
            !!state.isSubFlow
              ? '当父流程流转到子流程时，将对应的上一节点表单字段赋值给子流程发起节点'
              : '当节点流转到本节点时，将对应的上一节点的字段赋值给本节点'
          }}
        </div>
        <a-row :gutter="10" v-for="(child, cIndex) in item.ruleList" :key="cIndex" class="mb-10px">
          <a-col :span="2" class="rule-cell">{{ !!state.isSubFlow ? '父流程' : '上节点' }}</a-col>
          <a-col :span="7" class="rule-cell">
            <jnpf-select
              v-model:value="child.parentField"
              :options="handleFormFieldList(item.formFieldList)"
              :fieldNames="{ options: 'children' }"
              showSearch
              allowClear
              class="!w-full" />
          </a-col>
          <a-col :span="4" class="rule-cell mid">赋值给</a-col>
          <a-col :span="2" class="rule-cell">{{ !!state.isSubFlow ? '子流程' : '本节点' }}</a-col>
          <a-col :span="7" class="rule-cell">
            <jnpf-select
              v-model:value="child.childField"
              :options="getChildFieldOptions"
              :fieldNames="{ options: 'options1' }"
              showSearch
              allowClear
              class="!w-full" />
          </a-col>
          <a-col :span="2" class="rule-cell">
            <a-button type="danger" @click="delTransmitRule(i, cIndex)"><i class="icon-ym icon-ym-nav-close" /></a-button>
          </a-col>
        </a-row>
        <div class="table-add-action" @click="addTransmitRule(i)">
          <a-button type="link" preIcon="icon-ym icon-ym-btn-add">新增规则</a-button>
        </div>
      </a-tab-pane>
    </a-tabs>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { BasicModal, useModalInner } from '@/components/Modal';
  import { useMessage } from '@/hooks/web/useMessage';
  import { cloneDeep } from 'lodash-es';
  import { reactive, computed } from 'vue';
  import { systemComponentsList } from '@/components/FormGenerator/src/helper/config';

  interface State {
    assignList: any[];
    isSubFlow: number;
    childFieldOptions: any[];
  }

  const state = reactive<State>({
    assignList: [],
    isSubFlow: 0,
    childFieldOptions: [],
  });

  const props = defineProps(['formConf', 'formFieldsOptions']);
  defineOptions({ inheritAttrs: false });
  const { createMessage } = useMessage();
  const emit = defineEmits(['register', 'change']);
  const [registerModal, { closeModal }] = useModalInner(init);

  const getChildFieldOptions = computed(() => {
    const list = cloneDeep(!!state.isSubFlow ? state.childFieldOptions : props.formFieldsOptions);
    return list.filter(o => !['table', ...systemComponentsList].includes(o?.__config__?.jnpfKey));
  });
  function init(data) {
    state.assignList = data.assignList || [];
    state.isSubFlow = data.isSubFlow || 0;
    state.childFieldOptions = data.childFieldOptions || [];
  }
  function addTransmitRule(i) {
    state.assignList[i].ruleList.push({
      parentField: '',
      childField: '',
      childFieldOptions: [],
    });
  }
  function delTransmitRule(i, cIndex) {
    state.assignList[i].ruleList.splice(cIndex, 1);
  }
  function handleSubmit() {
    let boo = true;
    for (let i = 0; i < state.assignList.length; i++) {
      const e = state.assignList[i];
      const ruleList = e.ruleList;
      for (let j = 0; j < ruleList.length; j++) {
        if (!ruleList[j].parentField) {
          boo = false;
          createMessage.error(`请选择${e.title}的上节点字段`);
          break;
        }
        if (!ruleList[j].childField) {
          boo = false;
          createMessage.error(`请选择${e.title}的本节点字段`);
          break;
        }
      }
    }
    if (!boo) return;
    props.formConf.assignList = state.assignList;
    state.assignList = [];
    closeModal();
  }
  // 过滤禁用
  function handleFormFieldList(formFieldList: any[]) {
    // 如果没有数据或者数据没有变化，直接返回
    if (!formFieldList?.length) return formFieldList;
    formFieldList.forEach((item: any) => {
      if (item.children?.length) {
        item.children.forEach((childrenItem: any) => {
          if (childrenItem.disabled !== false) {
            childrenItem.disabled = false;
          }
        });
      }
    });
    return formFieldList;
  }
</script>
