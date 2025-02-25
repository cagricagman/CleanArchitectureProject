using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository,ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
        if(orderToDelete == null)
            throw new OrderNotFoundException(nameof(Order), request.Id);
        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation($"Order {orderToDelete.Id} has been deleted.");
        return Unit.Value;
    }
}