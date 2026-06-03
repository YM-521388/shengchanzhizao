using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AMaintDevice;
using JNPF.example.Entitys.Dto.AMaintDeviceItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AMaintDevice;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AMaintDeviceCrInput, AMaintDeviceEntity>()
			.Map(dest => dest.F_DeviceNo, src => src.F_DeviceNo != null ? src.F_DeviceNo : null)
			.Map(dest => dest.F_DeviceName, src => src.F_DeviceName != null ? src.F_DeviceName : null)
			.Map(dest => dest.F_DeviceSpec, src => src.F_DeviceSpec != null ? src.F_DeviceSpec : null)
			.Map(dest => dest.F_DeviceStatus, src => src.F_DeviceStatus != null ? src.F_DeviceStatus : null)
			.Map(dest => dest.F_DeviceType, src => src.F_DeviceType != null && src.F_DeviceType.Count > 0 ? src.F_DeviceType.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_GroupId, src => src.F_GroupId != null ? src.F_GroupId : null)
			.Map(dest => dest.F_DeviceUsersId, src => src.F_DeviceUsersId != null && src.F_DeviceUsersId.Count > 0 ? src.F_DeviceUsersId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null && src.F_WorkshopId.Count > 0 ? src.F_WorkshopId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_LineId, src => src.F_LineId != null ? src.F_LineId : null)
			.Map(dest => dest.F_DeviceQrCode, src => src.F_DeviceQrCode != null ? src.F_DeviceQrCode : null)
			.Map(dest => dest.F_DeviceImages, src => src.F_DeviceImages != null && src.F_DeviceImages.Count > 0 ? src.F_DeviceImages.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_EnabledMark, src => src.F_EnabledMark)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AMaintDeviceEntity, AMaintDeviceInfoOutput>()
			.Map(dest => dest.F_DeviceNo, src => src.F_DeviceNo != null ? src.F_DeviceNo : null)
			.Map(dest => dest.F_DeviceName, src => src.F_DeviceName != null ? src.F_DeviceName : null)
			.Map(dest => dest.F_DeviceSpec, src => src.F_DeviceSpec != null ? src.F_DeviceSpec : null)
			.Map(dest => dest.F_DeviceStatus, src => src.F_DeviceStatus != null ? src.F_DeviceStatus : null)
			.Map(dest => dest.F_DeviceType, src => src.F_DeviceType != null ? src.F_DeviceType.ToObject<List<string>>() : null)
			.Map(dest => dest.F_GroupId, src => src.F_GroupId != null ? src.F_GroupId : null)
			.Map(dest => dest.F_DeviceUsersId, src => src.F_DeviceUsersId != null ? src.F_DeviceUsersId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null ? src.F_WorkshopId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_LineId, src => src.F_LineId != null ? src.F_LineId : null)
			.Map(dest => dest.F_DeviceQrCode, src => src.F_DeviceQrCode != null ? src.F_DeviceQrCode : null)
			.Map(dest => dest.F_DeviceImages, src => src.F_DeviceImages != null ? src.F_DeviceImages.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_EnabledMark, src => src.F_EnabledMark != null ? src.F_EnabledMark : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField72c841, src => src.AMaintDeviceItemList.Adapt<List<AMaintDeviceItemInfoOutput>>())
		;
		config.ForType<AMaintDeviceEntity, AMaintDeviceDetailOutput>()
            .Map(dest => dest.tableField72c841, src => src.AMaintDeviceItemList.Adapt<List<AMaintDeviceItemDetailOutput>>())
		;
	}
}
