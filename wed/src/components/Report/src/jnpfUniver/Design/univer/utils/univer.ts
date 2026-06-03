import { IWorkbookData, LocaleType, merge, Univer, UniverInstanceType } from '@univerjs/core';
import { defaultTheme } from '@univerjs/design';
import DesignZhCN from '@univerjs/design/locale/zh-CN';
import UIZhCN from '@univerjs/ui/locale/zh-CN';
import DocsUIZhCN from '@univerjs/docs-ui/locale/zh-CN';
import SheetsZhCN from '@univerjs/sheets/locale/zh-CN';
import SheetsUIZhCN from '@univerjs/sheets-ui/locale/zh-CN';
import SheetsNumfmtUIZhCN from '@univerjs/sheets-numfmt-ui/locale/zh-CN';
import SheetsFormulaUIZhCN from '@univerjs/sheets-formula-ui/locale/zh-CN';
import UniscriptZhCN from '@univerjs/uniscript/locale/zh-CN';

import SheetsFilterUIZhCN from '@univerjs/sheets-filter-ui/locale/zh-CN';
import SheetsSortUIZhCN from '@univerjs/sheets-sort-ui/locale/zh-CN';
import SheetsDataValidationZhCN from '@univerjs/sheets-data-validation-ui/locale/zh-CN';
import SheetsConditionalFormattingUIZhCN from '@univerjs/sheets-conditional-formatting-ui/locale/zh-CN';
import SheetsHyperLinkUIZhCN from '@univerjs/sheets-hyper-link-ui/locale/zh-CN';
import DrawingUIZhCN from '@univerjs/drawing-ui/locale/zh-CN';
import SheetsDrawingUIZhCN from '@univerjs/sheets-drawing-ui/locale/zh-CN';
import watermarkZhCN from '@univerjs/watermark/locale/zh-CN';
import ThreadCommentUIZhCN from '@univerjs/thread-comment-ui/locale/zh-CN';
import SheetsThreadCommentUIZhCN from '@univerjs/sheets-thread-comment-ui/locale/zh-CN';
import FindReplaceZhCN from '@univerjs/find-replace/locale/zh-CN';
import SheetsFindReplaceZhCN from '@univerjs/sheets-find-replace/locale/zh-CN';
import SheetsCrosshairHighlightZhCN from '@univerjs/sheets-crosshair-highlight/locale/zh-CN';
import SheetsZenEditorZhCN from '@univerjs/sheets-zen-editor/locale/zh-CN';

import { UniverRenderEnginePlugin } from '@univerjs/engine-render';
import { UniverFormulaEnginePlugin } from '@univerjs/engine-formula';
import { UniverUIPlugin } from '@univerjs/ui';
import { UniverDocsPlugin } from '@univerjs/docs';
import { UniverDocsUIPlugin } from '@univerjs/docs-ui';
import { UniverSheetsPlugin } from '@univerjs/sheets';
import { UniverSheetsUIPlugin } from '@univerjs/sheets-ui';
import { UniverSheetsFormulaPlugin } from '@univerjs/sheets-formula';
import { UniverSheetsFormulaUIPlugin } from '@univerjs/sheets-formula-ui';
import { UniverSheetsNumfmtPlugin } from '@univerjs/sheets-numfmt';
import { UniverSheetsNumfmtUIPlugin } from '@univerjs/sheets-numfmt-ui';

