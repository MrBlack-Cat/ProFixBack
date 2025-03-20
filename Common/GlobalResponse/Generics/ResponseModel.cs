namespace Common.GlobalResponse;

public class ResponseModel<T> : ResponseModel
{
    public T? Data { get; set; }

    public ResponseModel(List<string> messages) : base(messages) { }

    public ResponseModel() : base()
    {
        Data = default;
    }
}
