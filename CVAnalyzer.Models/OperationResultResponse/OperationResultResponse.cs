namespace CVAnalyzer.Models.OperationResultResponse
{
    public class OperationResultResponse<T>
    {
        public bool IsSuccess { get; }
        public T? Result { get; }
        public string? Error { get; }
        public ResultStatus Status { get; }
        
        public OperationResultResponse(T value)
        {
            IsSuccess = true;
            Result = value;
            Status = ResultStatus.Ok;
        }

        public OperationResultResponse(string error, ResultStatus status)
        {
            IsSuccess = false;
            Error = error;
            Status = status;
        }
    }
}