// 筛选
import { UniverSheetsFilterPlugin } from '@univerjs/sheets-filter';
import { UniverSheetsFilterUIPlugin } from '@univerjs/sheets-filter-ui';
// 排序
import { UniverSheetsSortPlugin } from '@univerjs/sheets-sort';
import { UniverSheetsSortUIPlugin } from '@univerjs/sheets-sort-ui';
// 数据验证
import { UniverDataValidationPlugin } from '@univerjs/data-validation';
import { UniverSheetsDataValidationPlugin } from '@univerjs/sheets-data-validation';
import { UniverSheetsDataValidationUIPlugin } from '@univerjs/sheets-data-validation-ui';
// 条件格式
import { UniverSheetsConditionalFormattingPlugin } from '@univerjs/sheets-conditional-formatting';
import { UniverSheetsConditionalFormattingUIPlugin } from '@univerjs/sheets-conditional-formatting-ui';
// 超链接
import { UniverSheetsHyperLinkPlugin } from '@univerjs/sheets-hyper-link';
import { UniverSheetsHyperLinkUIPlugin } from '@univerjs/sheets-hyper-link-ui';
// 图片
import { UniverDocsDrawingPlugin } from '@univerjs/docs-drawing';
import { UniverDrawingPlugin } from '@univerjs/drawing';
import { UniverDrawingUIPlugin } from '@univerjs/drawing-ui';
import { UniverSheetsDrawingPlugin } from '@univerjs/sheets-drawing';
import { UniverSheetsDrawingUIPlugin } from '@univerjs/sheets-drawing-ui';
// 水印
import { UniverWatermarkPlugin } from '@univerjs/watermark';
// 评论/批注
import { UniverThreadCommentPlugin } from '@univerjs/thread-comment';
import { UniverThreadCommentUIPlugin } from '@univerjs/thread-comment-ui';
import { UniverSheetsThreadCommentPlugin } from '@univerjs/sheets-thread-comment';
import { UniverSheetsThreadCommentUIPlugin } from '@univerjs/sheets-thread-comment-ui';
// 查找 & 替换
import { UniverFindReplacePlugin } from '@univerjs/find-replace';
import { UniverSheetsFindReplacePlugin } from '@univerjs/sheets-find-replace';
// 十字高亮
import { UniverSheetsCrosshairHighlightPlugin } from '@univerjs/sheets-crosshair-highlight';
// 禅编译器
import { UniverSheetsZenEditorPlugin } from '@univerjs/sheets-zen-editor';
// uniscript
import { UniverUniscriptPlugin } from '@univerjs/uniscript';

// 自定义
import { JnpfSheetsCellPlugin } from '../plugins/sheet-cell.plugin';
import { JnpfSheetsRangePlugin } from '../plugins/sheet-range.plugin';
import { JnpfSheetsDialogPlugin } from '../plugins/sheet-dialog.plugin';
import { JnpfSheetsExcelFilePlugin } from '../plugins/sheet-excel-file.plugin';
import { JnpfSheetsFloatEchartPlugin } from '../plugins/sheet-float-echart.plugin';
import { JnpfSheetsCellEchartPlugin } from '../plugins/sheet-cell-echart.plugin';
import { JnpfSheetsFloatImagePlugin } from '../plugins/sheet-float-image.plugin';
import { JnpfSheetsFloatDomPlugin } from '../plugins/sheet-float-dom.plugin';
import { JnpfSheetsPreviewPlugin } from '../plugins/sheet-preview.plugin';
import { JnpfSheetsPrintPlugin } from '../plugins/sheet-print.plugin';

import { JnpfUniverFloatEchartKey, JnpfUniverFloatImageKey } from './define';
import { correctSheetCellData, isEmptyObject, moveArrayValueToEnd } from './index';

