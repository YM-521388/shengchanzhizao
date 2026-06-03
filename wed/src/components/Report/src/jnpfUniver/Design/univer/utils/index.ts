import { JnpfPrintDirectionEnum, JnpfPrintPaperSizeForType } from './define';

/**
 * 检查值是否是 null 或 undefined
 * @param value 要检查的值
 * @returns 如果值是 null 或 undefined，返回 true，否则返回 false
 */
export const isNullOrUndefined = (value: any) => {
  return value === null || value === undefined;
};

/**
 * 判断一个值是否为空对象
 * @param obj - 待检查的值
 * @returns 如果是空对象，返回 true；否则返回 false
 */
export const isEmptyObject = (obj: unknown): boolean => {
  return obj !== null && typeof obj === 'object' && !Array.isArray(obj) && Object.keys(obj).length === 0;
};

/**
 * 将指定值从数组中移除并将其添加到数组的末尾。
 * @param array - 要操作的数组。
 * @param  value - 要移动到数组末尾的值。
 * @returns - 操作后的数组（原数组被修改）。
 */
export const moveArrayValueToEnd = (array: any[], value: string | number) => {
  const index = array?.indexOf(value); // 找到目标值的索引

  if (index !== -1) {
    // 如果值存在于数组中
    const [item] = array.splice(index, 1); // 删除目标值
    array.push(item); // 添加到数组末尾
  }

  return array;
};

/**
 * 根据字母规则计算输入字符串的索引值。
 * @param input {string} - 输入的字母字符串（如 "A", "B", "AA", "AB" 等）。
 * @returns {number} - 对应的索引值。
 */
export const getIndexFromAlphabetRule = (input: string): number => {
  const base = 26; // 字母表的长度
  const charCodeA = 'A'.charCodeAt(0);

  let index = 0;
  for (let i = 0; i < input.length; i++) {
    index = index * base + (input.charCodeAt(i) - charCodeA + 1);
  }

  return index - 1; // 数组索引从 0 开始
};

/**
 * 根据索引值获取字母字符串。
 * @param index {number} - 输入的索引值（如 0, 1, 26, 27 等）。
 * @returns {string} - 对应的字母字符串（如 "A", "B", "AA", "AB" 等）。
 */
export const getAlphabetFromIndexRule = (index: number): string => {
  const base = 26; // 字母表的长度
  const charCodeA = 'A'.charCodeAt(0);

  let result = '';
  index += 1; // 转为从 1 开始的规则

  while (index > 0) {
    const remainder = (index - 1) % base;
    result = String.fromCharCode(charCodeA + remainder) + result;
    index = Math.floor((index - 1) / base);
  }

  return result;
};

/**
 * 根据类型和坐标获取箭头单元格数组
 * @param parentCellType - 父单元格类型
 * @param colName - 列名
 * @param rowName - 行名
 * @returns 单元格坐标数组
 */
export const getSheetRelationCell = (parentCellType: string, colName?: string, rowName?: string): { row: number; col: number }[] => {
  if (parentCellType === 'none' || !colName || !rowName) return [];

  const col = getIndexFromAlphabetRule(colName);
  const row = Number(rowName) - 1;

  return [{ row, col }];
};

/**
 * 修正工作表中的单元格数据。
 * 根据传入的单元格数据 (`cellData`) 和指定的行、列数量，判断是否需要修正数据。
 * 如果所有单元格数据异常（如未定义、空字符串、或无效值），则修正为默认配置；
 * 如果存在正常单元格，则返回原始数据。
 *
 */
