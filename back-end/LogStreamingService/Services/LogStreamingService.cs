namespace LogStreamingService.Services
{
    public class LogStreamerService
    {
        private readonly string _logFilePath = "C://Users//Mohit//OneDrive//Desktop//browser-stack//back-end//log.txt";

        /// <summary>
        /// Stream the updates in the log file to the connected clients using SSE
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task StreamLogsAsync(HttpContext context)
        {
            var response = context.Response;

            using var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


            var lastLines = await GetLastNLines(10);
            foreach (var line in lastLines)
            {
                await response.WriteAsync($"data: {line}\n\n");
                await response.Body.FlushAsync();
            }


            //foreach (var line in reader.ReadLineAsync().Result)
            //{
            //    await response.WriteAsync($"data: {line}\n\n");
            //    await response.Body.FlushAsync();
            //}

            //read the last line of the file.similar to tail
            fs.Seek(0, SeekOrigin.End);
            using var reader = new StreamReader(fs);

            //keep reading the file unless connection is disconnected
            while (!context.RequestAborted.IsCancellationRequested)
            {
                //read the last line
                var line = await reader.ReadLineAsync();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    //Send the log line to the client. create the response
                    await response.WriteAsync($"data: {line}\n\n");
                    await response.Body.FlushAsync();
                }
            }
        }

        public async Task<List<string>> GetLastNLines(int n)
        {
            var lines = new List<string>();
            try
            {
                using var fs = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(fs);

                var allLines = new List<string>();
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    allLines.Add(line);
                }
                lines = allLines.Skip(Math.Max(0, allLines.Count - n)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lines;
        }
    }
}
