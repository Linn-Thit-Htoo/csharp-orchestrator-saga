using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;

namespace csharp_orchestrator_saga.OrderOrchestrator.Services.Stock
{
    public interface IStockService
    {
        Task ProcessStock(CreateOrderCommand command, CancellationToken cs = default);
    }
}
