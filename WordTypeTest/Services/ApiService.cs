using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WordTypeTest.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task SendContentAsync(string content)
        {
            try
            {
                var contentToSend = new StringContent(content, System.Text.Encoding.UTF8, "text/plain");
                var response = await _httpClient.PostAsync("http://localhost:3000/receive-content", contentToSend);

                if (response.IsSuccessStatusCode)
                {
                    // Optional: Notify user of success
                }
            }
            catch (Exception ex)
            {
                // Handle exception or notify user
                Console.WriteLine($"Error sending content: {ex.Message}");
            }
        }

    }
}
