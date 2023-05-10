namespace user_app.Dtos;
public class Page
{
    public bool IsInValidState => PageSize > 0 && PageNumber > 0;
    public int Offset => (PageNumber - 1) * PageSize;
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}