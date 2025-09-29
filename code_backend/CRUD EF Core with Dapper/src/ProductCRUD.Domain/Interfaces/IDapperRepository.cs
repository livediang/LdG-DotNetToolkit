using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCRUD.Domain.Interfaces
{
    public interface IDapperRepository
    {
        Task<T?> QuerySingleAsync<T>(string sql, object? param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
        Task<(IEnumerable<T> Items, int Total)> QueryPagedAsync<T>(string baseSql, string countSql, object? param, int page, int pageSize);
    }
}
