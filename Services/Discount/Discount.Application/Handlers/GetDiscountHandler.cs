using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountHandler : IRequestHandler<GetDiscountQuery,CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupons = await _discountRepository.GetDiscountAsync(request.ProductName);
        if(coupons is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"No discount found for product: {request.ProductName}"));
        var couponModel = new CouponModel
        {
            Id = coupons.Id,
            ProductName = coupons.ProductName,
            Description = coupons.Description,
            Amount = coupons.Amount
        };
        return couponModel;
    }
}