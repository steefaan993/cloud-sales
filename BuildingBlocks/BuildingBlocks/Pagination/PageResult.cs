namespace BuildingBlocks.Pagination;

public class PageResult<TEntity>
    (int page, int size, long total, IEnumerable<TEntity> data) 
    where TEntity : class
{
    public int Page { get; } = page;
    public int Size { get; } = size;
    public long Total { get; } = total;
    public IEnumerable<TEntity> Data { get; } = data;
}
