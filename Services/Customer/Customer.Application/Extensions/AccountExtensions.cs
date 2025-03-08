namespace Customer.Application.Extensions;

public static class AccountExtensions
{
    public static IEnumerable<AccountData> ToAccountDataList(this IEnumerable<Account> accounts)
    {
        if (!accounts.Any())
        {
            return Enumerable.Empty<AccountData>();
        }

        return accounts.Select(DataFromAccount);
    }

    public static AccountData ToAccountData(this Account account)
    {
        return DataFromAccount(account);
    }

    private static AccountData DataFromAccount(Account account)
    {
        return new AccountData(
                    Name: account.Name,
                    Department: account.Department
                );
    }
}
