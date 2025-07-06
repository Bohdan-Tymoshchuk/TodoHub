namespace TodoHub.Domain.Pagination;

public class PaginatedResult<TEntity>
    (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : notnull
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long TotalCount { get; } = count;
    public IEnumerable<TEntity> Items { get; } = data;
}