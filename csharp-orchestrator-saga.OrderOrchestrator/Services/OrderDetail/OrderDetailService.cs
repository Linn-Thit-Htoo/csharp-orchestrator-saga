using csharp_orchestrator_saga.OrderOrchestrator.Entities;
using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;
using csharp_orchestrator_saga.OrderOrchestrator.Persistence.Base;

namespace csharp_orchestrator_saga.OrderOrchestrator.Services.OrderDetail
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepositoryBase<TblOrderDetail> _orderDetailRepository;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(IRepositoryBase<TblOrderDetail> orderDetailRepository, ILogger<OrderDetailService> logger)
        {
            _orderDetailRepository = orderDetailRepository;
            _logger = logger;
        }

        public async Task ProcessOrderDetail(CreateOrderCommand command, Guid orderId, Guid invoice, CancellationToken cs = default)
        {
            foreach (var item in command.OrderDetails)
            {
                await _orderDetailRepository.AddAsync(new TblOrderDetail()
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    InvoiceNo = invoice,
                    TotalItems = item.TotalItems,
                    SubTotal = item.SubTotal
                }, cs);
            }

            await _orderDetailRepository.SaveChangesAsync(cs);
        }
    }
}
