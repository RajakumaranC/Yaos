namespace Yaos.Api.Search.Interfaces
{
    public interface ICustomersService
    {
        Task<(bool IsSuccess, Models.Customer? Customers, string? ErrorMessage)> GetCustomerAsync(int customerId);
    }
}
