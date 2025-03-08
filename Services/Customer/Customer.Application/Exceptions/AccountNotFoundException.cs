using BuildingBlocks.Exceptions;

namespace Customer.Application.Exceptions;

public class AccountNotFoundException(Guid id) : NotFoundException("Account", id)
{
}
