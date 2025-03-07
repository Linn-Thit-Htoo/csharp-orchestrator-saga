using csharp_orchestrator_saga.OrderOrchestrator.Entities;
using csharp_orchestrator_saga.OrderOrchestrator.Extensions;
using csharp_orchestrator_saga.OrderOrchestrator.Persistence.Base;
using csharp_orchestrator_saga.OrderOrchestrator.Services.OrderDetail;
using csharp_orchestrator_saga.OrderOrchestrator.Services.Stock;
using csharp_orchestrator_saga.OrderOrchestrator.Utils;
using MediatR;
using System.Transactions;

namespace csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
    {
        private readonly IRepositoryBase<TblOrder> _orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IStockService _stockService;

        public CreateOrderCommandHandler(IRepositoryBase<TblOrder> orderRepository, ILogger<CreateOrderCommandHandler> logger, IOrderDetailService orderDetailService, IStockService stockService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _orderDetailService = orderDetailService;
            _stockService = stockService;
        }

        public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead },
                TransactionScopeAsyncFlowOption.Enabled
            );

            var order = request.ToEntity();
            await _orderRepository.AddAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order Processed...");

            await _orderDetailService.ProcessOrderDetail(request, order.OrderId, order.InvoiceNo, cancellationToken);
            _logger.LogInformation("Order Detail Processed...");

            await _stockService.ProcessStock(request, cancellationToken);
            _logger.LogInformation("Stock Reduction Processed...");

            scope.Complete();

            return Result<CreateOrderResponse>.Success(new CreateOrderResponse() { OrderId = order.OrderId });
        }
    }
}
