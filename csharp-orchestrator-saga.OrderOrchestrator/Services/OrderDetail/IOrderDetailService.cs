using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;

namespace csharp_orchestrator_saga.OrderOrchestrator.Services.OrderDetail
{
    public interface IOrderDetailService
    {
        Task ProcessOrderDetail(CreateOrderCommand command, Guid orderId, Guid invoice, CancellationToken cs = default);
    }
}