// 创建univer实例
export function createUniverInstance(config: any) {
  const { container, header, footer, contextMenu, workbookData } = config ?? {};

  const univerInstance = new Univer({
    theme: defaultTheme,
    locale: LocaleType.ZH_CN,
    locales: {
      [LocaleType.ZH_CN]: merge(
        {},
        DesignZhCN,
        UIZhCN,
        DocsUIZhCN,
        SheetsZhCN,
        SheetsUIZhCN,
        SheetsNumfmtUIZhCN,
        SheetsFormulaUIZhCN,
        SheetsFilterUIZhCN,
        SheetsSortUIZhCN,
        SheetsDataValidationZhCN,
        SheetsConditionalFormattingUIZhCN,
        SheetsHyperLinkUIZhCN,
        DrawingUIZhCN,
        SheetsDrawingUIZhCN,
        watermarkZhCN,
        ThreadCommentUIZhCN,
        SheetsThreadCommentUIZhCN,
        FindReplaceZhCN,
        SheetsFindReplaceZhCN,
        SheetsCrosshairHighlightZhCN,
        SheetsZenEditorZhCN,
        UniscriptZhCN,
      ),
    },
  });

  univerInstance.registerPlugin(UniverRenderEnginePlugin);
  univerInstance.registerPlugin(UniverFormulaEnginePlugin);
  univerInstance.registerPlugin(UniverUIPlugin, {
    container,
    header,
    footer,
    contextMenu,
  });
  univerInstance.registerPlugin(UniverDocsPlugin);
  univerInstance.registerPlugin(UniverDocsUIPlugin);
  univerInstance.registerPlugin(UniverSheetsPlugin);
  univerInstance.registerPlugin(UniverSheetsUIPlugin, {
    menu: {
      ['sheet.command.insert-cell-image']: {
        hidden: true,
      },
    },
  });
  univerInstance.registerPlugin(UniverSheetsFormulaPlugin);
  univerInstance.registerPlugin(UniverSheetsFormulaUIPlugin);
  univerInstance.registerPlugin(UniverSheetsNumfmtPlugin);
  univerInstance.registerPlugin(UniverSheetsNumfmtUIPlugin);

  // 这个和注销的冲突掉了
  univerInstance.registerPlugin(UniverSheetsHyperLinkPlugin);
  univerInstance.registerPlugin(UniverSheetsHyperLinkUIPlugin);

  univerInstance.registerPlugin(UniverDocsDrawingPlugin);
  univerInstance.registerPlugin(UniverDrawingPlugin);
  univerInstance.registerPlugin(UniverDrawingUIPlugin);
  univerInstance.registerPlugin(UniverSheetsDrawingPlugin);
  univerInstance.registerPlugin(UniverSheetsDrawingUIPlugin);

  univerInstance.registerPlugin(UniverFindReplacePlugin);
  univerInstance.registerPlugin(UniverSheetsFindReplacePlugin);
  univerInstance.registerPlugin(UniverSheetsFilterPlugin);
  univerInstance.registerPlugin(UniverSheetsFilterUIPlugin);
  univerInstance.registerPlugin(UniverSheetsSortPlugin);
  univerInstance.registerPlugin(UniverSheetsSortUIPlugin);
  univerInstance.registerPlugin(UniverDataValidationPlugin);
  univerInstance.registerPlugin(UniverSheetsDataValidationPlugin);
  univerInstance.registerPlugin(UniverSheetsDataValidationUIPlugin, {
    showEditOnDropdown: true,
  });
  univerInstance.registerPlugin(UniverSheetsConditionalFormattingPlugin);
  univerInstance.registerPlugin(UniverSheetsConditionalFormattingUIPlugin);
  univerInstance.registerPlugin(UniverSheetsCrosshairHighlightPlugin);
  univerInstance.registerPlugin(UniverThreadCommentPlugin);
  univerInstance.registerPlugin(UniverThreadCommentUIPlugin);
  univerInstance.registerPlugin(UniverSheetsThreadCommentPlugin);
  univerInstance.registerPlugin(UniverSheetsThreadCommentUIPlugin);
  univerInstance.registerPlugin(UniverUniscriptPlugin);
  univerInstance.registerPlugin(UniverSheetsZenEditorPlugin);
  univerInstance.registerPlugin(UniverWatermarkPlugin);

  univerInstance.registerPlugin(JnpfSheetsCellPlugin);
  univerInstance.registerPlugin(JnpfSheetsRangePlugin);
  univerInstance.registerPlugin(JnpfSheetsDialogPlugin);
  univerInstance.registerPlugin(JnpfSheetsExcelFilePlugin);
  univerInstance.registerPlugin(JnpfSheetsFloatDomPlugin);
  univerInstance.registerPlugin(JnpfSheetsFloatEchartPlugin);
  univerInstance.registerPlugin(JnpfSheetsCellEchartPlugin);
  univerInstance.registerPlugin(JnpfSheetsFloatImagePlugin);
  univerInstance.registerPlugin(JnpfSheetsPreviewPlugin);
  univerInstance.registerPlugin(JnpfSheetsPrintPlugin);

  univerInstance.createUnit(UniverInstanceType.UNIVER_SHEET, workbookData);
  return univerInstance;
}

