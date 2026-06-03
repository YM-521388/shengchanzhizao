<template>
  <HeaderContainer :formConf="formConf" @onNodeNameChange="updateBpmnProperties('nodeName', $event)" />
  <a-tabs v-model:activeKey="activeKey" size="small" class="pane-tabs approver-pane-tabs">
    <a-tab-pane :key="1" tab="基础信息" />
    <a-tab-pane :key="2" tab="高级设置" />
    <a-tab-pane :key="3" tab="表单权限" />
    <a-tab-pane :key="5" tab="流程通知" />
    <a-tab-pane :key="6" tab="超时设置" />
  </a-tabs>
  <template v-if="activeKey !== 3">
    <a-form :colon="false" :model="formConf" class="config-content" layout="vertical">
      <template v-if="activeKey === 1">
        <template v-if="getJnpfGlobalData?.hasAloneConfigureForms">
          <a-form-item class="form-item-reload">
            <template #label>表单设置<BasicHelp text="办理节点不设置表单，默认引用发起节点表单" /></template>
            <FlowFormModal :value="formConf.formId" :title="formConf.formName" @change="onFormIdChange" placeholder="请选择" :disabled="flowState != 0" />
            <a-tooltip title="刷新表单" v-if="flowState == 0 && formConf.formId">
              <a-button @click="handleRefreshFormField">
                <ReloadOutlined :spin="formLoading" />
              </a-button>
            </a-tooltip>
          </a-form-item>
          <a-form-item v-if="true">
            <template #label>
              数据传递
              <BasicHelp :text="['不设置传递规则时字段名称相同自动赋值', '设置传递规则时相同名称字段会自动赋值字段后再按传递规则赋值']" />
            </template>
            <a-input :value="getHasAssign ? '已设置' : ''" placeholder="请设置数据传递规则" readonly class="hand" @click="openTransmitRuleBox">
              <template #suffix>
                <span class="ant-select-arrow"><down-outlined /></span>
              </template>
            </a-input>
          </a-form-item>
        </template>
        <a-form-item label="办理设置">
          <jnpf-radio
            v-model:value="formConf.assigneeType"
            :options="typeOptions.filter(o => o.id != 5 && o.id != 7)"
            class="type-radio"
            @change="onAssigneeTypeChange" />
          <div :class="{ 'mb-10px': formConf.assigneeType !== 3, 'mt-10px': true }">
            <div v-if="[1, 2].includes(formConf.assigneeType)" class="common-tip">选择主管</div>
            <div v-if="formConf.assigneeType === 3" class="common-tip">发起者自己将作为办理人处理办理单</div>
            <div v-if="formConf.assigneeType === 4" class="common-tip">选择流程表单字段的值作为办理人</div>
            <div v-if="formConf.assigneeType === 6" class="common-tip">指定办理人处理办理单</div>
            <div v-if="formConf.assigneeType === 9" class="common-tip">从目标接口中获取办理人</div>
          </div>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType == 1">
            <div class="countersign-cell">
              <jnpf-select v-model:value="formConf.approverType" :options="approverTypeOptions" class="!w-130px mr-8px" />
              的
              <a-select v-model:value="formConf.managerLevel" placeholder="请选择" class="flex-1 ml-8px">
                <a-select-option v-for="item in 10" :key="item" :value="item">{{ item === 1 ? '直属主管' : '第' + item + '级主管' }}</a-select-option>
              </a-select>
            </div>
          </a-form-item>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType == 2">
            <div class="countersign-cell">
              <jnpf-select v-model:value="formConf.managerApproverType" :options="approverTypeOptions" class="!w-130px mr-8px" />
              的部门主管
            </div>
          </a-form-item>
          <a-form-item label="表单字段" class="!mb-0" v-if="formConf.assigneeType === 4">
            <a-input-group compact>
              <jnpf-select v-model:value="formConf.formFieldType" :options="formFieldTypeOptions" class="!w-100px" @change="onFormFieldTypeChange" />
              <jnpf-select
                v-model:value="formConf.formField"
                :options="getFormFieldOptions"
                :fieldNames="{ options: 'options1' }"
                showSearch
                style="width: calc(100% - 100px)" />
            </a-input-group>
          </a-form-item>
          <a-form-item label="办理节点" class="!mb-0" v-if="formConf.assigneeType === 5">
            <jnpf-select v-model:value="formConf.approverNodeId" showSearch :options="nodeOptions" />
          </a-form-item>
          <a-form-item class="!mb-0" v-if="formConf.assigneeType === 9">
            <div class="ant-form-item-label">
              <label class="ant-form-item-no-colon">数据接口</label>
              <BasicHelp text='请求自带参数：taskId、taskNodeId，返回结构：JSON对象{"handleId":"id1,id2"}' />
            </div>
            <interface-modal :value="formConf.interfaceConfig.interfaceId" :title="formConf.interfaceConfig.interfaceName" @change="onInterfaceChange" />
            <template v-if="formConf.interfaceConfig.templateJson?.length">
              <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">参数设置</label></div>
              <select-interface-btn
                :templateJson="formConf.interfaceConfig.templateJson"
                :fieldOptions="funcOptions"
                isFlow
                showFieldFullLabel
                showSystemFullLabel
                @fieldChange="onRelationFieldChange" />
            </template>
          </a-form-item>
          <div v-if="formConf.assigneeType === 6 || formConf.assigneeType === 7">
            <jnpf-users-select
              v-model:value="formConf.approvers"
              buttonType="button"
              :modalTitle="formConf.assigneeType === 6 ? '添加办理人' : '添加候选人'"
              multiple
              @Change="onApproversChange"
              @labelChange="onLabelChange" />
          </div>
          <a-form-item class="!mb-0 !mt-10px" v-if="formConf.assigneeType === 6 || (formConf.assigneeType === 7 && formConf.approvers?.length)">
            <template #label>
              {{ formConf.assigneeType === 6 ? '办理人范围' : '候选人范围' }}
              <BasicHelp :text="formConf.assigneeType === 6 ? '指定成员增加人员选择范围附加条件' : '候选人增加人员选择范围附加条件'" />
            </template>
            <jnpf-select v-model:value="formConf.extraRule" :options="formConf.assigneeType === 6 ? extraRuleOptions : candidateExtraRuleOptions" />
          </a-form-item>
        </a-form-item>
        <a-form-item label="办理方式">
          <jnpf-radio v-model:value="formConf.counterSign" :options="counterSignOptions" class="counterSign-radio" />
        </a-form-item>
        <a-form-item label="设置会签流转规则" v-if="formConf.counterSign == 1">
          <div class="countersign-cell mb-10px">
            <span class="w-70px inline-block">同意规则</span>
            <jnpf-select v-model:value="formConf.counterSignConfig.auditType" :options="auditTypeOptions" class="!w-120px !mx-10px" />
            <a-select v-model:value="formConf.counterSignConfig.auditRatio" class="!w-120px !mr-10px" v-if="formConf.counterSignConfig.auditType === 1">
              <a-select-option v-for="item in 10" :key="item" :value="item * 10">{{ item * 10 + '%' }}</a-select-option>
            </a-select>
            <a-input-number
              class="!w-120px !mr-10px"
              v-model:value="formConf.counterSignConfig.auditNum"
              placeholder="请输入"
              :precision="0"
              :min="1"
              @change="onSignNumberChange($event, 'auditNum')"
              v-if="formConf.counterSignConfig.auditType === 2" />
            <span>进入下一节点</span>
          </div>
          <div class="countersign-cell">
            <span class="w-70px inline-block">不同意规则</span>
            <jnpf-select v-model:value="formConf.counterSignConfig.rejectType" :options="rejectTypeOptions" class="!w-120px !mx-10px" />
            <a-select v-model:value="formConf.counterSignConfig.rejectRatio" class="!w-120px !mr-10px" v-if="formConf.counterSignConfig.rejectType === 1">
              <a-select-option v-for="item in 10" :key="item" :value="item * 10">{{ item * 10 + '%' }}</a-select-option>
            </a-select>
            <a-input-number
              class="!w-120px !mr-10px"
              v-model:value="formConf.counterSignConfig.rejectNum"
              placeholder="请输入"
              :precision="0"
              :min="1"
              @change="onSignNumberChange($event, 'rejectNum')"
              v-if="formConf.counterSignConfig.rejectType === 2" />
            <span v-if="formConf.counterSignConfig.rejectType">时拒绝</span>
          </div>
        </a-form-item>
        <a-form-item v-if="formConf.counterSign == 1">
          <template #label>计算方式<BasicHelp :text="['实时计算：办理过程中实时进行规则判断', '延后计算：在所有办理人办理完成后进行规则判断']" /></template>
          <jnpf-radio v-model:value="formConf.counterSignConfig.calculateType" :options="calculateTypeOptions" />
        </a-form-item>
        <a-form-item label="设置依次办理顺序" v-if="getCanSetApproversSort">
          <a-button preIcon="icon-ym icon-ym-btn-add" @click="openApproversSortListModal">设置办理顺序</a-button>
        </a-form-item>
        <a-form-item label="办理意见">
          <a-checkbox v-model:checked="formConf.hasSign">启用签名</a-checkbox>
          <div class="mt-10px">
            <a-checkbox v-model:checked="formConf.hasFile">启用附件</a-checkbox>
          </div>
          <jnpf-select
            v-if="formConf.hasApprovalField"
            v-model:value="formConf.approvalField"
            :options="getApprovalFieldOptions"
            :fieldNames="{ label: 'fieldName', options: 'options1' }"
            multiple
            class="mt-10px" />
        </a-form-item>
        <a-form-item label="抄送设置">
          <jnpf-users-select v-model:value="formConf.circulateUser" buttonType="button" modalTitle="添加抄送人" multiple />
          <a-form-item class="!pt-10px">
            <template #label>抄送人范围<BasicHelp text="抄送人员增加人员选择范围附加条件" /></template>
            <jnpf-select v-model:value="formConf.extraCopyRule" :options="extraCopyRuleOptions" />
          </a-form-item>
          <a-checkbox v-model:checked="formConf.isCustomCopy">允许自选抄送人</a-checkbox>
          <div class="mt-10px">
            <a-checkbox v-model:checked="formConf.isInitiatorCopy">抄送给流程发起人</a-checkbox>
          </div>
          <div class="mt-10px">
            <a-checkbox v-model:checked="formConf.isFormFieldCopy">抄送给表单变量</a-checkbox>
          </div>
          <div v-if="formConf.isFormFieldCopy" class="common-tip my-10px">选择流程表单字段的值作为抄送人</div>
          <a-form-item class="!mb-0" v-if="formConf.isFormFieldCopy">
            <a-input-group compact>
              <jnpf-select v-model:value="formConf.copyFormFieldType" :options="formFieldTypeOptions" class="!w-100px" @change="onCopyFormFieldTypeChange" />
              <jnpf-select
                v-model:value="formConf.copyFormField"
                :options="getCopyFormFieldOptions"
                :fieldNames="{ options: 'options1' }"
                showSearch
                style="width: calc(100% - 100px)" />
            </a-input-group>
          </a-form-item>
        </a-form-item>
      </template>
      <template v-if="activeKey === 2">
        <a-form-item label="处置操作">
          <div class="btn-cell">
            <a-checkbox v-model:checked="formConf.hasAuditBtn">办理</a-checkbox>
            <a-input v-model:value="formConf.auditBtnText" placeholder="请输入" />
          </div>
          <div class="btn-cell">
            <a-checkbox v-model:checked="formConf.hasBackBtn">退回</a-checkbox>
            <a-input v-model:value="formConf.backBtnText" placeholder="请输入" />
          </div>
          <div class="btn-cell">
            <a-checkbox v-model:checked="formConf.hasTransferBtn">转办</a-checkbox>
            <a-input v-model:value="formConf.transferBtnText" placeholder="请输入" />
          </div>
          <div class="btn-cell">
            <a-checkbox v-model:checked="formConf.hasSaveAuditBtn">暂存</a-checkbox>
            <a-input v-model:value="formConf.saveAuditBtnText" placeholder="请输入" />
          </div>
        </a-form-item>
        <a-form-item v-if="formConf.hasBackBtn">
          <template #label>退回设置<BasicHelp text="纯表单流程设置退回到发起节点无效" /></template>
          <div class="form-item-content"></div>
          <a-form-item label="被退回的节点重新提交时">
            <a-radio-group v-model:value="formConf.backType" class="counterSign-radio" @change="onBackTypeChange">
              <a-radio :value="1">重新办理<BasicHelp text="若流程为A->B->C，C退回至A，则C->A->B->C" /></a-radio>
              <a-radio :value="2">从当前节点办理<BasicHelp text="若流程为A->B->C，C退回至A，则C->A->C" /></a-radio>
              <a-radio :value="3">自定义办理<BasicHelp text="由用户选择重新办理或从当前节点办理" /></a-radio>
            </a-radio-group>
          </a-form-item>
          <a-form-item label="设置退回到的节点" class="!mb-0px">
            <jnpf-select v-model:value="formConf.backNodeCode" :options="backNodeCodeOptions" showSearch />
          </a-form-item>
        </a-form-item>
        <a-divider><text class="approver-text">自定义按钮区</text></a-divider>
        <a-form-item class="custom-draggable-list">
          <draggable v-model="formConf.customBtns" :animation="300" group="selectItem" handle=".option-drag" itemKey="value">
            <template #item="{ element, index }">
              <div class="custom-draggable-item mb-10px">
                <div class="custom-line-icon option-drag">
                  <i class="icon-ym icon-ym-darg" />
                </div>
                <a-input-group class="btn-cell ml-15px mr-15px" style="margin-bottom: 0" compact>
                  <a-input v-model:value="element.label" :maxlength="4" @blur="setDefaultValue({ element, index })" />
                  <a-button @click="editStyle(element, index)">事件</a-button>
                </a-input-group>
                <div class="close-btn custom-line-icon" @click="formConf.customBtns.splice(index, 1)">
                  <i class="icon-ym icon-ym-btn-clearn" />
                </div>
              </div>
            </template>
          </draggable>
          <div class="add-btn" v-if="formConf.customBtns?.length < 5">
            <a-button type="link" preIcon="icon-ym icon-ym-btn-add" @click="addCustomBtn()">添加按钮</a-button>
          </div>
        </a-form-item>
        <a-form-item label="节点打印">
          <a-checkbox class="leading-32px" v-model:checked="formConf.printConfig.on">节点打印</a-checkbox>
          <template v-if="formConf.printConfig.on">
            <a-form-item label="请选择打印模板" class="!mt-12px">
              <jnpf-tree-select
                v-model:value="formConf.printConfig.printIds"
                :options="printTplOptions"
                multiple
                lastLevel
                :showCheckedStrategy="TreeSelect.SHOW_CHILD" />
            </a-form-item>
            <a-form-item label="打印条件">
              <jnpf-select v-model:value="formConf.printConfig.conditionType" :options="printConditionTypeOptions" />
            </a-form-item>
            <template v-if="formConf.printConfig.conditionType == 4">
              <a-button @click="handlePrintConfig">设置打印条件</a-button>
              <div class="conditions-content" v-if="formConf.printConfig.conditions.length">
                <span @click="handlePrintConfig"> {{ getPrintConditionsContent }}</span>
                <i class="icon-ym icon-ym-delete" @click="handlePrintConfigRemove"></i>
              </div>
            </template>
          </template>
        </a-form-item>
        <a-form-item label="节点参数">
          <div class="parameter-content">
            <span>参数设置</span>
            <i class="icon-ym icon-ym-btn-add" @click="handleAddParameter"></i>
          </div>
          <a-row :gutter="8" class="condition-main mt-10px" v-for="(item, index) in formConf.parameterList" :key="index">
            <a-col :span="8" class="overflow-auto">
              <jnpf-select
                v-model:value="item.field"
                :options="getParameterList"
                :fieldNames="{ label: 'fieldName', value: 'fieldName', options: 'options1' }" />
            </a-col>
            <a-col :span="5" class="overflow-auto">
              <jnpf-select v-model:value="item.fieldValueType" :options="conditionTypeOptions" @change="onFieldValueTypeChange(item)" />
            </a-col>
            <a-col :span="9" class="overflow-auto">
              <jnpf-select
                v-model:value="item.fieldValue"
                :options="usedFormItems"
                allowClear
                showSearch
                :fieldNames="{ options: 'options1' }"
                class="w-100%"
                @change="(val, data) => onFieldValueChange(item, val, data)"
                v-if="item.fieldValueType === 1" />
              <div class="w-100%" v-if="item.fieldValueType === 2">
                <template v-if="item.jnpfKey === 'inputNumber'">
                  <a-input-number v-model:value="item.fieldValue" placeholder="请输入" :precision="item.precision" />
                </template>
                <template v-else-if="item.jnpfKey === 'calculate'">
                  <a-input-number v-model:value="item.fieldValue" placeholder="请输入" :precision="2" />
                </template>
                <template v-else-if="['rate', 'slider'].includes(item.jnpfKey)">
                  <a-input-number v-model:value="item.fieldValue" placeholder="请输入" />
                </template>
                <template v-else-if="item.jnpfKey === 'switch'">
                  <jnpf-switch v-model:value="item.fieldValue" />
                </template>
                <template v-else-if="item.jnpfKey === 'timePicker'">
                  <jnpf-time-picker v-model:value="item.fieldValue" :format="item.format || 'HH:mm:ss'" allowClear />
                </template>
                <template v-else-if="['datePicker', 'createTime', 'modifyTime'].includes(item.jnpfKey)">
                  <jnpf-date-picker v-model:value="item.fieldValue" :format="item.format" allowClear @change="onConditionDateChange($event, item)" />
                </template>
                <template v-else-if="['organizeSelect', 'currOrganize'].includes(item.jnpfKey)">
                  <jnpf-organize-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionOrganizeChange(item, val, data)" />
                </template>
                <template v-else-if="['depSelect'].includes(item.jnpfKey)">
                  <jnpf-dep-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="item.jnpfKey === 'roleSelect'">
                  <jnpf-role-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="item.jnpfKey === 'groupSelect'">
                  <jnpf-group-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="['posSelect', 'currPosition'].includes(item.jnpfKey)">
                  <jnpf-pos-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="['userSelect', 'createUser', 'modifyUser'].includes(item.jnpfKey)">
                  <jnpf-user-select v-model:value="item.fieldValue" hasSys allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="['usersSelect'].includes(item.jnpfKey)">
                  <jnpf-users-select v-model:value="item.fieldValue" allowClear @change="(val, data) => onConditionObjChange(item, val, data)" />
                </template>
                <template v-else-if="item.jnpfKey === 'areaSelect'">
                  <jnpf-area-select
                    v-model:value="item.fieldValue"
                    :level="item.level"
                    allowClear
                    @change="(val, data) => onConditionListChange(item, val, data)" />
                </template>
                <template v-else>
                  <a-input v-model:value="item.fieldValue" placeholder="请输入" allowClear />
                </template>
              </div>
              <jnpf-select
                v-model:value="item.fieldValue"
                :options="getSystemFieldOptions"
                :fieldNames="{ label: 'label', options: 'options1' }"
                allowClear
                class="w-100%"
                v-else-if="item.fieldValueType === 3" />
            </a-col>
            <a-col :span="2" class="text-center">
              <i class="icon-ym icon-ym-btn-clearn" @click="handleDelItem(index)" />
            </a-col>
          </a-row>
        </a-form-item>
        <a-form-item v-if="props.type != 1" label="分流规则">
          <jnpf-select v-model:value="formConf.divideRule" :options="divideRuleOptions" @change="handleDivideRule()" />
        </a-form-item>
      </template>
      <template v-if="activeKey === 5">
        <a-alert message="自定义通知以“消息中心-发送配置”为主，请移步设置；关闭：表示不提醒，默认：表示站内提醒" type="warning" showIcon class="!mb-10px" />
        <!-- 节点退回 -->
        <NoticeConfig :noticeConfig="formConf.backMsgConfig" type="back" v-bind="getBindValue" />
        <!-- 节点抄送 -->
        <NoticeConfig :noticeConfig="formConf.copyMsgConfig" type="copy" v-bind="getBindValue" />
        <!-- 节点超时 -->
        <NoticeConfig :noticeConfig="formConf.overTimeMsgConfig" type="overTime" v-bind="getBindValue" />
        <!-- 节点提醒 -->
        <NoticeConfig :noticeConfig="formConf.noticeMsgConfig" type="notice" v-bind="getBindValue" />
      </template>
      <template v-if="activeKey === 6">
        <a-form-item label="限时设置">
          <jnpf-select v-model:value="formConf.timeLimitConfig.on" :options="nodeOverTimeMsgOptions" @change="onTimeLimitChange" />
          <template v-if="formConf.timeLimitConfig.on === 1">
            <a-form-item class="!mt-12px" label="节点处理截止时间">
              <div class="flex items-center">
                <jnpf-select v-model:value="formConf.timeLimitConfig.nodeLimit" :options="overTimeOptions" />
                <span class="!ml-10px flex-shrink-0">开始</span>
              </div>
              <div class="mt-10px" v-if="formConf.timeLimitConfig.nodeLimit === 2">
                <jnpf-select
                  v-model:value="formConf.timeLimitConfig.formField"
                  :options="usedFormItems"
                  :fieldNames="{ options: 'options1' }"
                  showSearch
                  allowClear />
              </div>
              <div class="countersign-cell mt-12px">
                <span class="w-85px inline-block">流程到达节点</span>
                <a-input-number
                  v-model:value="formConf.timeLimitConfig.duringDeal"
                  @change="e => handleSelectChange(e, 'timeLimitConfig.duringDeal', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时后，视为超时</span>
              </div>
            </a-form-item>
            <a-form-item label="限时提醒规则" class="!mt-12px">
              <jnpf-select v-model:value="formConf.noticeConfig.on" :options="nodeOverTimeMsgOptions" />
            </a-form-item>
            <a-form-item v-if="formConf.noticeConfig.on === 1">
              <div class="countersign-cell">
                <span class="w-85px inline-block">流程到达节点</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.firstOver"
                  @change="e => handleSelectChange(e, 'noticeConfig.firstOver', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时，开始提醒通知</span>
              </div>
              <div class="countersign-cell mt-12px">
                <span class="w-85px inline-block">每间隔</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.overTimeDuring"
                  @change="e => handleSelectChange(e, 'noticeConfig.overTimeDuring', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>小时，提醒通知一次</span>
              </div>
              <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">提醒事务</label></div>
              <a-row class="leading-32px block">
                <a-checkbox v-model:checked="formConf.noticeConfig.overNotice">
                  提醒通知<BasicHelp text="勾选后才能进行提醒消息发送（站内信系统默认发送，第三方超时消息需在节点通知内配置）" />
                </a-checkbox>
              </a-row>
              <a-row class="leading-32px block" v-if="false">
                <a-checkbox v-model:checked="formConf.noticeConfig.overEvent">提醒事件<BasicHelp text="请在节点事件内配置提醒事件" /></a-checkbox>
              </a-row>
              <div class="countersign-cell mt-12px" v-if="formConf.noticeConfig.overEvent">
                <span class="w-85px inline-block">在提醒通知第</span>
                <a-input-number
                  v-model:value="formConf.noticeConfig.overEventTime"
                  @change="e => handleSelectChange(e, 'noticeConfig.overEventTime', 1)"
                  :min="1"
                  :precision="0"
                  class="!w-90px !mx-8px" />
                <span>次时，执行提醒事件</span>
              </div>
            </a-form-item>
          </template>
        </a-form-item>
        <a-form-item label="超时设置">
          <jnpf-select v-model:value="formConf.overTimeConfig.on" :options="nodeOverTimeMsgOptions" :disabled="formConf.timeLimitConfig.on == 0" />
        </a-form-item>
        <a-form-item v-if="formConf.overTimeConfig.on === 1">
          <div class="countersign-cell">
            <span class="w-85px inline-block">超时</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.firstOver"
              @change="e => handleSelectChange(e, 'overTimeConfig.firstOver', 0)"
              :min="0"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>小时，开始超时通知</span>
          </div>
          <div class="countersign-cell mt-12px">
            <span class="w-85px inline-block">每间隔</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overTimeDuring"
              @change="e => handleSelectChange(e, 'overTimeConfig.overTimeDuring', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-8px" />
            <span>小时，超时通知一次</span>
          </div>
          <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">超时事务</label></div>
          <a-row class="leading-32px block">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overNotice">
              超时通知<BasicHelp text="勾选后才能进行提醒消息发送（站内信系统默认发送，第三方超时消息需在节点通知内配置）" />
            </a-checkbox>
          </a-row>
          <a-row class="leading-32px block" v-if="false">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overEvent">超时事件<BasicHelp text="请在节点事件内配置提醒事件" /></a-checkbox>
          </a-row>
          <div class="countersign-cell" v-if="formConf.overTimeConfig.overEvent">
            <span class="w-85px inline-block">在超时通知</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overEventTime"
              @change="e => handleSelectChange(e, 'overTimeConfig.overEventTime', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-5px" />
            <span>次时，执行超时事件</span>
          </div>
          <a-row class="leading-32px block">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overAutoApprove" @change="onOverAutoApproveChange(formConf.overTimeConfig.overAutoApprove)">
              超时自动办理
              <BasicHelp text="当前办理节点表单必填字段为空工单流转时不做校验，下一办理节点设置候选人员、选择分支、异常节点时当前办理节点规则失效" />
            </a-checkbox>
          </a-row>
          <div class="countersign-cell" v-if="formConf.overTimeConfig.overAutoApprove">
            <span class="w-85px inline-block">在超时通知</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overAutoApproveTime"
              @change="e => handleSelectChange(e, 'overTimeConfig.overAutoApproveTime', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-5px" />
            <span>次，执行超时自动办理</span>
          </div>
          <a-row class="leading-32px block">
            <a-checkbox v-model:checked="formConf.overTimeConfig.overAutoTransfer" @change="onOverAutoTransferChange(formConf.overTimeConfig.overAutoTransfer)">
              超时自动转办
            </a-checkbox>
          </a-row>
          <div class="countersign-cell" v-if="formConf.overTimeConfig.overAutoTransfer">
            <span class="w-85px inline-block">在超时通知</span>
            <a-input-number
              v-model:value="formConf.overTimeConfig.overAutoTransferTime"
              @change="e => handleSelectChange(e, 'overTimeConfig.overAutoTransferTime', 1)"
              :min="1"
              :precision="0"
              class="!w-90px !mx-5px" />
            <span>次，执行超时自动转办</span>
          </div>
          <div v-if="formConf.overTimeConfig.overAutoTransfer" class="transfer-tip !mt-12px">转办人设置</div>
          <jnpf-radio
            v-if="formConf.overTimeConfig.overAutoTransfer"
            v-model:value="formConf.overTimeConfig.overTimeType"
            :options="transferTypeOptions"
            class="type-radio transfer-radio" />
          <template v-if="formConf.overTimeConfig.overAutoTransfer">
            <div v-if="formConf.overTimeConfig.overTimeType === 6" class="common-tip !mt-12px">指定转办人处理办理单</div>
            <div v-if="formConf.overTimeConfig.overTimeType === 9" class="common-tip !mt-12px">从目标接口中获取办理人</div>
            <div v-if="formConf.overTimeConfig.overTimeType === 11" class="common-tip !mt-12px">选择与超时办理人相关的设置</div>
          </template>
        </a-form-item>
        <template v-if="formConf.overTimeConfig.overAutoTransfer && formConf.overTimeConfig.on === 1">
          <a-form-item class="!mb-0" v-if="formConf.overTimeConfig.overTimeType === 6">
            <jnpf-users-select v-model:value="formConf.overTimeConfig.reApprovers" buttonType="button" modalTitle="添加转办人" :multiple="false" />
          </a-form-item>
          <a-form-item class="!mb-0 !mt-10px" v-if="formConf.overTimeConfig.overTimeType === 11">
            <jnpf-select v-model:value="formConf.overTimeConfig.overTimeExtraRule" :options="overTimeRuleOptions" />
          </a-form-item>
          <a-form-item class="!mb-0" v-if="formConf.overTimeConfig.overTimeType === 9">
            <div class="ant-form-item-label">
              <label class="ant-form-item-no-colon">数据接口</label>
              <BasicHelp text='请求自带参数：taskId、taskNodeId，返回结构：JSON对象{"handleId":"id1,id2"}' />
            </div>
            <interface-modal
              :value="formConf.overTimeConfig.interfaceId"
              :title="formConf.overTimeConfig.interfaceName"
              @change="onOverTimeConfigInterfaceChange" />
            <template v-if="formConf.overTimeConfig.templateJson?.length">
              <div class="ant-form-item-label !mt-12px"><label class="ant-form-item-no-colon">参数设置</label></div>
              <select-interface-btn
                :templateJson="formConf.overTimeConfig.templateJson"
                :fieldOptions="funcOptions"
                isFlow
                showFieldFullLabel
                showSystemFullLabel
                @fieldChange="onRelationFieldChange" />
            </template>
          </a-form-item>
        </template>
      </template>
    </a-form>
  </template>
  <!-- 表单权限 -->
  <template v-else>
    <a-table :data-source="formConf.formOperates" :columns="formOperatesColumns" size="small" :pagination="false" :scroll="{ y: 'calc(100vh - 197px)' }">
      <template #headerCell="{ column }">
        <template v-if="column.key === 'write'">
          <a-checkbox v-model:checked="readAllChecked" :indeterminate="isReadIndeterminate" @change="handleCheckAllChange($event, 1)">查看</a-checkbox>
          <a-checkbox v-model:checked="writeAllChecked" :indeterminate="isWriteIndeterminate" @change="handleCheckAllChange($event, 2)">编辑</a-checkbox>
          <a-checkbox v-model:checked="requiredAllChecked" :indeterminate="isRequiredIndeterminate" @change="handleCheckAllChange($event, 3)">
            必填
          </a-checkbox>
        </template>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'write'">
          <a-checkbox v-model:checked="record.read" @change="handleCheckedChange">查看</a-checkbox>
          <a-checkbox v-model:checked="record.write" @change="handleCheckedChange">编辑</a-checkbox>
          <a-checkbox v-model:checked="record.required" :disabled="record.requiredDisabled" @change="handleCheckedChange">必填</a-checkbox>
        </template>
      </template>
    </a-table>
  </template>
  <AssignRuleModal @register="registerAssignRuleModal" v-bind="getAssignBindValue" />
  <ConditionModal @register="registerConditionModal" @confirm="updatePrintConfig" />
  <ApproversSortModal @register="registerApproversSortModal" @confirm="updateApproversSortList" />
  <StyleScript type="workflow" @register="registerStyleScriptModal" @confirm="updateStyleScript" />
