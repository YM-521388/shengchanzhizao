import {
  BooleanNumber,
  BuildTextUtils,
  createDocumentModelWithStyle,
  Disposable,
  DrawingTypeEnum,
  IAccessor,
  ICommandService,
  IImageIoService,
  IImageIoServiceParam,
  Inject,
  Injector,
  IUniverInstanceService,
  Nullable,
  ObjectRelativeFromH,
  ObjectRelativeFromV,
  PositionedObjectLayoutType,
  UniverInstanceType,
  Workbook,
  WrapTextType,
} from '@univerjs/core';
import { ISheetLocationBase, SetRangeValuesCommand } from '@univerjs/sheets';
import { getImageSize } from '@univerjs/drawing';
import { IRenderManagerService } from '@univerjs/engine-render';
import { docDrawingPositionToTransform } from '@univerjs/docs-ui';
import { SheetSkeletonManagerService } from '@univerjs/sheets-ui';
import { rotatedBoundingBox } from '../utils';

export class JnpfSheetsCellEchartService extends Disposable {
  constructor(
    @Inject(Injector) private readonly _injector: Injector,
    @IUniverInstanceService private readonly _univerInstanceService: IUniverInstanceService,
    @ICommandService private readonly _commandService: ICommandService,
    @IImageIoService private readonly _imageIoService: IImageIoService,
  ) {
    super();
  }

  /**
   * 获取单元格内图形的大小
   * @param accessor 访问器对象
   * @param location 单元格位置信息
   * @param originImageWidth 原始图像宽度
   * @param originImageHeight 原始图像高度
   * @param angle 旋转角度（0-360，单位：度）
   * @returns 计算后的图像尺寸，或在获取失败时返回 `false`
   */
  private _getDrawingSizeByCell(accessor: IAccessor, location: ISheetLocationBase, originImageWidth: number, originImageHeight: number, angle: number) {
    const { rotatedHeight, rotatedWidth } = rotatedBoundingBox(originImageWidth, originImageHeight, angle);
    const renderManagerService = accessor.get(IRenderManagerService);
    const currentRender = renderManagerService.getRenderById(location.unitId);
    if (!currentRender) {
      return false;
    }
    const skeletonManagerService = currentRender.with(SheetSkeletonManagerService);
    const skeleton = skeletonManagerService.getWorksheetSkeleton(location.subUnitId)?.skeleton;
    if (skeleton == null) {
      return false;
    }
    const cellInfo = skeleton.getCellWithCoordByIndex(location.row, location.col);

    const cellWidth = cellInfo.mergeInfo.endX - cellInfo.mergeInfo.startX - 2;
    const cellHeight = cellInfo.mergeInfo.endY - cellInfo.mergeInfo.startY - 2;
    const imageRatio = rotatedWidth / rotatedHeight;
    const imageWidth = Math.ceil(Math.min(cellWidth, cellHeight * imageRatio));
    const scale = imageWidth / rotatedWidth;
    const realScale = !scale || Number.isNaN(scale) ? 0.001 : scale;

    return {
      width: originImageWidth * realScale,
      height: originImageHeight * realScale,
    };
  }

  /**
   * 在当前选中的单元格插入 EChart 图片
   * @param file 要插入的图片文件
   * @param row 行
   * @param col 列
   * @param echartConfig echart配置信息
   * @returns 插入结果，成功返回执行命令的结果，失败返回 `false` 或 `null`
   */
  public async insertCellEchart(file: File, row: number, col: number, echartConfig: any) {
    // 获取当前的工作簿实例
    const workbook = this._univerInstanceService.getCurrentUnitForType<Workbook>(UniverInstanceType.UNIVER_SHEET);
    if (!workbook) {
      return false;
    }
    const unitId = workbook.getUnitId();

    // 获取当前激活的工作表
    const worksheet = workbook.getActiveSheet();
    if (!worksheet) {
      return false;
    }
    const subUnitId = worksheet.getSheetId();

    // 将图片文件保存到 ImageIoService 并获取图片参数
    let imageParam: Nullable<IImageIoServiceParam>;
    try {
      imageParam = await this._imageIoService.saveImage(file);
    } catch (error) {
      return false;
    }
    if (imageParam == null) {
      return false;
    }

    // 获取图片相关信息
    const { imageId, imageSourceType, source, base64Cache } = imageParam;
    const { width, height, image } = await getImageSize(base64Cache || '');
    // 缓存图片数据
    this._imageIoService.addImageSourceCache(source, imageSourceType, image);

    const docDataModel = createDocumentModelWithStyle('', {});

    // 计算图片适应单元格后的尺寸
    const imageSize = this._getDrawingSizeByCell(
      this._injector,
      {
        unitId,
        subUnitId,
        row,
        col,
      },
      width,
      height,
      0,
    );
    if (!imageSize) {
      return false;
    }

    // 定义图片的文档变换参数
    const docTransform = {
      size: {
        width: imageSize.width,
        height: imageSize.height,
      },
      positionH: {
        relativeFrom: ObjectRelativeFromH.PAGE,
        posOffset: 0,
      },
      positionV: {
        relativeFrom: ObjectRelativeFromV.PARAGRAPH,
        posOffset: 0,
      },
      angle: 0,
    };

    // 定义文档中的绘图参数
    const docDrawingParam = {
      unitId: docDataModel.getUnitId(),
      subUnitId: docDataModel.getUnitId(),
      drawingId: imageId,
      drawingType: DrawingTypeEnum.DRAWING_IMAGE,
      imageSourceType,
      source,
      transform: docDrawingPositionToTransform(docTransform),
      docTransform,
      behindDoc: BooleanNumber.FALSE,
      title: '',
      description: '',
      layoutType: PositionedObjectLayoutType.INLINE, // Insert inline drawing by default.
      wrapText: WrapTextType.BOTH_SIDES,
      distB: 0,
      distL: 0,
      distR: 0,
      distT: 0,
    };

    // 生成插入绘图的 JSON 结构
    const jsonXActions = BuildTextUtils.drawing.add({
      documentDataModel: docDataModel,
      drawings: [docDrawingParam],
      selection: {
        collapsed: true,
        startOffset: 0,
        endOffset: 0,
      },
    });

    if (!jsonXActions) {
      return false;
    }

    docDataModel.apply(jsonXActions);

    return this._commandService.syncExecuteCommand(SetRangeValuesCommand.id, {
      value: {
        [row]: {
          [col]: {
            p: docDataModel.getSnapshot(),
            t: 1,
            custom: {
              type: 'cellEchart',
              config: {
                ...echartConfig,
                drawingId: imageId,
              },
            },
          },
        },
      },
    });
  }
}
