using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.AProdOutsource;
using JNPF.example.Entitys.Dto.AProdOutsourceItem;
using Mapster;

namespace JNPF.example.Entitys.Mapper.AProdOutsource;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<AProdOutsourceCrInput, AProdOutsourceEntity>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_OutsourceNo, src => src.F_OutsourceNo != null ? src.F_OutsourceNo : null)
			.Map(dest => dest.F_PlanNum, src => src.F_PlanNum != null ? src.F_PlanNum : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_ContactPerson, src => src.F_ContactPerson != null ? src.F_ContactPerson : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null && src.F_Region.Count > 0 ? src.F_Region.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_DetailAddress, src => src.F_DetailAddress != null ? src.F_DetailAddress : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
			.Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<AProdOutsourceEntity, AProdOutsourceInfoOutput>()
			.Map(dest => dest.F_GoodsId, src => src.F_GoodsId != null ? src.F_GoodsId : null)
			.Map(dest => dest.F_BOMId, src => src.F_BOMId != null ? src.F_BOMId : null)
			.Map(dest => dest.F_OutsourceNo, src => src.F_OutsourceNo != null ? src.F_OutsourceNo : null)
			.Map(dest => dest.F_PlanNum, src => src.F_PlanNum != null ? src.F_PlanNum : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_ContactPerson, src => src.F_ContactPerson != null ? src.F_ContactPerson : null)
			.Map(dest => dest.F_ContactPhone, src => src.F_ContactPhone != null ? src.F_ContactPhone : null)
			.Map(dest => dest.F_Region, src => src.F_Region != null ? src.F_Region.ToObject<List<string>>() : null)
			.Map(dest => dest.F_DetailAddress, src => src.F_DetailAddress != null ? src.F_DetailAddress : null)
			.Map(dest => dest.F_Price, src => src.F_Price != null ? src.F_Price : null)
			.Map(dest => dest.F_DeliveryDate, src => src.F_DeliveryDate != null ? src.F_DeliveryDate : null)
			.Map(dest => dest.F_Priority, src => src.F_Priority != null ? src.F_Priority : null)
			.Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
			.Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_State, src => src.F_State != null ? src.F_State : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField7a8044, src => src.AProdOutsourceItemList.Adapt<List<AProdOutsourceItemInfoOutput>>())
		;
		config.ForType<AProdOutsourceEntity, AProdOutsourceDetailOutput>()
            .Map(dest => dest.tableField7a8044, src => src.AProdOutsourceItemList.Adapt<List<AProdOutsourceItemDetailOutput>>())
		;
	}
}
