namespace CVAnalyzer.Models.OperationResultResponse
{
    public class OperationResultResponse<T>
    {
        public bool IsSuccess { get; }
        public T? Body { get; }
        public string? Message { get; }
        public ResultStatus Status { get; }
        
        public OperationResultResponse(T value)
        {
            IsSuccess = true;
            Body = value;
            Status = ResultStatus.Ok;
        }

        public OperationResultResponse(string message, ResultStatus status)
        {
            IsSuccess = false;
            Message = message;
            Status = status;
        }
    }
}