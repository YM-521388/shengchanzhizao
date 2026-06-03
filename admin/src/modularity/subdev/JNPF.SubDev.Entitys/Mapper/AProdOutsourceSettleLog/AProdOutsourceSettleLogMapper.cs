using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdOutsourceSettleLog;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdOutsourceSettleLog;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdOutsourceSettleLogCrInput, AProdOutsourceSettleLogEntity>()
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		//config.ForType<AProdOutsourceSettleLogEntity, AProdOutsourceSettleLogListOutput>()
		;
		config.ForType<AProdOutsourceSettleLogEntity, AProdOutsourceSettleLogInfoOutput>()
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdOutsourceSettleLogEntity, AProdOutsourceSettleLogDetailOutput>()
			.Map(dest => dest.F_Title, src => src.F_Title != null ? src.F_Title : null)
			.Map(dest => dest.F_Content, src => src.F_Content != null ? src.F_Content : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
