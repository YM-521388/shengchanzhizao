using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdOutsourceAcceptItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdOutsourceAcceptItem;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdOutsourceAcceptItemCrInput, AProdOutsourceAcceptItemEntity>()
			.Map(dest => dest.F_AcceptId, src => src.F_AcceptId != null ? src.F_AcceptId : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		//config.ForType<AProdOutsourceAcceptItemEntity, AProdOutsourceAcceptItemListOutput>()
		;
		config.ForType<AProdOutsourceAcceptItemEntity, AProdOutsourceAcceptItemInfoOutput>()
			.Map(dest => dest.F_AcceptId, src => src.F_AcceptId != null ? src.F_AcceptId : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
		config.ForType<AProdOutsourceAcceptItemEntity, AProdOutsourceAcceptItemDetailOutput>()
			.Map(dest => dest.F_AcceptId, src => src.F_AcceptId != null ? src.F_AcceptId : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => src.F_CreatorTime != null ? src.F_CreatorTime : null)
		;
	}
}
