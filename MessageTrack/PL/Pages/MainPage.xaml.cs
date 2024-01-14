using MessageTrack.PL.ViewModels;
using System.Windows.Controls;

namespace MessageTrack.PL.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            DataContext = mainPageViewModel;
        }
    }
}
