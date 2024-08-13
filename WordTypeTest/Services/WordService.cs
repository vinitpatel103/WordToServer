using System;
using System.Timers;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace WordTypeTest.Services
{
    public class WordService
    {
        private Word.Application _wordApp;
        private Word.Document _wordDoc;
        private Timer _timer;

        public event Action<string> WordContentChanged;

        public void StartMonitoringWordDocument()
        {
            _timer = new Timer(2000);
            _timer.Elapsed += CheckWordContent;
            _timer.Start();
        }

        public async Task OpenWordDocumentAsync()
        {
            await Task.Run(() =>
            {
                _wordApp = new Word.Application();
                _wordApp.Visible = true;
                _wordDoc = _wordApp.Documents.Add();

                StartMonitoringWordDocument();
            });
        }

        private void CheckWordContent(object sender, ElapsedEventArgs e)
        {
            string content = _wordDoc.Content.Text;
            WordContentChanged?.Invoke(content);
        }
    }
}
