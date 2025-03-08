namespace Purchase.API.Dtos;

public record CCPPurchaseSoftwareRequest(string SoftwareName, string Vendor, int PeriodInMohtns, int Quantity);
