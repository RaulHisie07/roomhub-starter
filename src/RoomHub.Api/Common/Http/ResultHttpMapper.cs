using RoomHub.Api.Domain.Common;
namespace RoomHub.Api.Common.Http;
public static class ResultHttpMapper
{
    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess) return onSuccess(result.Value);
        var status = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => throw new InvalidOperationException("Unknown error type.")
        };
        return Results.Problem(statusCode: status, title: result.Error.Code, detail: result.Error.Message);
    }
}
