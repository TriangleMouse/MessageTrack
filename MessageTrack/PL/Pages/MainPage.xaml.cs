using MessageTrack.PL.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MessageTrack.PL.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly MainPageViewModel _mainPageViewModel;

        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            _mainPageViewModel = mainPageViewModel;
            DataContext = mainPageViewModel;
            Loaded += LoadForm;
        }

        private async void LoadForm(object sender, RoutedEventArgs e)
        {
            await _mainPageViewModel.LoadData();
        }

    }
}
