namespace Yaos.Api.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(bool IsSuccess, IEnumerable<Models.Order>? Orders, string? ErrorMessage)> GetOrdersAsync(int customerId);
    }
}