</template>
<script lang="ts" setup>
  import { reactive, toRefs, inject, computed, unref, watch } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import { useModal } from '@/components/Modal';
  import { DownOutlined } from '@ant-design/icons-vue';
  import {
    typeOptions,
    defaultStep,
    formFieldTypeOptions,
    nodeOverTimeMsgOptions,
    overTimeOptions,
    printConditionTypeOptions,
    divideRuleOptions,
    systemFieldOptions,
  } from '../helper/define';
  import { SelectInterfaceBtn } from '@/components/Interface';
  import { InterfaceModal } from '@/components/CommonModal';
  import { TreeSelect } from 'ant-design-vue';
  import { NodeUtils } from '../bpmn/utils/nodeUtil';
  import { formatToDateTime } from '@/utils/dateUtil';
  import HeaderContainer from './components/HeaderContainer.vue';
  import FlowFormModal from './components/FlowFormModal.vue';
  import NoticeConfig from './components/NoticeConfig.vue';
  import AssignRuleModal from './components/AssignRuleModal.vue';
  import ConditionModal from './components/ConditionModal.vue';
  import ApproversSortModal from './components/ApproversSortModal.vue';
  import { typeConfig } from '../bpmn/config';
  import { bpmnProcessing, bpmnTask } from '../bpmn/config/variableName';
  import { buildBitUUID } from '@/utils/uuid';
  import draggable from 'vuedraggable';
  import StyleScript from './components/StyleScript.vue';
  import { ReloadOutlined } from '@ant-design/icons-vue';

  interface State {
    activeKey: number;
    assignList: any[];
    userLabel: string;
    readAllChecked: boolean;
    writeAllChecked: boolean;
    requiredAllChecked: boolean;
    isReadIndeterminate: boolean;
    isWriteIndeterminate: boolean;
    isRequiredIndeterminate: boolean;
    conditionType: string;
    currentBtnIndex: number;
    jsJson: string;
    formLoading: boolean;
  }

  const state = reactive<State>({
    activeKey: 1,
    assignList: [],
    userLabel: '',
    readAllChecked: false,
    writeAllChecked: false,
    requiredAllChecked: false,
    isReadIndeterminate: false,
    isWriteIndeterminate: false,
    isRequiredIndeterminate: false,
    conditionType: '',
    currentBtnIndex: 0,
    jsJson: '({ flowInfo, formData, taskInfo, onlineUtils }) => {\n    // 在此编写代码\n    \n}',
    formLoading: false,
  });
  defineOptions({ inheritAttrs: false });
  defineExpose({ getContent, updateCheckStatus });
  const { activeKey, readAllChecked, writeAllChecked, requiredAllChecked, isReadIndeterminate, isWriteIndeterminate, isRequiredIndeterminate, formLoading } =
    toRefs(state);
  const emit = defineEmits(['updateDivideRule']);
  const props = defineProps([
    'formConf',
    'printTplOptions',
    'prevNodeList',
    'beforeNodeOptions',
    'funcOptions',
    'noticeOptions',
    'formFieldsOptions',
    'usedFormItems',
    'getFormFieldList',
    'initFormOperates',
    'updateJnpfData',
    'nodeOptions',
    'flowState',
    'type',
    'updateFormFieldList',
    'updateBpmnProperties',
  ]);
  const [registerAssignRuleModal, { openModal: openAssignRuleModal }] = useModal();
  const [registerConditionModal, { openModal: openConditionModal }] = useModal();
  const [registerApproversSortModal, { openModal: openApproversSortModal }] = useModal();
  const [registerStyleScriptModal, { openModal: openStyleScriptModal }] = useModal();
  const bpmn: (() => string | undefined) | undefined = inject('bpmn');
  const extraRuleOptions = [
    { id: 1, fullName: '无办理人范围' },
    { id: 6, fullName: '同一公司' },
    { id: 2, fullName: '同一部门' },
    { id: 3, fullName: '同一岗位' },
    { id: 4, fullName: '发起人上级' },
    { id: 5, fullName: '发起人下属' },
  ];
  const candidateExtraRuleOptions = [
    { id: 1, fullName: '无候选人范围' },
    { id: 6, fullName: '同一公司' },
    { id: 2, fullName: '同一部门' },
    { id: 3, fullName: '同一岗位' },
  ];
  const overTimeRuleOptions = [
    { id: 2, fullName: '同一部门' },
    { id: 7, fullName: '同一角色' },
    { id: 3, fullName: '同一岗位' },
    { id: 8, fullName: '同一分组' },
  ];
  const extraCopyRuleOptions = extraRuleOptions.map(o => (o.id === 1 ? { id: 1, fullName: '无抄送人范围' } : o));
  const counterSignOptions = [
    { id: 0, fullName: '或签（一名成员办理即可）' },
    { id: 1, fullName: '会签（无序会签，办理达到会签比例，办理通过）' },
  ];
  const auditTypeOptions = [
    { id: 1, fullName: '按百分比' },
    { id: 2, fullName: '按人数' },
  ];
  const calculateTypeOptions = [
    { id: 1, fullName: '实时计算' },
    { id: 2, fullName: '延后计算' },
  ];
  const rejectTypeOptions = [{ id: 0, fullName: '无' }, ...auditTypeOptions];
  const formOperatesColumns = [
    { title: '表单字段', dataIndex: 'name', key: 'name' },
    { title: '操作', dataIndex: 'write', key: 'write', align: 'center', width: 250 },
  ];
  const conditionTypeOptions = [
    { id: 1, fullName: '字段' },
    { id: 2, fullName: '自定义' },
    { id: 3, fullName: '系统参数' },
  ];
  const approverTypeOptions = [
    { id: 1, fullName: '发起人' },
    { id: 2, fullName: '上节点办理人' },
  ];
  const transferTypeOptions = [
    { fullName: '指定成员', id: 6 },
    { fullName: '超时办理人', id: 11 },
    { fullName: '数据接口', id: 9 },
  ];
  const getBpmn = computed(() => (bpmn as () => any)());
  const getJnpfGlobalData = computed(() => {
    const jnpfData: any = unref(getBpmn).get('jnpfData');
    return jnpfData?.getValue('global') || {};
  });
  const getBindValue = computed(() => ({
    funcOptions: props.noticeOptions,
    isApprover: true,
  }));
  const getAssignBindValue = computed(() => ({
    formConf: props.formConf,
    formFieldsOptions: props.formFieldsOptions,
  }));
  const backNodeCodeOptions = computed(() => {
    let options = [...defaultStep, ...props.beforeNodeOptions];
    if (props.formConf.backType == 2) options = options.filter(o => o.id != 1);
    return options;
  });
  const getPrintConditionsContent = computed(() =>
    NodeUtils.getConditionsContent(props.formConf.printConfig.conditions, props.formConf.printConfig.matchLogic),
  );
  const getSystemFieldOptions = computed(() => systemFieldOptions.map(o => ({ ...o, label: o.fullName ? o.id + '(' + o.fullName + ')' : o.id })));
  const getParameterList = computed(() => unref(getJnpfGlobalData).globalParameterList || []);
  const getApprovalFieldOptions = computed(() => unref(getJnpfGlobalData).approvalFieldList || []);
  const getCanSetApproversSort = computed(() => {
    return (
      props.formConf.assigneeType === 6 &&
      props.formConf.approvers?.length &&
      props.formConf.approvers.every(o => o.indexOf('--user') !== -1) &&
      props.formConf.counterSign == 2
    );
  });
  const getHasAssign = computed(() => props.formConf.assignList?.length && props.formConf.assignList.some(o => o.ruleList?.length));
  const getFormFieldOptions = computed(() => getFormField(props.usedFormItems, props.formConf.formFieldType));
  const getCopyFormFieldOptions = computed(() => getFormField(props.usedFormItems, props.formConf.copyFormFieldType));

  watch(
    () => props.formConf,
    () => props.updateJnpfData(props.formConf),
    { deep: true, immediate: true },
  );
  watch(
    () => state.activeKey,
    val => {
      val == 3 && updateCheckStatus();
    },
  );

  function onFormIdChange(id, item) {
    if (!id) return handleNull();
    const isSameForm = props.formConf.formId === id;
    props.formConf.formName = item.fullName;
    props.formConf.formId = id;
    props.formConf.assignList = [];
    props.getFormFieldList(id, 'approverForm', isSameForm);
  }
  function handleNull() {
    props.formConf.formName = '';
    props.formConf.formId = '';
    let formFieldList = [];
    const jnpfData: any = unref(getBpmn).get('jnpfData');
    const global = jnpfData.getValue('global');
    const formId = global.formId;
    if (formId && global.allFormMap && global.allFormMap['form_' + formId]) {
      formFieldList = global.allFormMap['form_' + formId] || [];
    }
    props.updateFormFieldList(formFieldList);
    props.formConf.formOperates = props.initFormOperates(props.formConf, true);
  }
  function onBackTypeChange(e) {
    if (e?.target?.value == 2 && props.formConf.backNodeCode == 1) props.formConf.backNodeCode = 0;
  }
  function openTransmitRuleBox() {
    const assignList = getRealAssignList(props.formConf.assignList ? cloneDeep(props.formConf.assignList) : []);
    openAssignRuleModal(true, { assignList });
  }
  function getRealAssignList(assignList) {
    const jnpfData: any = unref(getBpmn).get('jnpfData');
    const global = jnpfData.getValue('global');
    let newAssignList = props.prevNodeList.map(o => {
      let formFieldList: any[] = [];
      const formId = o.formId;
      if (formId && o.type != 'start' && global.allFormMap && global.allFormMap['form_' + formId]) {
        let list = cloneDeep(global.allFormMap['form_' + formId]) || [];
        list = list.filter(o => o.__config__.jnpfKey !== 'table');
        list = list.map(o => ({ ...o, id: o.id + '|' + formId }));
        const sysFieldList = [{ id: '@prevNodeFormId', fullName: '上节点表单Id' }];
        formFieldList.push({ fullName: '上节点表单', children: [...sysFieldList, ...list] });
      }
      const globalFormId = global.formId;
      if (globalFormId && global.allFormMap && global.allFormMap['form_' + globalFormId]) {
        let list = cloneDeep(global.allFormMap['form_' + globalFormId]) || [];
        list = list.filter(o => o.__config__.jnpfKey !== 'table');
        list = list.map(o => ({ ...o, id: o.id + '|' + globalFormId }));
        const sysFieldList = [{ id: '@startNodeFormId', fullName: '发起节点表单Id' }];
        formFieldList.push({ fullName: '发起节点表单', children: [...sysFieldList, ...list] });
      }
      const globalParameterList = cloneDeep(global.globalParameterList).map(o => ({ ...o, fullName: o.fieldName, id: o.fieldName + '|globalParameter' }));
      globalParameterList.length && formFieldList.push({ fullName: '全局变量', children: globalParameterList || [] });
      return {
        nodeId: o.id,
        title: o.fullName,
        formFieldList,
        ruleList: [],
      };
    });
    if (!assignList.length) return newAssignList;
    let list: any[] = [];
    // 去掉被删掉的节点
    for (let i = 0; i < assignList.length; i++) {
      const e = assignList[i];
      inter: for (let j = 0; j < newAssignList.length; j++) {
        if (e.nodeId === newAssignList[j].nodeId) {
          const item = {
            nodeId: e.nodeId,
            title: newAssignList[j].title,
            formFieldList: newAssignList[j].formFieldList,
            ruleList: e.ruleList,
          };
          list.push(item);
          break inter;
        }
      }
    }
    const addList = newAssignList.filter(o => !assignList.some(item => item.nodeId === o.nodeId));
    return [...list, ...addList];
  }
  function onAssigneeTypeChange() {
    props.formConf.approvers = [];
    props.formConf.approversSortList = [];
    props.formConf.extraRule = 1;
    props.updateBpmnProperties('elementBodyName', getContent());
  }
  function onFormFieldTypeChange() {
    props.formConf.formField = '';
  }
  function onCopyFormFieldTypeChange() {
    props.formConf.copyFormField = '';
  }
  function getFormField(list, type) {
    if (type === 2) return list.filter(o => o.__config__.jnpfKey == 'depSelect');
    if (type === 3) return list.filter(o => o.__config__.jnpfKey == 'posSelect');
    if (type === 4) return list.filter(o => o.__config__.jnpfKey == 'roleSelect');
    if (type === 5) return list.filter(o => o.__config__.jnpfKey == 'groupSelect');
    return list.filter(o => o.__config__.jnpfKey == 'userSelect' || o.__config__.jnpfKey == 'usersSelect');
  }
  function onApproversChange(val) {
    if (props.formConf.assigneeType != 6 || !val || !val.length || !val.every(o => o.indexOf('--user') !== -1)) return (props.formConf.approversSortList = []);
    if (!props.formConf.approversSortList?.length) return (props.formConf.approversSortList = val);
    const arr = props.formConf.approversSortList.filter(o => val.indexOf(o) !== -1);
    const updated = val.filter(o => props.formConf.approversSortList.indexOf(o) === -1);
    props.formConf.approversSortList = [...arr, ...updated];
  }
  function onLabelChange(val) {
    state.userLabel = val;
    props.updateBpmnProperties('elementBodyName', getContent());
  }
  function getContent() {
    let content = typeConfig[bpmnProcessing].renderer.bodyDefaultText;
    if (props.formConf.assigneeType != 6) {
      content = typeOptions.find(o => o.id === props.formConf.assigneeType)?.fullName || typeConfig[bpmnProcessing].renderer.bodyDefaultText;
    } else {
      content = state.userLabel || typeConfig[bpmnProcessing].renderer.bodyDefaultText;
    }
    props.formConf.content = content;
    return content;
  }
  function onInterfaceChange(val, row) {
    if (!val) {
      props.formConf.interfaceConfig.interfaceId = '';
      props.formConf.interfaceConfig.interfaceName = '';
      props.formConf.interfaceConfig.templateJson = [];
      return;
    }
    if (props.formConf.interfaceConfig.interfaceId === val) return;
    props.formConf.interfaceConfig.interfaceId = val;
    props.formConf.interfaceConfig.interfaceName = row.fullName;
    props.formConf.interfaceConfig.templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
  }
  function onOverTimeConfigInterfaceChange(val, row) {
    if (!val) {
      props.formConf.overTimeConfig.interfaceId = '';
      props.formConf.overTimeConfig.interfaceName = '';
      props.formConf.overTimeConfig.templateJson = [];
      return;
    }
    if (props.formConf.overTimeConfig.interfaceId === val) return;
    props.formConf.overTimeConfig.interfaceId = val;
    props.formConf.overTimeConfig.interfaceName = row.fullName;
    props.formConf.overTimeConfig.templateJson = row.templateJson.map(o => ({ ...o, sourceType: 1 })) || [];
  }
  function onRelationFieldChange(val, row) {
    if (!val) return;
    let list = props.funcOptions.filter(o => o.id === val);
    if (!list.length) return;
    let item = list[0];
    row.isSubTable = item.__config__ && item.__config__.isSubTable ? item.__config__.isSubTable : false;
  }
  function onSignNumberChange(val, key) {
    if (val) return;
    props.formConf.counterSignConfig[key] = 1;
  }
  function handleCheckAllChange(e, type) {
    const val = e.target.checked;
    if (type == 1) state.isReadIndeterminate = false;
    if (type == 2) state.isWriteIndeterminate = false;
    if (type == 3) state.isRequiredIndeterminate = false;
    props.formConf.formOperates.forEach(item => {
      if (type == 1) item.read = val;
      if (type == 2) item.write = val;
      if (type == 3 && !item.requiredDisabled) item.required = val;
    });
  }
  function handleCheckedChange() {
    updateCheckStatus();
  }
  function updateCheckStatus() {
    const formOperatesLen = props.formConf.formOperates?.length || 0;
    const requiredDisabledCount = props.formConf.formOperates?.filter(o => !o.requiredDisabled).length || 0;
    let readCount = 0;
    let writeCount = 0;
    let requiredCount = 0;
    props.formConf.formOperates?.forEach(item => {
      if (item.read) readCount++;
      if (item.write) writeCount++;
      if (item.required && !item.requiredDisabled) requiredCount++;
    });
    state.readAllChecked = !!formOperatesLen ? formOperatesLen === readCount : false;
    state.writeAllChecked = !!formOperatesLen ? formOperatesLen === writeCount : false;
    state.requiredAllChecked = !!formOperatesLen ? requiredDisabledCount === requiredCount : false;
    state.isReadIndeterminate = !!readCount && readCount < formOperatesLen;
    state.isWriteIndeterminate = !!writeCount && writeCount < formOperatesLen;
    state.isRequiredIndeterminate = !!requiredCount && requiredCount < requiredDisabledCount;
  }
  function handlePrintConfig() {
    state.conditionType = 'print';
    openConditionModal(true, { usedFormItems: props.usedFormItems, formFieldsOptions: props.formFieldsOptions, ...props.formConf.printConfig });
  }
  function updatePrintConfig(data) {
    const form = state.conditionType == 'print' ? 'printConfig' : state.conditionType == 'audit' ? 'autoAuditRule' : 'autoRejectRule';
    props.formConf[form] = { ...props.formConf[form], ...data };
  }
  function handlePrintConfigRemove() {
    props.formConf.printConfig.matchLogic = 'and';
    props.formConf.printConfig.conditions = [];
  }
  function handleAddParameter() {
    props.formConf.parameterList.push({
      field: '',
      fieldValueType: 2,
      fieldValue: '',
    });
  }
  function onFieldValueChange(item, val, data) {
    item.fieldLabel = val ? data.fullName : '';
    item.fieldValueJnpfKey = val ? data.__config__.jnpfKey : '';
  }
  function onConditionDateChange(val, item) {
    if (!val) return (item.fieldLabel = '');
    const format = item.format || 'YYYY-MM-DD HH:mm:ss';
    item.fieldLabel = formatToDateTime(val, format);
  }
  function onConditionListChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    const labelList = data.map(o => o.fullName);
    item.fieldLabel = labelList.join('/');
  }
  function onConditionOrganizeChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    item.fieldLabel = data.organize || '';
  }
  function onConditionObjChange(item, val, data) {
    if (!val) return (item.fieldLabel = '');
    item.fieldLabel = data.fullName || '';
  }
  function handleDelItem(index) {
    props.formConf.parameterList.splice(index, 1);
  }
  function onFieldValueTypeChange(item) {
    item.fieldValue = '';
    item.fieldLabel = '';
  }
  function openApproversSortListModal() {
    openApproversSortModal(true, { ids: props.formConf.approversSortList });
  }
  function updateApproversSortList(data) {
    props.formConf.approversSortList = data;
  }
  function onTimeLimitChange(val) {
    if (val == 0) props.formConf.overTimeConfig.on = 0;
  }
  function onOverAutoTransferChange(value) {
    if (value) props.formConf.overTimeConfig.overAutoApprove = false;
  }
  function onOverAutoApproveChange(value) {
    if (value) props.formConf.overTimeConfig.overAutoTransfer = false;
  }
  function addCustomBtn() {
    const data = {
      value: 'btn_' + buildBitUUID(),
      label: '按钮' + ((props.formConf.customBtns?.length || 0) + 1),
      jsJson: state.jsJson,
    };
    props.formConf.customBtns?.push(data);
  }
  function setDefaultValue(e) {
    if (e.element.label === '') {
      const data = {
        ...e.element,
        label: '按钮' + (e.index + 1),
      };
      props.formConf.customBtns[e.index] = data;
    }
  }
  function editStyle(element, index) {
    state.currentBtnIndex = index;
    openStyleScriptModal(true, { text: element.jsJson || state.jsJson });
  }
  function updateStyleScript(data) {
    props.formConf.customBtns[state.currentBtnIndex].jsJson = data;
  }
  function handleSelectChange(value, fieldPath, min) {
    let keys = fieldPath.split('.');
    let obj = props.formConf;
    while (keys.length > 1) {
      const key = keys.shift();
      obj = obj[key];
    }
    const lastKey = keys[0];
    obj[lastKey] = value || min;
  }
  function handleDivideRule() {
    emit('updateDivideRule', props.formConf.divideRule);
  }
  function handleRefreshFormField() {
    if (state.formLoading == true) return;
    state.formLoading = true;
    props
      .getFormFieldList(props.formConf.formId, 'approverForm', true)
      .then(() => {
        state.formLoading = false;
      })
      .catch(() => {
        state.formLoading = false;
      });
  }
</script>
