namespace WEBAPI.IntegrationTests.Mock;
public class TestPagedList<T> 
{
    public List<T>? List { set; get; }
    public int TotalCount { set; get; }
    public bool HasMore { set; get; }
    public int? SkipCount { set; get; }
    public int? TakeCount { set; get; }
}