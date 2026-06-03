using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuTransfer;
using JNPF.example.Entitys.Dto.APuTransferItem;
using JNPF.example.Entitys.Dto.APuTransferLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuTransfer;
 
public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<APuTransferCrInput, APuTransferEntity>()
			.Map(dest => dest.F_TransferDate, src => src.F_TransferDate != null ? src.F_TransferDate : null)
			.Map(dest => dest.F_TransferUserId, src => src.F_TransferUserId != null ? src.F_TransferUserId : null)
        .Map(dest => dest.F_FromWarehouseId, src => src.F_FromWarehouseId != null && src.F_FromWarehouseId.Count > 0 ? src.F_FromWarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_ToWarehouseId, src => src.F_ToWarehouseId != null && src.F_ToWarehouseId.Count > 0 ? src.F_ToWarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
		;
		config.ForType<APuTransferEntity, APuTransferInfoOutput>()
			.Map(dest => dest.F_TransferDate, src => src.F_TransferDate != null ? src.F_TransferDate : null)
			.Map(dest => dest.F_TransferUserId, src => src.F_TransferUserId != null ? src.F_TransferUserId : null)
            .Map(dest => dest.F_FromWarehouseId, src => src.F_FromWarehouseId != null ? src.F_FromWarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_ToWarehouseId, src => src.F_ToWarehouseId != null ? src.F_ToWarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
			.Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
			.Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableFieldaf2f04, src => src.APuTransferItemList.Adapt<List<APuTransferItemInfoOutput>>())
            .Map(dest => dest.tableFieldc39d26, src => src.APuTransferLogList.Adapt<List<APuTransferLogInfoOutput>>())
		;
		config.ForType<APuTransferEntity, APuTransferDetailOutput>()
            .Map(dest => dest.tableFieldaf2f04, src => src.APuTransferItemList.Adapt<List<APuTransferItemDetailOutput>>())
            .Map(dest => dest.tableFieldc39d26, src => src.APuTransferLogList.Adapt<List<APuTransferLogDetailOutput>>())
		;
	}
}
