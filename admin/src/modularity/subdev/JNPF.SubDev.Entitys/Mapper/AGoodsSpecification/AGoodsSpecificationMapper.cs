using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AGoodsSpecification;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AGoodsSpecification;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AGoodsSpecificationCrInput, AGoodsSpecificationEntity>()
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null && src.F_ParentId.Count > 0 ? src.F_ParentId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AGoodsSpecificationEntity, AGoodsSpecificationInfoOutput>()
			.Map(dest => dest.F_Name, src => src.F_Name != null ? src.F_Name : null)
			.Map(dest => dest.F_ParentId, src => src.F_ParentId != null ? src.F_ParentId.ToObject<List<string>>() : null)
			.Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
