namespace ShriGo.Pages
{
    public class TrackingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<TrackingMiddleware> _logger;

        public TrackingMiddleware(RequestDelegate next, ILogger<TrackingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Logic before the next middleware
            _logger.LogInformation("Tracking Request");

            await _next(context); // 2. Call the next middleware

            // 3. Logic after the next middleware
            _logger.LogInformation("Tracking Response");
        }
    }
}
