namespace Catalog.Core.Specs;

public class Pagination<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IReadOnlyList<T> Items { get; set; }

    public Pagination()
    {
        
    }

    public Pagination(int pageIndex,int pageSize,int count, IReadOnlyList<T> items)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Items = items;
    }
}