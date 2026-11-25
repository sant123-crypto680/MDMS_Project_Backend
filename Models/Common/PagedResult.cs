namespace MDMS_PRoject_Backend.Models.Common;

/// <summary>
/// Generic paginated result wrapper returned by list endpoints when ?page= is provided.
/// </summary>
public class PagedResult<T>
{
    public int Page       { get; set; }
    public int PageSize   { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext     => Page < TotalPages;
    public List<T> Items  { get; set; } = new();
}
