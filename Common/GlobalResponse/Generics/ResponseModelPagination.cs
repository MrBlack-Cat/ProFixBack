namespace Common.GlobalResponse.Generics;

public class ResponseModelPagination<T> : ResponseModel
{
    public Pagination<T> Data { get; set; }

    public ResponseModelPagination(List<string> messages) : base(messages)
    {
    }

    public ResponseModelPagination()
    {
    }
}
