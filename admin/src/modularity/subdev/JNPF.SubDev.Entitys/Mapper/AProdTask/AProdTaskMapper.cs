using JNPF.Common.Security;
using JNPF.example.Entitys.Dto.AProdTask;
using JNPF.example.Entitys.Dto.AProdTaskItem;
using Mapster;
 
namespace JNPF.example.Entitys.Mapper.AProdTask;

public class Mapper : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
        config.ForType<AProdTaskCrInput, AProdTaskEntity>()
            .Map(dest => dest.F_ProcessingId, src => src.F_ProcessingId != null ? src.F_ProcessingId : null)
            .Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
            .Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
            .Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
            .Map(dest => dest.F_ProdDispatch, src => src.F_ProdDispatch != null && src.F_ProdDispatch.Count > 0 ? src.F_ProdDispatch.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_QcDispatch, src => src.F_QcDispatch != null && src.F_QcDispatch.Count > 0 ? src.F_QcDispatch.ToJsonString().Replace("\r\n", "").Replace(" ", "") : null)
            .Map(dest => dest.F_ProdQty, src => src.F_ProdQty != null ? src.F_ProdQty : null)
            .Map(dest => dest.F_TaskStatus, src => src.F_TaskStatus != null ? src.F_TaskStatus : null)
            .Map(dest => dest.F_TaskType, src => src.F_TaskType != null ? src.F_TaskType : null)
            .Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
        ;
        config.ForType<AProdTaskEntity, AProdTaskInfoOutput>()
            .Map(dest => dest.F_ProcessingId, src => src.F_ProcessingId != null ? src.F_ProcessingId : null)
            .Map(dest => dest.F_ProcessId, src => src.F_ProcessId != null ? src.F_ProcessId : null)
            .Map(dest => dest.F_PlanStartDate, src => src.F_PlanStartDate != null ? src.F_PlanStartDate : null)
            .Map(dest => dest.F_PlanEndDate, src => src.F_PlanEndDate != null ? src.F_PlanEndDate : null)
            .Map(dest => dest.F_ProdDispatch, src => src.F_ProdDispatch != null ? src.F_ProdDispatch.ToObject<List<string>>() : null)
            .Map(dest => dest.F_QcDispatch, src => src.F_QcDispatch != null ? src.F_QcDispatch.ToObject<List<string>>() : null)
            .Map(dest => dest.F_ProdQty, src => src.F_ProdQty != null ? src.F_ProdQty : null)
            .Map(dest => dest.F_TaskStatus, src => src.F_TaskStatus != null ? src.F_TaskStatus : null)
            .Map(dest => dest.F_TaskType, src => src.F_TaskType != null ? src.F_TaskType : null)
            .Map(dest => dest.F_SortCode, src => src.F_SortCode != null ? src.F_SortCode : null)
            .Map(dest => dest.F_Description, src => src.F_Description != null ? src.F_Description : null)
            .Map(dest => dest.F_CreatorTime, src => string.Format("{0:yyyy-MM-dd HH:mm:ss}", src.F_CreatorTime))
            .Map(dest => dest.tableField0bb944, src => src.AProdTaskItemList.Adapt<List<AProdTaskItemInfoOutput>>())
        ;
        config.ForType<AProdTaskEntity, AProdTaskDetailOutput>()
            .Map(dest => dest.tableField0bb944, src => src.AProdTaskItemList.Adapt<List<AProdTaskItemDetailOutput>>())
        ;
    }
}
