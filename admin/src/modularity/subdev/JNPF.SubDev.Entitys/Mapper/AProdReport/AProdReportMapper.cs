using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdReport;
using JNPF.example.Entitys.Dto.AProdReportDefect;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdReport;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdReportCrInput, AProdReportEntity>()
            .Map(dest => dest.F_ProdUserId, src => src.F_ProdUserId != null ? src.F_ProdUserId : null)
            .Map(dest => dest.F_GoodQty, src => src.F_GoodQty != null ? src.F_GoodQty : null)
			.Map(dest => dest.F_GoodNotQty, src => src.F_GoodNotQty != null ? src.F_GoodNotQty : null)
			.Map(dest => dest.F_StartTime, src => src.F_StartTime != null ? src.F_StartTime : null)
			.Map(dest => dest.F_EndTime, src => src.F_EndTime != null ? src.F_EndTime : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleUserId, src => src.F_SettleUserId != null ? src.F_SettleUserId : null)
			.Map(dest => dest.F_SettleTime, src => src.F_SettleTime != null ? src.F_SettleTime : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
		;
		config.ForType<AProdReportEntity, AProdReportInfoOutput>()
            .Map(dest => dest.F_ProdUserId, src => src.F_ProdUserId != null ? src.F_ProdUserId : null)
            .Map(dest => dest.F_GoodQty, src => src.F_GoodQty != null ? src.F_GoodQty : null)
			.Map(dest => dest.F_GoodNotQty, src => src.F_GoodNotQty != null ? src.F_GoodNotQty : null)
			.Map(dest => dest.F_StartTime, src => src.F_StartTime != null ? src.F_StartTime : null)
			.Map(dest => dest.F_EndTime, src => src.F_EndTime != null ? src.F_EndTime : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleUserId, src => src.F_SettleUserId != null ? src.F_SettleUserId : null)
			.Map(dest => dest.F_SettleTime, src => src.F_SettleTime != null ? src.F_SettleTime : null)
			.Map(dest => dest.F_WagePrice, src => src.F_WagePrice != null ? src.F_WagePrice : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField579169, src => src.AProdReportDefectList.Adapt<List<AProdReportDefectInfoOutput>>())
		;
		config.ForType<AProdReportEntity, AProdReportDetailOutput>()
            .Map(dest => dest.tableField579169, src => src.AProdReportDefectList.Adapt<List<AProdReportDefectDetailOutput>>())
		;
	}
}
