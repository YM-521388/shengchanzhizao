using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.APuCheck;
using JNPF.example.Entitys.Dto.APuCheckItem;
using JNPF.example.Entitys.Dto.APuCheckLog;
using Mapster;

namespace JNPF.example.Entitys.Mapper.APuCheck;

public class Mapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<APuCheckCrInput, APuCheckEntity>()
            .Map(dest => dest.F_CheckDate, src => src.F_CheckDate != null ? src.F_CheckDate : null)
            .Map(dest => dest.F_CheckUserId, src => src.F_CheckUserId != null ? src.F_CheckUserId : null)
          .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null && src.F_WarehouseId.Count > 0 ? src.F_WarehouseId.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
            .Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
        ;
        config.ForType<APuCheckEntity, APuCheckInfoOutput>()
            .Map(dest => dest.F_CheckDate, src => src.F_CheckDate != null ? src.F_CheckDate : null)
            .Map(dest => dest.F_CheckUserId, src => src.F_CheckUserId != null ? src.F_CheckUserId : null)
           .Map(dest => dest.F_WarehouseId, src => src.F_WarehouseId != null ? src.F_WarehouseId.ToObject<List<string>>() : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
            .Map(dest => dest.F_CreatorUserId, src => src.F_CreatorUserId != null ? src.F_CreatorUserId : null)
            .Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField91ea29, src => src.APuCheckItemList.Adapt<List<APuCheckItemInfoOutput>>())
            .Map(dest => dest.tableField960db3, src => src.APuCheckLogList.Adapt<List<APuCheckLogInfoOutput>>())
        ;
        config.ForType<APuCheckEntity, APuCheckDetailOutput>()
            .Map(dest => dest.tableField91ea29, src => src.APuCheckItemList.Adapt<List<APuCheckItemDetailOutput>>())
            .Map(dest => dest.tableField960db3, src => src.APuCheckLogList.Adapt<List<APuCheckLogDetailOutput>>())
        ;
    }
}
