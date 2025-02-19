using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery,IList<TypeResponse>>
{
    private readonly ITypesRepository _typesRepository;

    public GetAllTypesHandler(ITypesRepository typesRepository)
    {
        _typesRepository = typesRepository;
    }
    public async Task<IList<TypeResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
    {
        var typeList = await _typesRepository.GetAllTypesAsync();
        var typeListResponse = ProductMapper.Mapper.Map<IList<TypeResponse>>(typeList);
        return typeListResponse;
    }
}