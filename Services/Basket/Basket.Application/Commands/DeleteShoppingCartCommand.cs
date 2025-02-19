using MediatR;

namespace Basket.Application.Commands;

public class DeleteShoppingCartCommand : IRequest<Unit>
{
    public DeleteShoppingCartCommand(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}