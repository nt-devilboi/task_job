using Dapper;
using Npgsql;

namespace TestJobSys.Db;

public class ElementRepository(string connectionString)
{
    private NpgsqlConnection CreateConnection() => new(connectionString);

    public async Task AddRangeAsync(IEnumerable<Element> elements)
    {
        const string sql = """
                           INSERT INTO elements (attribute_value, outer_html)
                           VALUES (@AttributeValue, @OuterHtml);
                           """;

        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        await connection.ExecuteAsync(sql, elements, transaction);

        await transaction.CommitAsync();
    }
}