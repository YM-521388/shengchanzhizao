export const defaultCellProperties = {
  text: {
    leftParentCellType: 'none',
    leftParentCellCustomRowName: '',
    leftParentCellCustomColName: '',
    topParentCellType: 'none',
    topParentCellCustomRowName: '',
    topParentCellCustomColName: '',
  },
  parameter: {
    field: '',
    leftParentCellType: 'none',
    leftParentCellCustomRowName: '',
    leftParentCellCustomColName: '',
    topParentCellType: 'none',
    topParentCellCustomRowName: '',
    topParentCellCustomColName: '',
  },
  dataSource: {
    field: '',
    polymerizationType: '1',
    summaryType: 'sum',
    fillDirection: 'portrait',
    leftParentCellType: 'default',
    leftParentCellCustomRowName: '',
    leftParentCellCustomColName: '',
    topParentCellType: 'default',
    topParentCellCustomRowName: '',
    topParentCellCustomColName: '',
  },
};

//拼接url
export function getFloatImageUrl(data, url) {
  for (let key in data) {
    const type = data[key].source;
    let src = data[key].option.src;
    if (src && type == 1) data[key].option.src = url + src;
  }
  return data;
}

//拼接快照url
export function getFloatUrl(list, reportApiUrl, floatImages = {}) {
  let item: any = {};
  if (!list.length) return;
  for (let index = 0; index < list.length; index++) {
    const element = list[index];
    if (element.name != 'SHEET_DRAWING_PLUGIN') continue;
    element.data = element.data ? JSON.parse(element.data) : {};
    for (let key in element.data) {
      for (let imageKey in element.data[key].data) {
        let componentKey = element.data[key].data[imageKey].componentKey;
        if (componentKey == 'JnpfUniverFloatEchart') continue;
        let source = element.data[key].data[imageKey].source;
        const drawingId = element.data[key].data[imageKey].drawingId;
        if (source) element.data[key].data[imageKey].source = reportApiUrl + source;
        if (floatImages && floatImages[drawingId]) floatImages[drawingId].option.src = reportApiUrl + source;
      }
    }
    element.data = JSON.stringify(element.data);
  }
  item.list = list;
  item.floatImages = floatImages;
  return item;
}

//截取url前缀
export function interceptUrl(data, reportApiUrl) {
  for (let key in data) {
    const type = data[key].source;
    let src = data[key].option.src;
    if (src && type == 1) {
      src = src.split(reportApiUrl)[1];
      data[key].option.src = src;
    }
  }
  return data;
}

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
