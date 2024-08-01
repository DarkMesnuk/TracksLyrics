using Common.Models;

namespace Common.Helpers;

public enum StatusCodes
{
    Success = 1000,
    NotFound = 1001,
    SomethingWentWrong = 1002,
    SomeFieldsMustBeUnique = 1003,
    WrongDataFormat = 1004,
    UnsupportedContentType = 1005,
    CreationFailed = 1006,
    UpdateFailed = 1007,
    DeleteFailed = 1008,
    FileUploadFailed = 1009,
    NoCreatingForeignKey = 1010,
    AccessDenied = 1011,
    ValidationFailed = 1012,
    IncorrectCredentials = 1013,
}

public static class ApplicationResponseStatuses
{
    public static Dictionary<StatusCodes, ApplicationResponse> Statuses;

    static ApplicationResponseStatuses()
    {
        Statuses = new Dictionary<StatusCodes, ApplicationResponse>();
        
        Statuses
            .CreatePair(StatusCodes.Success, "Success", 200)
            .CreatePair(StatusCodes.NotFound, "Not found", 404)
            .CreatePair(StatusCodes.SomethingWentWrong, "Something went wrong", 400)
            .CreatePair(StatusCodes.SomeFieldsMustBeUnique, "Some fields must be unique", 400)
            .CreatePair(StatusCodes.NoCreatingForeignKey, "Not creating foreign key", 400)
            .CreatePair(StatusCodes.WrongDataFormat, "Wrong data format", 400)
            .CreatePair(StatusCodes.UnsupportedContentType, "Unsupported content type", 415)
            .CreatePair(StatusCodes.FileUploadFailed, "File upload to cloud failed", 400)
            .CreatePair(StatusCodes.CreationFailed, "Creation failed", 400)
            .CreatePair(StatusCodes.UpdateFailed, "Failed to update", 400)
            .CreatePair(StatusCodes.DeleteFailed, "Failed to delete", 400)
            .CreatePair(StatusCodes.AccessDenied, "Access denied", 403)
            .CreatePair(StatusCodes.ValidationFailed, "Validation failed", 400)
            .CreatePair(StatusCodes.IncorrectCredentials, "Incorrect credentials", 400);
    }
    
    private static Dictionary<StatusCodes, ApplicationResponse> CreatePair(this Dictionary<StatusCodes, ApplicationResponse> statuses, StatusCodes statusCode, string description, int httpCode)
    {
        statuses.Add(statusCode, new ApplicationResponse { Status = statusCode.GetStatusCode(), Type = statusCode.ToString(), Description = description, HttpCode = httpCode} );
        return statuses;
    }

    private static string GetStatusCode(this StatusCodes code)
    {
        return $"{(int)code}";
    }
}