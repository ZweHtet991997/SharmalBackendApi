using SharmalRealEstateSystem.Shared.Configs;

namespace DotNet7.SharmalRealEstateSample.Shared.Services;

public class DapperService
{
    private readonly IConfiguration _configuration;

    public DapperService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<T> Query<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );
        List<T> lst = db.Query<T>(query, parameters, commandType: commandType).ToList();
        return lst;
    }

    public async Task<List<T>> QueryAsync<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );
        var lst = await db.QueryAsync<T>(query, parameters, commandType: commandType);
        return lst.ToList();
    }

    //Read Multiple data result
    public async Task<(IEnumerable<TData>, int)> QueryMultipleAsync<TData>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );

        using var multi = await db.QueryMultipleAsync(query, parameters, commandType: commandType);

        var data = multi.Read<TData>();
        var totalCount = multi.ReadSingle<int>();

        return (data, totalCount);
    }

    public async Task<int> GetTotalCountAsync(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );

        return await db.ExecuteScalarAsync<int>(query, parameters, commandType: commandType);
    }

    public T QueryFirstOrDefault<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );

        var item = db.Query<T>(query, parameters, commandType: commandType).FirstOrDefault();
        return item!;
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string query,
        object? parameters = null,
        CommandType commandType = CommandType.Text
    )
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );
        var item = await db.QueryFirstOrDefaultAsync<T>(
            query,
            parameters,
            commandType: commandType
        );
        return item;
    }

    public int Execute(string query, object? parameters = default)
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );
        return db.Execute(query, parameters);
    }

    public async Task<int> ExecuteAsync(string query, object? parameters = default)
    {
        using IDbConnection db = Deployment.IsDevelopment() ?
            new SqlConnection(
            DatabaseConfig.UATDbConnectionString()
        ) : new SqlConnection(
            DatabaseConfig.ProdDbConnectionString()
        );
        return await db.ExecuteAsync(query, parameters);
    }
}
