using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Word = Microsoft.Office.Interop.Word;

namespace WordTypeTest.ViewModels
{
    public class MainViewModel : ViewModelBase // Ensure you have ViewModelBase defined or replace it with an alternative
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private Word.Application _wordApp;
        private Word.Document _wordDoc;
        private Timer _timer;
        private string _lastContent = string.Empty; // Field to store the last sent content

        public MainViewModel()
        {
            _timer = new Timer(5000); // Check every 5 seconds
            _timer.Elapsed += CheckWordContent;
        }

        public async Task OpenWordDocumentAsync()
        {
            await Task.Run(() =>
            {
                _wordApp = new Word.Application();
                _wordApp.Visible = true;
                _wordDoc = _wordApp.Documents.Add();

                _timer.Start(); // Start monitoring changes
            });
        }

        private void CheckWordContent(object sender, ElapsedEventArgs e)
        {
            const int maxRetries = 3;
            const int retryDelayMs = 1000; // 1 second

            string currentContent = string.Empty;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    currentContent = _wordDoc.Content.Text;
                    break; // Exit loop if successful
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    if (attempt < maxRetries - 1)
                    {
                        System.Threading.Thread.Sleep(retryDelayMs); // Wait before retrying
                    }
                    else
                    {
                        // Handle the error or log it
                        Console.WriteLine("Failed to retrieve content from Word document.");
                        return;
                    }
                }
            }

            // Send content only if it has changed
            if (currentContent != _lastContent)
            {
                _lastContent = currentContent;
                _ = SendContentAsync(currentContent);
            }
        }

        public async Task SendContentAsync(string content)
        {
            try
            {
                var contentToSend = new StringContent(content, Encoding.UTF8, "text/plain");
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