// 解析创建univer实例的快照数据
export function analysisCreateUnitData(workbookData: string | IWorkbookData, createMode: string, isReadonly: boolean) {
  try {
    const parseWorkbookData = typeof workbookData === 'string' ? JSON.parse(workbookData) : workbookData;

    if (createMode === 'design' && !isReadonly) {
      // 设计模式并且非只读的情况下直接返回快照
      return parseWorkbookData;
    }

    const { resources = [] } = parseWorkbookData ?? {};

    // 主要是预览的情况下不让图表可操作
    const updatedResources = resources?.map((resource: any) => {
      if (resource?.name === 'SHEET_DRAWING_PLUGIN') {
        const parseResourceData = JSON.parse(resource?.data ?? '{}');

        const updatedParseResourceData = Object.fromEntries(
          Object.entries(parseResourceData)?.map(([sheetKey, sheetResourceOption]: any) => {
            const sheetResourceOptionData = sheetResourceOption?.data || {};

            const updateSheetResourceOption = Object.fromEntries(
              Object.entries(sheetResourceOptionData).map(([floatDomKey, floatDomValue]: any) => {
                const { componentKey, ...floatDomValueRest } = floatDomValue || {};

                if (componentKey === JnpfUniverFloatEchartKey || componentKey === JnpfUniverFloatImageKey) {
                  return [
                    floatDomKey,
                    {
                      ...floatDomValueRest,
                      componentKey,
                      allowTransform: false,
                    },
                  ];
                } else {
                  return [floatDomKey, floatDomValue];
                }
              }),
            );

            return [sheetKey, { ...sheetResourceOption, data: updateSheetResourceOption }];
          }),
        );

        return {
          ...resource,
          data: JSON.stringify(updatedParseResourceData),
        };
      } else {
        return resource;
      }
    });

    return {
      ...parseWorkbookData,
      resources: updatedResources,
    };
  } catch (e) {
    console.error(e);
    return {};
  }
}

