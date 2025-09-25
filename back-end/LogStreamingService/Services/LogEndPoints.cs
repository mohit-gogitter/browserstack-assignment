namespace LogStreamingService.Services
{
    public static class LogEndPoints
    {
        public static async Task StreamLogs(LogStreamerService service, HttpContext context)
        {
            context.Response.Headers.Add("Content-Type", "text/event-stream");
            await service.StreamLogsAsync(context);
        }
    }
}