export const correctSheetCellData = (cellData: any = {}, rowCount: number, columnCount: number, isFloatDom: boolean = false) => {
  // 单元格纠正配置
  const correctCellDataConfig = {
    v: ' ',
    t: 1,
  };

  // 如果 cellData 是空对象，直接返回默认初始化的单元格数据
  if (isEmptyObject(cellData)) {
    return {
      0: {
        0: correctCellDataConfig,
      },
    };
  }

  let totalCells = 0;
  let abnormalCells = 0;
  // 统计单元格数据中正常与异常的数量
  for (const rowKey in cellData) {
    const rowValue = cellData[rowKey] ?? {};
    for (const colKey in rowValue) {
      const { v, t, s } = rowValue[colKey] ?? {};

      // 判断异常单元格条件
      if (isNullOrUndefined(s)) {
        if (v === undefined || (v === '' && t === 1) || (v === 0 && (t === 2 || t === 3))) {
          abnormalCells++;
        }
      }

      totalCells++;
    }
  }

  // 如果存在正常单元格，直接返回原始数据
  if (totalCells > abnormalCells) {
    return cellData;
  }

  // 否则需要修正数据
  for (let i = 0; i < rowCount; i++) {
    for (let j = 0; j < columnCount; j++) {
      const { v, t, custom } = cellData?.[i]?.[j] ?? {};

      // 判断是否需要修正（根据 isFloatDom 区分）
      const needsCorrection = isFloatDom
        ? custom === undefined && (v === undefined || (v === '' && t === 1))
        : custom === undefined && (v === undefined || (v === '' && t === 1) || (v === 0 && (t === 2 || t === 3)));

      // 如果发现第一个需要修正的单元格，修正并结束所有循环
      if (needsCorrection) {
        if (!cellData[i]) {
          cellData[i] = {};
        }
        cellData[i][j] = correctCellDataConfig;

        return cellData; // 修正完成后直接返回
      }
    }
  }

  // 排查不出来问题，只能返回了
  return cellData;
};

/**
 * 将 Base64 编码的字符串转换为 File 对象
 * @param base64String Base64 字符串（必须以 `data:` 开头）
 * @param fileName 生成的文件名
 * @param mimeType 文件 MIME 类型（如 "image/png", "application/pdf"）
 * @returns 返回一个 Promise，解析后得到 File 对象
 */
export function base64ToFile(base64String: string, fileName: string, mimeType: string): Promise<File> {
  return fetch(base64String)
    .then(res => res.blob()) // 将 Base64 转换为 Blob
    .then(blob => new File([blob], fileName, { type: mimeType })); // 生成 File 对象
}

/**
 * 计算旋转后的边界框尺寸
 * @param {number} width 原始宽度
 * @param {number} height 原始高度
 * @param {number} angleDegrees 旋转角度（0-360，单位：度）
 * @returns {{ rotatedWidth: number; rotatedHeight: number }} 旋转后的宽度和高度
 */
export function rotatedBoundingBox(width: number, height: number, angleDegrees: number): { rotatedWidth: number; rotatedHeight: number } {
  const angle = (angleDegrees * Math.PI) / 180; // 角度转换为弧度
  const rotatedWidth = Math.abs(width * Math.cos(angle)) + Math.abs(height * Math.sin(angle));
  const rotatedHeight = Math.abs(width * Math.sin(angle)) + Math.abs(height * Math.cos(angle));
  return { rotatedWidth, rotatedHeight };
}

/**
 * 获取打印页面的样式
 * @param paperType - 纸张类型，对应 `JnpfPrintPaperSizeForType` 中的键值
 * @param direction - 打印方向，取值为 `JnpfPrintDirectionEnum.portrait`（纵向）或 `JnpfPrintDirectionEnum.landscape`（横向）
 * @returns 一个包含打印样式的 `<style>` 元素
 */
export function getPrintPageStyle(paperType: string, direction: string) {
  const { w, h } = JnpfPrintPaperSizeForType[paperType as keyof typeof JnpfPrintPaperSizeForType] ?? {};
  const width = direction === JnpfPrintDirectionEnum?.portrait ? w : h;
  const height = direction === JnpfPrintDirectionEnum?.portrait ? h : w;
  const style = `
@page {
    size: ${width}px ${height}px; 
}
@page {
    margin: 0;
    visibility: hidden;
}
@media print {
    body > * {
        display: none!important;
    }
    #jnpfReportPrint, #jnpfReportPrint * {
        display: block!important;
        height: fit-content;
        overflow: visible;
        top: 0;
        width: fit-content;
    }
    #jnpfReportPrint .printContainer {
        page-break-after: always!important;
        height: ${height}px;
        width: ${width}px;
        position: relative;
    }
}`;
  const $style = document.createElement('style');
  $style.innerHTML = style;
  $style.className = 'jnpfPrintCss';
  return $style;
}
