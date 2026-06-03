using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.ACtcFollow;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.ACtcFollow;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<ACtcFollowCrInput, ACtcFollowEntity>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_FollowType, src => src.F_FollowType != null ? src.F_FollowType : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_FollowContent, src => src.F_FollowContent != null ? src.F_FollowContent : null)
			.Map(dest => dest.F_NextFollowTime, src => src.F_NextFollowTime != null ? src.F_NextFollowTime : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<ACtcFollowEntity, ACtcFollowInfoOutput>()
			.Map(dest => dest.F_CustomerId, src => src.F_CustomerId != null ? src.F_CustomerId : null)
			.Map(dest => dest.F_FollowType, src => src.F_FollowType != null ? src.F_FollowType : null)
			.Map(dest => dest.F_ContactId, src => src.F_ContactId != null ? src.F_ContactId : null)
			.Map(dest => dest.F_FollowContent, src => src.F_FollowContent != null ? src.F_FollowContent : null)
			.Map(dest => dest.F_NextFollowTime, src => src.F_NextFollowTime != null ? src.F_NextFollowTime : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
