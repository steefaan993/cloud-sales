namespace Catalog.API.Models;

public class Software
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Vendor { get; set; } = default!;
    public decimal Price { get; set; } = default!;
}
