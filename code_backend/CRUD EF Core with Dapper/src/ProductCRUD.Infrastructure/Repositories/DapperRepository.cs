using ProductCRUD.Domain.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ProductCRUD.Infrastructure.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly string _connectionString;

        public DapperRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("TestConnection")!;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.QueryAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using var conn = CreateConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        public async Task<(IEnumerable<T> Items, int Total)> QueryPagedAsync<T>(string baseSql, string countSql, object? param, int page, int pageSize)
        {
            using var conn = CreateConnection();
            var offset = (page - 1) * pageSize;
            var pagedSql = $"{baseSql} ORDER BY (SELECT 1) OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            var items = await conn.QueryAsync<T>(pagedSql, new DynamicParameters(new { Offset = offset, PageSize = pageSize }));
            var total = await conn.QuerySingleAsync<int>(countSql, param);
            return (items, total);
        }
    }
}
