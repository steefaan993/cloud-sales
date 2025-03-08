namespace Customer.Application.Extensions;
public static class SoftwareLicenseExtensions
{
    public static IEnumerable<SoftwareLicenseData> ToSoftwareLicenseDataList(this IEnumerable<SoftwareLicense> softwareLicenses)
    {
        if (!softwareLicenses.Any())
        {
            return Enumerable.Empty<SoftwareLicenseData>();
        }

        return softwareLicenses.Select(DataFromSoftwareLicense);
    }

    public static SoftwareLicenseData ToSoftwareLicenseData(this SoftwareLicense softwareLicense)
    {
        return DataFromSoftwareLicense(softwareLicense);
    }

    private static SoftwareLicenseData DataFromSoftwareLicense(SoftwareLicense softwareLicense)
    {
        return new SoftwareLicenseData(
                    Vendor: softwareLicense.Vendor,
                    SoftwareName: softwareLicense.SoftwareName,
                    Quantity: softwareLicense.Quantity,
                    State: softwareLicense.State.ToString(),
                    ReferenceId: softwareLicense.ReferenceId,
                    ValidFrom: softwareLicense.ValidFrom,
                    ValidTo: softwareLicense.ValidTo
                );
    }
}
