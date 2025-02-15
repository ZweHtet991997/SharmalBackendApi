namespace SharmalRealEstateSystem.Models.Features;

public class Result<T>
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public T Data { get; set; }
    public string Message { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Token { get; set; }

    [JsonIgnore]
    public bool IsSuccess { get; set; }
    public EnumStatusCode StatusCode { get; set; }

    [JsonIgnore]
    public bool IsError
    {
        get { return !IsSuccess; }
    }

    public static Result<T> SuccessResult(
        string message = "Success.",
        EnumStatusCode statusCode = EnumStatusCode.Success
    )
    {
        return new Result<T>
        {
            Message = message,
            IsSuccess = true,
            StatusCode = statusCode
        };
    }

    public static Result<T> SuccessResult(
        string token,
        string message = "Success.",
        EnumStatusCode statusCode = EnumStatusCode.Success
    )
    {
        return new Result<T>
        {
            Message = message,
            IsSuccess = true,
            StatusCode = statusCode
        };
    }

    public static Result<T> SuccessResult(
        T data,
        string message = "Success.",
        EnumStatusCode statusCode = EnumStatusCode.Success
    )
    {
        return new Result<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static Result<T> FailureResult(
        string message = "Fail.",
        EnumStatusCode statusCode = EnumStatusCode.BadRequest
    )
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static Result<T> FailureResult(
        Exception ex,
        EnumStatusCode statusCode = EnumStatusCode.InternalServerError
    )
    {
        return new Result<T>
        {
            Message = ex.ToString(),
            IsSuccess = false,
            StatusCode = statusCode
        };
    }

    public static Result<T> ExecuteResult(
        int result,
        EnumStatusCode successStatusCode = EnumStatusCode.Success,
        EnumStatusCode failureStatusCode = EnumStatusCode.BadRequest
    )
    {
        return result > 0
            ? Result<T>.SuccessResult(statusCode: successStatusCode)
            : Result<T>.FailureResult(statusCode: failureStatusCode);
    }
}
