using FluentValidation;

namespace BuildingBlocks.Pagination;

public class PageRequestValidator : AbstractValidator<PageRequest>
{
    public PageRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(0).WithMessage("Page index must be greater than or equal to 0");
        RuleFor(r => r.Size).GreaterThan(0).WithMessage("Page size must be greater than 0");
    }
}
