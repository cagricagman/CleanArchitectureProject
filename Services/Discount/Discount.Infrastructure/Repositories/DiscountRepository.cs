using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<Coupon> GetDiscountAsync(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
        return (coupon == null) ? new Coupon{ProductName = "No Discount", Amount = 0,Description = "No Discount Availables"} : coupon;
    }

    public async Task<bool> CreateDiscountAsync(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});
        return affected  == 0 ? false : true;
    }

    public async Task<bool> UpdateDiscountAsync(Coupon coupon)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE Id=@Id",new Coupon{ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id});
        return affected == 0 ? false : true;
    }

    public async Task<bool> DeleteDiscountAsync(string productName)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName=@ProductName",new {ProductName = productName});
        return affected == 0 ? false : true;
    }
}