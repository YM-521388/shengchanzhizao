using JNPF.Common.Security;
using JNPF.Common.Models;
using JNPF.example.Entitys.Dto.APuOrderInvoice;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.APuOrderInvoice;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuOrderInvoiceCrInput, APuOrderInvoiceEntity>()
			.Map(dest => dest.F_OrderId, src => src.F_OrderId != null ? src.F_OrderId : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_InvoiceDate, src => src.F_InvoiceDate != null ? src.F_InvoiceDate : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null && src.F_Files.Count > 0 ? src.F_Files.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuOrderInvoiceEntity, APuOrderInvoiceInfoOutput>()
			.Map(dest => dest.F_OrderId, src => src.F_OrderId != null ? src.F_OrderId : null)
			.Map(dest => dest.F_SupplierId, src => src.F_SupplierId != null ? src.F_SupplierId : null)
			.Map(dest => dest.F_Money, src => src.F_Money != null ? src.F_Money : null)
			.Map(dest => dest.F_InvoiceDate, src => src.F_InvoiceDate != null ? src.F_InvoiceDate : null)
			.Map(dest => dest.F_Files, src => src.F_Files != null ? src.F_Files.ToObject<List<FileControlsModel>>() : new List<FileControlsModel>())
			.Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
		;
	}
}
