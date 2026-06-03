using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AGoodsInventoryQr;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AGoodsInventoryQr;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AGoodsInventoryQrCrInput, AGoodsInventoryQrEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
			.Map(dest => dest.F_CreatorOrganizeId, src => src.F_CreatorOrganizeId != null ? src.F_CreatorOrganizeId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AGoodsInventoryQrEntity, AGoodsInventoryQrInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_Code, src => src.F_Code != null ? src.F_Code : null)
			.Map(dest => dest.F_CreatorOrganizeId, src => src.F_CreatorOrganizeId != null ? src.F_CreatorOrganizeId : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
