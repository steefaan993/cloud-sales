using BuildingBlocks.Exceptions;

namespace Customer.Application.Exceptions;

public class SoftwareLicenseNotFoundException(Guid id) : NotFoundException("Software license", id)
{
}
