using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdRoutingStep;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdRoutingStep;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdRoutingStepCrInput, AProdRoutingStepEntity>()
			.Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_PriceType, src => src.F_PriceType != null ? src.F_PriceType : null)
			.Map(dest => dest.F_UnitRatioProd, src => src.F_UnitRatioProd != null ? src.F_UnitRatioProd : null)
			.Map(dest => dest.F_UnitRatioReport, src => src.F_UnitRatioReport != null ? src.F_UnitRatioReport : null)
			.Map(dest => dest.F_UnitRatioBase, src => src.F_UnitRatioBase != null ? src.F_UnitRatioBase : null)
			.Map(dest => dest.F_IsOutsource, src => src.F_IsOutsource)
			.Map(dest => dest.F_IsQc, src => src.F_IsQc)
			.Map(dest => dest.F_DefectHandle, src => src.F_DefectHandle)
			.Map(dest => dest.F_TaskTimer, src => src.F_TaskTimer)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<AProdRoutingStepEntity, AProdRoutingStepInfoOutput>()
			.Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_PriceType, src => src.F_PriceType != null ? src.F_PriceType : null)
			.Map(dest => dest.F_UnitRatioProd, src => src.F_UnitRatioProd != null ? src.F_UnitRatioProd : null)
			.Map(dest => dest.F_UnitRatioReport, src => src.F_UnitRatioReport != null ? src.F_UnitRatioReport : null)
			.Map(dest => dest.F_UnitRatioBase, src => src.F_UnitRatioBase != null ? src.F_UnitRatioBase : null)
			.Map(dest => dest.F_IsOutsource, src => src.F_IsOutsource != null ? src.F_IsOutsource : null)
			.Map(dest => dest.F_IsQc, src => src.F_IsQc != null ? src.F_IsQc : null)
			.Map(dest => dest.F_DefectHandle, src => src.F_DefectHandle != null ? src.F_DefectHandle : null)
			.Map(dest => dest.F_TaskTimer, src => src.F_TaskTimer != null ? src.F_TaskTimer : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdRoutingStepEntity, AProdRoutingStepDetailOutput>()
			.Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_StdHour, src => src.F_StdHour != null ? src.F_StdHour : null)
			.Map(dest => dest.F_StdMinute, src => src.F_StdMinute != null ? src.F_StdMinute : null)
			.Map(dest => dest.F_StdSecond, src => src.F_StdSecond != null ? src.F_StdSecond : null)
			.Map(dest => dest.F_PriceType, src => src.F_PriceType != null ? src.F_PriceType : null)
			.Map(dest => dest.F_UnitRatioProd, src => src.F_UnitRatioProd != null ? src.F_UnitRatioProd : null)
			.Map(dest => dest.F_UnitRatioReport, src => src.F_UnitRatioReport != null ? src.F_UnitRatioReport : null)
			.Map(dest => dest.F_UnitRatioBase, src => src.F_UnitRatioBase != null ? src.F_UnitRatioBase : null)
			.Map(dest => dest.F_IsOutsource, src => src.F_IsOutsource != null ? src.F_IsOutsource : null)
			.Map(dest => dest.F_IsQc, src => src.F_IsQc != null ? src.F_IsQc : null)
			.Map(dest => dest.F_DefectHandle, src => src.F_DefectHandle != null ? src.F_DefectHandle : null)
			.Map(dest => dest.F_TaskTimer, src => src.F_TaskTimer != null ? src.F_TaskTimer : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
