using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdProcess;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdProcess;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdProcessCrInput, AProdProcessEntity>()
			.Map(dest => dest.F_ProcessName, src => src.F_ProcessName != null ? src.F_ProcessName : null)
			.Map(dest => dest.F_ProcessCode, src => src.F_ProcessCode != null ? src.F_ProcessCode : null)
			.Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null && src.F_WorkshopId.Count > 0 ? src.F_WorkshopId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Line, src => src.F_Line != null ? src.F_Line : null)
			.Map(dest => dest.F_ReportUnit, src => src.F_ReportUnit != null ? src.F_ReportUnit : null)
			.Map(dest => dest.F_UnitRatio, src => src.F_UnitRatio != null ? src.F_UnitRatio : null)
			.Map(dest => dest.F_PriceType, src => src.F_PriceType != null ? src.F_PriceType : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_MachineId, src => src.F_MachineId != null && src.F_MachineId.Count > 0 ? src.F_MachineId.ToJsonString() : null)
            .Map(dest => dest.F_ProdUserIds, src => src.F_ProdUserIds != null && src.F_ProdUserIds.Count > 0 ? src.F_ProdUserIds.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_DefectIds, src => src.F_DefectIds != null && src.F_DefectIds.Count > 0 ? src.F_DefectIds.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_IsOutsource, src => src.F_IsOutsource)
			.Map(dest => dest.F_IsQc, src => src.F_IsQc)
			.Map(dest => dest.F_DefectHandle, src => src.F_DefectHandle)
			.Map(dest => dest.F_QcUserIds, src => src.F_QcUserIds != null && src.F_QcUserIds.Count > 0 ? src.F_QcUserIds.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_TaskTimer, src => src.F_TaskTimer)
			.Map(dest => dest.F_State, src => src.F_State)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdProcessEntity, AProdProcessInfoOutput>()
			.Map(dest => dest.F_ProcessName, src => src.F_ProcessName != null ? src.F_ProcessName : null)
			.Map(dest => dest.F_ProcessCode, src => src.F_ProcessCode != null ? src.F_ProcessCode : null)
			.Map(dest => dest.F_WorkshopId, src => src.F_WorkshopId != null ? src.F_WorkshopId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_Line, src => src.F_Line != null ? src.F_Line : null)
			.Map(dest => dest.F_ReportUnit, src => src.F_ReportUnit != null ? src.F_ReportUnit : null)
			.Map(dest => dest.F_UnitRatio, src => src.F_UnitRatio != null ? src.F_UnitRatio : null)
			.Map(dest => dest.F_PriceType, src => src.F_PriceType != null ? src.F_PriceType : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
            .Map(dest => dest.F_MachineId, src => src.F_MachineId != null ? src.F_MachineId.ToObject<List<string>>() : new List<string>())
            .Map(dest => dest.F_ProdUserIds, src => src.F_ProdUserIds != null ? src.F_ProdUserIds.ToObject<List<string>>() : null)
			.Map(dest => dest.F_DefectIds, src => src.F_DefectIds != null ? src.F_DefectIds.ToObject<List<string>>() : null)
			.Map(dest => dest.F_IsOutsource, src => src.F_IsOutsource != null ? src.F_IsOutsource : null)
			.Map(dest => dest.F_IsQc, src => src.F_IsQc != null ? src.F_IsQc : null)
			.Map(dest => dest.F_DefectHandle, src => src.F_DefectHandle != null ? src.F_DefectHandle : null)
			.Map(dest => dest.F_QcUserIds, src => src.F_QcUserIds != null ? src.F_QcUserIds.ToObject<List<string>>() : null)
			.Map(dest => dest.F_TaskTimer, src => src.F_TaskTimer != null ? src.F_TaskTimer : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
	}
}
