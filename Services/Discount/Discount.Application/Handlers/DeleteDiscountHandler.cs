using Discount.Application.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, bool>
{
    private readonly IDiscountRepository _discountRepository;

    public DeleteDiscountHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        return await _discountRepository.DeleteDiscountAsync(request.ProductName);
    }
}