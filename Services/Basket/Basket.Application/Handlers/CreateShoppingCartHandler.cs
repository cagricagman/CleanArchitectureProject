using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand,ShoppingCartResponse>
{
    private readonly IBasketRepository _repository;

    public CreateShoppingCartHandler(IBasketRepository repository)
    {
        _repository = repository;
    }
    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        //TODO burada Discount service kısmını entegre edeceğiz ilerde
        var shoppingCart = await _repository.UpdateBasket(new ShoppingCart
        {
            Items = request.Items,
            UserName = request.UserName,
        });
        var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
        return shoppingCartResponse;
    }
}