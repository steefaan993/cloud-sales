using BuildingBlocks.Exceptions;

namespace Customer.Application.Exceptions;

public class CustomerNotFoundException(Guid id) : NotFoundException("Customer", id)
{
}
