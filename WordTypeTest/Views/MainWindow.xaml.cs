using System.Windows;
using WordTypeTest.ViewModels;

namespace WordTypeTest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainViewModel();
            this.DataContext = viewModel;
            _ = viewModel.OpenWordDocumentAsync(); // Call the method to open the Word document
        }
    }
}
