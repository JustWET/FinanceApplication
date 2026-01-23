namespace PersonalFinanceDataManager.Core.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = Guid.NewGuid().ToString();
            context.Items["CorrelationId"] = correlationId;

            var requestInfo = await FormatRequest(context.Request);
            _logger.LogInformation("REQUEST [{CorrelationId}]: {Request}", correlationId, requestInfo);

            var originalResponseBody = context.Response.Body;

            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context);

            var responseInfo = await FormatResponse(context.Response);
            _logger.LogInformation("RESPONSE [{CorrelationId}]: Status={StatusCode}, Body={ResponseBody}",
                correlationId,
                context.Response.StatusCode,
                responseInfo);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var bodyStream = new StreamReader(request.Body);
            var bodyText = await bodyStream.ReadToEndAsync();
            request.Body.Position = 0;

            bodyText = RemoveSensitiveData(bodyText);

            return $"{request.Method} {request.Path} Body={bodyText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            var bodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            bodyText = RemoveSensitiveData(bodyText);

            return bodyText;
        }

        private string RemoveSensitiveData(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return body;

            foreach (var field in new[] { "password", "token", "secret" })
            {
                if (body.Contains(field, StringComparison.OrdinalIgnoreCase))
                {
                    body = System.Text.RegularExpressions.Regex.Replace(
                        body,
                        $"\"{field}\":\\s*\"[^\"]+\"",
                        $"\"{field}\": \"***\"",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );
                }
            }

            return body;
        }
    }
}
