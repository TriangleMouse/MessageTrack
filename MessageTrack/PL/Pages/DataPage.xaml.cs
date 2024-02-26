using MessageTrack.PL.ViewModels;
using System.Windows.Controls;

namespace MessageTrack.PL.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        public DataPage(DataViewModel dataViewModel)
        {
            InitializeComponent();
            DataContext = dataViewModel;
        }
    }
}
