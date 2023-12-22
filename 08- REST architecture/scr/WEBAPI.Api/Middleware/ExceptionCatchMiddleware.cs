using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WEBAPI.Common.Enums;
using WEBAPI.Common.Exceptions.Business;
using WEBAPI.Common.ViewModels;

namespace WEBAPI.Api.Middleware
{
    public class ExceptionCatchMiddleware
    {
        private readonly RequestDelegate _delegate;
        private readonly ILogger _logger;

        public ExceptionCatchMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionCatchMiddleware> logger)
        {
            _delegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _delegate(context);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message + Environment.NewLine + e.StackTrace);

                ////////////////////////////////////////////////////

                _logger.LogError("-----ERROR-----");

                _logger.LogError("Stack trace:");
                _logger.LogError(new EventId(e.HResult), e, e.Message);

                bool handled = false;

                //.. Handling Status Code ..//
                if (e is AggregateException)
                {
                    var aggregateException = e as AggregateException;
                    aggregateException.Handle((x) =>
                    {
                        if (x is UnauthorizedAccessException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            handled = true;
                            return true;
                        }
                        else if (x is ForbiddenException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            handled = true;
                            return true;
                        }
                        else
                        {
                            handled = false;
                            return true;
                        }
                        return false;
                    });
                }

                if (!handled)
                {
                    if (e is UnauthorizedAccessException)
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    else if (e is ForbiddenException)
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.OK;

                    BaseResponse errorResponse = new BaseResponse();
                    if (e is BaseException)
                    {
                        BaseException baseException = e as BaseException;

                        errorResponse.ErrorCode = baseException.Code;
                        errorResponse.ErrorMsg = baseException.Message;
                        errorResponse.ErrorDetails = baseException.MoreDetails;
                    }
                    else
                    {
                        var exType = e.GetType().BaseType;
                        if (exType.Name == "BaseException")
                        {
                            errorResponse.ErrorCode = (int)exType.GetProperty("Code")?.GetValue(e)!;
                            errorResponse.ErrorMsg = exType.GetProperty("Message")?.GetValue(e)?.ToString();
                            errorResponse.ErrorDetails = exType.GetProperty("MoreDetails")?.GetValue(e)?.ToString();
                        }
                        else
                        {
                            errorResponse.ErrorCode = (int)ErrorCodes.InternalServerError;
                            errorResponse.ErrorMsg = e.Message;

                            if (e.InnerException != null)
                                errorResponse.ErrorDetails = e.InnerException.Message;
                        }
                    }

                    _logger.LogError("Response error code: " + errorResponse.ErrorCode);
                    _logger.LogError("Response error message: " + errorResponse.ErrorMsg);
                    if (errorResponse.ErrorDetails != null)
                        _logger.LogError("Response error details: " + errorResponse.ErrorDetails);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize<BaseResponse>(errorResponse));
                }

                _logger.LogError("-----END ERROR-----");


            }
        }
    }
}