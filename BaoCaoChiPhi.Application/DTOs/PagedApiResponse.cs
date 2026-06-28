namespace BaoCaoChiPhi.Application.DTOs;

public class PagedApiResponse<T>
{
    public int Code { get; set; } = 200;
    public string Message { get; set; } = "Success";
    public List<T> Data { get; set; } = [];
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public static PagedApiResponse<T> Success(List<T> data, int totalRecords, int pageNumber, int pageSize) =>
        new() { Data = data, TotalRecords = totalRecords, PageNumber = pageNumber, PageSize = pageSize };

    public static PagedApiResponse<T> Fail(string message, int code = 400) =>
        new() { Code = code, Message = message };
}