// 整理设计的数据
export function arrangeDesignWorkbookData(snapshot: any, floatEchartDataCaches: any, floatImageDataCaches: any) {
  const updateFloatEcharts: Record<string, any> = {};
  const updateCellEcharts: Record<string, any> = {};
  const updateFloatImages: Record<string, any> = {};

  // 给悬浮图表和悬浮图片增加float dom的id 并且剔除已被删除的悬浮图表和悬浮图片数据
  const { resources = [], sheets = {} } = snapshot ?? {};

  const updatedResources = resources?.map((resource: any) => {
    if (resource?.name === 'SHEET_DRAWING_PLUGIN') {
      const parseResourceData = JSON.parse(resource?.data ?? '{}');

      const updatedParseResourceData = Object.fromEntries(
        Object.entries(parseResourceData)?.map(([sheetKey, sheetResourceOption]: any) => {
          const sheetResourceOptionData = sheetResourceOption?.data || {};

          const updateSheetResourceOption = Object.fromEntries(
            Object.entries(sheetResourceOptionData).map(([floatDomKey, floatDomValue]: any) => {
              const { componentKey, drawingId, props = {}, ...floatDomValueRest } = floatDomValue || {};

              if (componentKey === JnpfUniverFloatEchartKey) {
                if (floatEchartDataCaches?.hasOwnProperty(drawingId)) {
                  updateFloatEcharts[drawingId] = { ...floatEchartDataCaches[drawingId] }; // store中的悬浮图表数据过滤
                }

                const domId = floatEchartDataCaches?.[drawingId]?.domId;
                const updatedProps = {
                  ...props,
                  id: domId,
                };

                return [
                  floatDomKey,
                  {
                    ...floatDomValueRest,
                    drawingId,
                    componentKey,
                    props: updatedProps,
                  },
                ];
              } else if (componentKey === JnpfUniverFloatImageKey) {
                if (floatImageDataCaches?.hasOwnProperty(drawingId)) {
                  updateFloatImages[drawingId] = { ...floatImageDataCaches[drawingId] }; // store中的悬浮图片数据过滤
                }

                const domId = floatImageDataCaches?.[drawingId]?.domId;
                const updatedProps = {
                  ...props,
                  id: domId,
                };

                return [
                  floatDomKey,
                  {
                    ...floatDomValueRest,
                    drawingId,
                    componentKey,
                    props: updatedProps,
                  },
                ];
              } else {
                return [floatDomKey, floatDomValue];
              }
            }),
          );

          return [sheetKey, { ...sheetResourceOption, data: updateSheetResourceOption }];
        }),
      );

      return {
        ...resource,
        data: JSON.stringify(updatedParseResourceData),
      };
    } else {
      return resource;
    }
  });

  // 取出所有自定义的custom
  const customs: any[] = [];
  for (const sheetKey in sheets) {
    const sheet = sheets[sheetKey];
    const sheetCellData = sheet?.cellData;

    if (!sheetCellData) continue;
    for (const rowKey in sheetCellData) {
      const row = sheetCellData[rowKey];

      for (const colKey in row) {
        const cellData = row[colKey] ?? {};
        const custom = cellData?.custom;

        if (custom && !isEmptyObject(custom)) {
          if (custom.type !== 'cellEchart') {
            customs.push({
              sheetId: sheetKey,
              row: rowKey,
              col: colKey,
              cellData,
            });
          } else {
            if (!updateCellEcharts[sheetKey]) {
              updateCellEcharts[sheetKey] = {};
            }

            if (!updateCellEcharts[sheetKey][rowKey]) {
              updateCellEcharts[sheetKey][rowKey] = {};
            }

            updateCellEcharts[sheetKey][rowKey] = {
              ...updateCellEcharts[sheetKey][rowKey],
              [colKey]: custom.config ?? {},
            };
          }
        }
      }
    }
  }

  return {
    snapshot: {
      ...snapshot,
      resources: updatedResources,
    },
    floatEcharts: updateFloatEcharts,
    cellEcharts: updateCellEcharts,
    floatImages: updateFloatImages,
    customs,
  };
}

