using Customer.Domain.Enums;

namespace Customer.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Domain.Models.Customer> Customers =>
        new List<Domain.Models.Customer>
        {
            Domain.Models.Customer.Create(
                CustomerId.Of(Guid.Parse("e4f52443-6d2a-4780-b0bc-4cd1781059c2")),
                "Vega IT Sourcing", "200123456", "contact@vegaitsourcing.rs", "+381-21-6616-165", Address.Of("Bulevar Oslobođenja 127", "Serbia", "Novi Sad", "21000")
            ),
            Domain.Models.Customer.Create(
                CustomerId.Of(Guid.Parse("b5a99b4d-07d0-4cf0-97fa-46f3d41e58d9")),
                "Endava Serbia", "200234567", "belgrade@endava.com", "+381-11-6350-500", Address.Of("Bulevar Mihajla Pupina 10B", "Serbia", "Belgrade", "11070")
            ),
            Domain.Models.Customer.Create(
                CustomerId.Of(Guid.Parse("a8d0805f-b739-4c91-ae18-53359974e7ad")),
                "Levi9 Technology Services", "200345678", "info@levi9.com", "+381-21-4882-500", Address.Of("Narodnog Fronta 73", "Serbia", "Novi Sad", "21000")
            ),
            Domain.Models.Customer.Create(
                CustomerId.Of(Guid.Parse("d9fc79f1-9b77-46e0-b10b-8e8bc5d06b39")),
                "Enjoying.rs", "200678901", "info@enjoying.rs", "+381-11-3285-200", Address.Of("Balkanska 44", "Serbia", "Belgrade", "11000")
            ),
            Domain.Models.Customer.Create(
                CustomerId.Of(Guid.Parse("34fa6746-4c29-48a2-97a6-9fa4b80d57e1")),
                "Quantox Technology", "200567890", "info@quantox.com", "+381-64-3000-400", Address.Of("Savska 41", "Serbia", "Belgrade", "11000")
            )
        };

    public static IEnumerable<Account> Accounts =>
        new List<Account>
        {
            Account.Create(
                AccountId.Of(Guid.Parse("92b70435-9d32-4b59-9c44-88dbad957f63")),
                CustomerId.Of(Guid.Parse("e4f52443-6d2a-4780-b0bc-4cd1781059c2")),
                "IT Infrastructure", "IT"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("fd4f6788-2db5-487d-86b6-bd8d3b1f3b91")),
                CustomerId.Of(Guid.Parse("e4f52443-6d2a-4780-b0bc-4cd1781059c2")),
                "Systems Support", "IT"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("7acb44e5-ccf5-49db-b167-83187e846e93")),
                CustomerId.Of(Guid.Parse("b5a99b4d-07d0-4cf0-97fa-46f3d41e58d9")),
                "Network Services", "IT"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("8a272396-52b5-4552-b550-d4786c65f9bb")),
                CustomerId.Of(Guid.Parse("b5a99b4d-07d0-4cf0-97fa-46f3d41e58d9")),
                "Software Development", "R&D"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("c4b4dbb4-6a8f-4d56-95d9-d6fa9d7ef63c")),
                CustomerId.Of(Guid.Parse("a8d0805f-b739-4c91-ae18-53359974e7ad")),
                "Product Innovation", "R&D"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("79eeb02c-c465-46d6-9d77-836f2d1a7a7a")),
                CustomerId.Of(Guid.Parse("d9fc79f1-9b77-46e0-b10b-8e8bc5d06b39")),
                "Cloud Solutions", "IT"
            ),
            Account.Create(
                AccountId.Of(Guid.Parse("b619f9f0-0381-40e9-9131-22f6587c54be")),
                CustomerId.Of(Guid.Parse("34fa6746-4c29-48a2-97a6-9fa4b80d57e1")),
                "Prototype Development", "R&D"
            )
        };

    public static IEnumerable<SoftwareLicense> SoftwareLicenses =>
        new List<SoftwareLicense>
        {
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("3a8f5368-87da-4532-b11f-e0a8c43d38db")),
                AccountId.Of(Guid.Parse("92b70435-9d32-4b59-9c44-88dbad957f63")),
                "Microsoft", "Windows Server 2019", 10, Guid.Parse("3a8f5368-87da-4532-b11f-e0a8c43d38db"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("5f23e6db-bb7f-4632-b8e0-52c85bc5e20b")),
                AccountId.Of(Guid.Parse("92b70435-9d32-4b59-9c44-88dbad957f63")),
                "Cisco", "Cisco WebEx", 50, Guid.Parse("5f23e6db-bb7f-4632-b8e0-52c85bc5e20b"), DateTime.Now, DateTime.Now.AddMonths(3)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("3bc1d6a2-1c61-4de9-91fc-2d05b469711d")),
                AccountId.Of(Guid.Parse("fd4f6788-2db5-487d-86b6-bd8d3b1f3b91")),
                "McAfee", "McAfee Total Protection", 100, Guid.Parse("3bc1d6a2-1c61-4de9-91fc-2d05b469711d"), DateTime.Now, DateTime.Now.AddMonths(3)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("4e379a7f-52b0-45a5-a10d-6a5b9ed6e727")),
                AccountId.Of(Guid.Parse("7acb44e5-ccf5-49db-b167-83187e846e93")),
                "Atlassian", "JIRA Software", 15, Guid.Parse("4e379a7f-52b0-45a5-a10d-6a5b9ed6e727"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("8746743d-71e9-43b4-904b-cd7262060f8b")),
                AccountId.Of(Guid.Parse("7acb44e5-ccf5-49db-b167-83187e846e93")),
                "Atlassian", "Confluence", 15, Guid.Parse("8746743d-71e9-43b4-904b-cd7262060f8b"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("8e63cc65-5cf0-48a9-b0d4-5fd0bdfab016")),
                AccountId.Of(Guid.Parse("8a272396-52b5-4552-b550-d4786c65f9bb")),
                "Microsoft", "Azure DevOps", 25, Guid.Parse("8e63cc65-5cf0-48a9-b0d4-5fd0bdfab016"), DateTime.Now, DateTime.Now.AddMonths(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("db9b5d7d-bc69-4774-8611-84582322769e")),
                AccountId.Of(Guid.Parse("8a272396-52b5-4552-b550-d4786c65f9bb")),
                "JetBrains", "IntelliJ IDEA", 40, Guid.Parse("db9b5d7d-bc69-4774-8611-84582322769e"),DateTime.Now, DateTime.Now.AddMonths(3)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("38fc3f60-dc29-466b-8f79-82be12b159ad")),
                AccountId.Of(Guid.Parse("c4b4dbb4-6a8f-4d56-95d9-d6fa9d7ef63c")),
                "JetBrains", "ReSharper", 10, Guid.Parse("38fc3f60-dc29-466b-8f79-82be12b159ad"), DateTime.Now, DateTime.Now.AddMonths(3)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("c8692e0f-d34d-4aeb-9513-828a4bcfb264")),
                AccountId.Of(Guid.Parse("79eeb02c-c465-46d6-9d77-836f2d1a7a7a")),
                "AWS", "Amazon EC2", 30, Guid.Parse("c8692e0f-d34d-4aeb-9513-828a4bcfb264"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("dfad4d36-ea52-4d1f-b839-c01f9f0c0cf5")),
                AccountId.Of(Guid.Parse("79eeb02c-c465-46d6-9d77-836f2d1a7a7a")),
                "Google", "Google Cloud", 50, Guid.Parse("dfad4d36-ea52-4d1f-b839-c01f9f0c0cf5"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("3b2e4b8b-604d-44a3-a8b6-079d4a60a96f")),
                AccountId.Of(Guid.Parse("b619f9f0-0381-40e9-9131-22f6587c54be")),
                "Microsoft", "Office 365", 25, Guid.Parse("3b2e4b8b-604d-44a3-a8b6-079d4a60a96f"), DateTime.Now, DateTime.Now.AddYears(1)
            ),
            SoftwareLicense.Create(
                SoftwareLicenseId.Of(Guid.Parse("576e407e-b45f-4e3e-8b92-330e939a845d")),
                AccountId.Of(Guid.Parse("b619f9f0-0381-40e9-9131-22f6587c54be")),
                "JetBrains", "PyCharm", 30, Guid.Parse("576e407e-b45f-4e3e-8b92-330e939a845d"), DateTime.Now, DateTime.Now.AddMonths(1)
            )
        };
}
