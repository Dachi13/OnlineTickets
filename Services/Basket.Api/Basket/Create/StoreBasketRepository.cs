using Basket.Api.Models;

namespace Basket.Api.Basket.Create;

public class StoreBasketRepository(DapperContext context) : IStoreBasketRepository
{
    public async Task<Result<long?>> StoreToBasket(EventsBasket basket)
    {
        var connection = await context.CreateConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            long? basketId = null;

            foreach (Event @event in basket.Events)
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_basketid", basketId, DbType.Int64);
                parameters.Add("p_ticketid", @event.TicketId);
                parameters.Add("p_amount", @event.Amount);
                parameters.Add("p_price", @event.Price);
                parameters.Add("p_discountid", basket.DiscountId);
                parameters.Add("basketid", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "public.spaddbasket",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                basketId = parameters.Get<long>("basketid");
            }

            await transaction.CommitAsync();
            return basketId;
        }
        catch (NpgsqlException exception)
        {
            await transaction.RollbackAsync();
            return new Error("Database_Error", exception.Message, ErrorType.DatabaseError);
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            return new Error("Internal_Error", exception.Message, ErrorType.InternalServerError);
        }
    }
}