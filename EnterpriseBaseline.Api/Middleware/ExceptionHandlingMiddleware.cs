using EnterpriseBaseline.Application.Common;
using EnterpriseBaseline.Application.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;


namespace EnterpriseBaseline.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessException(context, ex);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                await HandleException(
                    context,
                    HttpStatusCode.BadRequest,
                    "VALIDATION_FAILED",
                    "Duplicate value detected"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await HandleException(
                    context,
                    HttpStatusCode.InternalServerError,
                    "INTERNAL_ERROR",
                    "An unexpected error occurred"
                );
            }
        }

        private static async Task HandleBusinessException(
            HttpContext context,
            BusinessException ex)
        {
            var statusCode = ex switch
            {
                UnauthorizedException => HttpStatusCode.Unauthorized,
                ValidationException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.BadRequest
            };

            var error = new ErrorResponse
            {
                Error =
                {
                    Code = ex.Code,
                    Message = ex.Message,
                    Details = (ex as ValidationException)?.Details
                },
                TraceId = context.TraceIdentifier
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(error)
            );
        }

        private static async Task HandleException(
            HttpContext context,
            HttpStatusCode statusCode,
            string code,
            string message)
        {
            var error = new ErrorResponse
            {
                Error =
                {
                    Code = code,
                    Message = message
                },
                TraceId = context.TraceIdentifier
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(error)
            );
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number == 2601 || sqlEx.Number == 2627;
            }

            return false;
        }
    }
}
