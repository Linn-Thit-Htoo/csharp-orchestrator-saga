using csharp_orchestrator_saga.OrderOrchestrator.Constants;
using csharp_orchestrator_saga.OrderOrchestrator.Entities;
using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;

namespace csharp_orchestrator_saga.OrderOrchestrator.Extensions
{
    public static class Mapper
    {
        public static TblOrder ToEntity(this CreateOrderCommand command)
        {
            return new TblOrder
            {
                UserId = command.UserId,
                InvoiceNo = Guid.NewGuid(),
                GrandTotal = command.GrandTotal,
                Status = nameof(OrderStatus.Pending)
            };
        }
    }
}