// 整理预览的数据
export function arrangePreviewWorkbookData(designWorkbookData: any, floatEchartToUniverImgDataCaches: any, floatImageToUniverImgDataCaches: any) {
  const { snapshot, floatEcharts, floatImages, customs } = designWorkbookData ?? {};

  const { resources = [], sheets } = snapshot ?? {};
  const updateSheets = { ...sheets };

  Object.entries(sheets)?.forEach(([sheetKey, sheetInfo]: any) => {
    const { cellData, rowCount, columnCount } = sheetInfo ?? {};

    // 找到第一个匹配的资源
    const resource = resources.find((resourceInfo: any) => resourceInfo?.name === 'SHEET_DRAWING_PLUGIN');
    const parseResourceData = JSON.parse(resource?.data ?? '{}');

    const hasFloatDom = !isEmptyObject(parseResourceData) && parseResourceData[sheetKey]?.order?.length;

    const updateCellData = hasFloatDom ? correctSheetCellData(cellData, rowCount, columnCount, true) : correctSheetCellData(cellData, rowCount, columnCount);

    updateSheets[sheetKey] = {
      ...sheets[sheetKey],
      cellData: updateCellData,
    };
  });

  const updatedResources = resources?.map((resource: any) => {
    if (resource?.name === 'SHEET_DRAWING_PLUGIN') {
      const parseResourceData = JSON.parse(resource?.data ?? '{}');

      const updatedParseResourceData = Object.fromEntries(
        Object.entries(parseResourceData)?.map(([sheetKey, sheetResourceOption]: any) => {
          const { data: sheetResourceOptionData = {}, order: originalOrder = [] } = sheetResourceOption || {};

          let sheetOrder = [...originalOrder]; // 使用一个可变的引用来追踪顺序更新

          const updatedSheetData = Object.fromEntries(
            Object.entries(sheetResourceOptionData).map(([floatDomKey, floatDomValue]: any) => {
              const { unitId, subUnitId, drawingId, componentKey, sheetTransform, transform, props } = floatDomValue || {};

              if (componentKey === JnpfUniverFloatEchartKey) {
                const { id: domId } = props ?? {};

                sheetOrder = moveArrayValueToEnd(sheetOrder, drawingId);

                const source = floatEchartToUniverImgDataCaches?.[domId] ?? '';

                return [
                  floatDomKey,
                  {
                    drawingId,
                    drawingType: 0,
                    imageSourceType: 'BASE64',
                    sheetTransform,
                    source,
                    subUnitId,
                    transform,
                    unitId,
                  },
                ];
              } else if (componentKey === JnpfUniverFloatImageKey) {
                const { id: domId } = props ?? {};

                sheetOrder = moveArrayValueToEnd(sheetOrder, drawingId);

                const source = floatImageToUniverImgDataCaches?.[domId] ?? '';

                return [
                  floatDomKey,
                  {
                    drawingId,
                    drawingType: 0,
                    imageSourceType: source?.startsWith('data:image/') ? 'BASE64' : 'URL',
                    sheetTransform,
                    source,
                    subUnitId,
                    transform,
                    unitId,
                  },
                ];
              } else {
                return [floatDomKey, floatDomValue];
              }
            }),
          );

          return [sheetKey, { ...sheetResourceOption, data: updatedSheetData, order: sheetOrder }];
        }),
      );

      return {
        ...resource,
        data: JSON.stringify(updatedParseResourceData),
      };
    } else {
      return resource;
    }
  });

  return {
    snapshot: {
      ...snapshot,
      sheets: updateSheets,
      resources: updatedResources,
    },
    floatEcharts,
    floatImages,
    customs,
  };
}

// 判断当前工作本是不是含有自定义的 float dom
export function judgeSheetHasCustomFloatDom(data: any): boolean {
  return Object.values(data).some(
    (floatDomValue: any) => floatDomValue?.componentKey === JnpfUniverFloatEchartKey || floatDomValue?.componentKey === JnpfUniverFloatImageKey,
  );
}

// 监听插入悬浮图表
export function onInsertedFloatEchart(jnpfUniverStore: any, command: any) {
  const { drawingId } = command?.params ?? {};
  if (!drawingId) {
    return;
  }

  const floatEchartDataCaches = jnpfUniverStore?.floatEchartDataCaches ?? {};

  const updateItems = {
    ...floatEchartDataCaches,
    [drawingId]: command?.params,
  };

  jnpfUniverStore?.setFloatEchartDataCaches(updateItems);
}

// 监听插入悬浮图片
export function onInsertedFloatImage(jnpfUniverStore: any, command: any) {
  const { drawingId } = command?.params ?? {};
  if (!drawingId) {
    return;
  }

  const floatImageDataCaches = jnpfUniverStore?.floatImageDataCaches ?? {};

  const updateItems = {
    ...floatImageDataCaches,
    [drawingId]: command?.params,
  };

  jnpfUniverStore?.setFloatImageDataCaches(updateItems);
}
