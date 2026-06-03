using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuStockPuLog;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuStockPuLog;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuStockPuLogCrInput, APuStockPuLogEntity>()
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_Reason, src => src.F_Reason != null ? src.F_Reason : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuStockPuLogEntity, APuStockPuLogListOutput>()
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_Reason, src => src.F_Reason != null ? src.F_Reason : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<APuStockPuLogEntity, APuStockPuLogInfoOutput>()
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_Reason, src => src.F_Reason != null ? src.F_Reason : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<APuStockPuLogEntity, APuStockPuLogDetailOutput>()
			.Map(dest => dest.F_Type, src => src.F_Type != null ? src.F_Type : null)
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_Reason, src => src.F_Reason != null ? src.F_Reason : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
