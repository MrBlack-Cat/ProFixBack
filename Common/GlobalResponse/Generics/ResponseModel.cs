namespace Common.GlobalResponse;

public class ResponseModel<T> : ResponseModel
{
    public T? Data { get; set; }

    public ResponseModel(List<string> messages) : base(messages)
    {
    }

    public ResponseModel()
    {
        Data = default;
    }

    public static ResponseModel<T> Success(T data)
    {
        return new ResponseModel<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static ResponseModel<T> Fail(params string[] errors)
    {
        return new ResponseModel<T>
        {
            IsSuccess = false,
            Errors = errors.ToList()
        };
    }
}
