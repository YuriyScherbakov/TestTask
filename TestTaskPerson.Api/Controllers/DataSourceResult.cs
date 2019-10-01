using System.Collections.Generic;
using System.Linq;

namespace TestTaskPerson.Api.Controllers
{
    /// <summary>
    /// Uses to return business items with their position with total count data set.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DataSourceResult<TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; } = Enumerable.Empty<TEntity>();
        public long Total { get; set; }
    }
}