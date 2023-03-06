namespace EFO.WebUi.Data;

public interface IOrderService
{
    Task AddOrderItemAsync(AddOrderItemDto item);
}
