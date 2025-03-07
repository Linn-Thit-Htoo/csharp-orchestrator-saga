using csharp_orchestrator_saga.OrderOrchestrator.Entities;
using csharp_orchestrator_saga.OrderOrchestrator.Features.Order.CreateOrder;
using csharp_orchestrator_saga.OrderOrchestrator.Persistence.Base;
using Microsoft.EntityFrameworkCore;

namespace csharp_orchestrator_saga.OrderOrchestrator.Services.Stock
{
    public class StockService : IStockService
    {
        private readonly IRepositoryBase<TblStock> _stockRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(IRepositoryBase<TblStock> stockRepository, ILogger<StockService> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task ProcessStock(CreateOrderCommand command, CancellationToken cs = default)
        {
            foreach (var item in command.OrderDetails)
            {
                var stock = await _stockRepository
                    .GetByCondition(x => x.ProductId == item.ProductId)
                    .SingleOrDefaultAsync(cancellationToken: cs);
                if (stock is null)
                {
                    _logger.LogError($"Product Id: {item.ProductId} not found in stock.");
                    throw new Exception("Stock not found.");
                }

                if (item.TotalItems > stock.Stock)
                {
                    _logger.LogError("Insufficient stock.");
                    throw new Exception("Insufficient stock.");
                }

                stock.Stock = stock.Stock - item.TotalItems;
                _stockRepository.Update(stock);
                await _stockRepository.SaveChangesAsync(cs);
            }
        }
    }
}
