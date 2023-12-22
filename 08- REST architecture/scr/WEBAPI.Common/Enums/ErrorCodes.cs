namespace WEBAPI.Common.Enums
{
    /// <summary>
    /// From 0 to 99
    /// </summary>
    public enum ErrorCodes
    {
        Success = 0,
        InternalServerError = 1,
        DuplicateRequest = 2,
        InvalidGuid = 9,
        InvalidParameter = 10,
        MissingParameter = 11,
        UnauthorizedAction = 12,
        ConnectionToProviderFailed = 13,
        InvalidOperation = 14,
        InvalidEmail = 15,
        InvalidPhoneNumber = 16,
        InvalidUrl = 17,
        ForceVersionUpdate = 18,
        OptionalVersionUpdate = 19,
        HashedValueMismatchSignature = 20,
        RequestToProviderFailedException = 21,
        InvalidUniqueParameter = 22,
        InvalidNationalId = 23,
        InvalidUserInput = 24
    }
}