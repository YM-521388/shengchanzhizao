using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdOutsourceAccept;
using JNPF.example.Entitys.Dto.AProdOutsourceAcceptItem;
using JNPF.example.Entitys.Dto.AProdOutsourceSettleLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdOutsourceAccept;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdOutsourceAcceptCrInput, AProdOutsourceAcceptEntity>()
			.Map(dest => dest.F_OutsourceId, src => src.F_OutsourceId != null ? src.F_OutsourceId : null)
			.Map(dest => dest.F_PassNum, src => src.F_PassNum != null ? src.F_PassNum : null)
			.Map(dest => dest.F_FailNum, src => src.F_FailNum != null ? src.F_FailNum : null)
			.Map(dest => dest.F_OutsourceType, src => src.F_OutsourceType != null ? src.F_OutsourceType : null)
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleUnitPrice, src => src.F_SettleUnitPrice != null ? src.F_SettleUnitPrice : null)
			.Map(dest => dest.F_SettleUserId, src => src.F_SettleUserId != null ? src.F_SettleUserId : null)
			.Map(dest => dest.F_SettleTime, src => src.F_SettleTime != null ? src.F_SettleTime : null)
			.Map(dest => dest.F_SettleFiles, src => src.F_SettleFiles != null && src.F_SettleFiles.Count > 0 ? src.F_SettleFiles.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SettleDesc, src => src.F_SettleDesc != null ? src.F_SettleDesc : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdOutsourceAcceptEntity, AProdOutsourceAcceptInfoOutput>()
			.Map(dest => dest.F_OutsourceId, src => src.F_OutsourceId != null ? src.F_OutsourceId : null)
			.Map(dest => dest.F_PassNum, src => src.F_PassNum != null ? src.F_PassNum : null)
			.Map(dest => dest.F_FailNum, src => src.F_FailNum != null ? src.F_FailNum : null)
			.Map(dest => dest.F_OutsourceType, src => src.F_OutsourceType != null ? src.F_OutsourceType : null)
			.Map(dest => dest.F_SettleStatus, src => src.F_SettleStatus != null ? src.F_SettleStatus : null)
			.Map(dest => dest.F_SettleUnitPrice, src => src.F_SettleUnitPrice != null ? src.F_SettleUnitPrice : null)
			.Map(dest => dest.F_SettleUserId, src => src.F_SettleUserId != null ? src.F_SettleUserId : null)
			.Map(dest => dest.F_SettleTime, src => src.F_SettleTime != null ? src.F_SettleTime : null)
			.Map(dest => dest.F_SettleFiles, src => src.F_SettleFiles != null ? src.F_SettleFiles.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_SettleDesc, src => src.F_SettleDesc != null ? src.F_SettleDesc : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField561a64, src => src.AProdOutsourceAcceptItemList.Adapt<List<AProdOutsourceAcceptItemInfoOutput>>())
            .Map(dest => dest.tableField4b440e, src => src.AProdOutsourceSettleLogList.Adapt<List<AProdOutsourceSettleLogInfoOutput>>())
		;
		config.ForType<AProdOutsourceAcceptEntity, AProdOutsourceAcceptDetailOutput>()
            .Map(dest => dest.tableField561a64, src => src.AProdOutsourceAcceptItemList.Adapt<List<AProdOutsourceAcceptItemDetailOutput>>())
            .Map(dest => dest.tableField4b440e, src => src.AProdOutsourceSettleLogList.Adapt<List<AProdOutsourceSettleLogDetailOutput>>())
		;
	}
}
