using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand,ShoppingCartResponse>
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;

    public CreateShoppingCartHandler(IBasketRepository repository,DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
    }
    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            item.Price -= coupon.Amount;
        }
        var shoppingCart = await _repository.UpdateBasket(new ShoppingCart
        {
            Items = request.Items,
            UserName = request.UserName,
        });
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
        return shoppingCartResponse;
    }
}