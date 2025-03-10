using Carter;
using CCP.API.Models;

namespace CCP.API.Softwares.GetSoftwares;

public record GetSoftwareCatalogResponse(IEnumerable<Software> Softwares);

public class GetSoftwareCatalogEndpoint : ICarterModule
{
    private static readonly List<Software> softwares =
    [
        new Software("Windows Server", "Microsoft", 75),
        new Software("Office 365", "Microsoft", 8),
        new Software("Azure Virtual Machines", "Microsoft", 70),
        new Software("SQL Server", "Microsoft", 120),
        new Software("Windows Server 2019", "Microsoft", 50),
        new Software("Azure DevOps", "Microsoft", 30),
        new Software("AWS EC2", "Amazon", 45),
        new Software("Amazon RDS", "Amazon", 60),
        new Software("AWS Lambda", "Amazon", 20),
        new Software("Amazon S3", "Amazon", 25),
        new Software("Amazon CloudFront", "Amazon", 40),
        new Software("Google Cloud Compute Engine", "Google", 50),
        new Software("Google Kubernetes Engine", "Google", 75),
        new Software("Google Cloud Storage", "Google", 25),
        new Software("Google BigQuery", "Google", 100),
        new Software("Google Cloud Spanner", "Google", 140),
        new Software("Oracle Cloud Infrastructure", "Oracle", 60),
        new Software("Oracle Autonomous Database", "Oracle", 300),
        new Software("Oracle Cloud Storage", "Oracle", 20),
        new Software("Oracle WebLogic Server", "Oracle", 125),
        new Software("Oracle Java SE", "Oracle", 25),
        new Software("Cisco WebEx", "Cisco", 25),
        new Software("McAfee Total Protection", "McAfee", 25),
        new Software("JIRA Software", "Attlasian", 15),
        new Software("Confluence", "Attlasian", 15),
        new Software("IntelliJ IDEA", "JetBrains", 20),
        new Software("ReSharper", "JetBrains", 10),
        new Software("PyCharm", "JetBrains", 20)
    ];

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/ccp/software-catalog", () =>
        {
            var items = softwares
                .OrderBy(s => s.Vendor)
                .OrderBy(s => s.SoftwareName)
                .ToList();

            var response = new GetSoftwareCatalogResponse(items);
            return Results.Ok(response);
        })

        .WithName("GetSoftwareCatalog")
        .Produces<GetSoftwareCatalogResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get software catalog - Mock")
        .WithDescription("Get software catalog - Mock");
    }
}
