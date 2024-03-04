using MessageTrack.PL.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MessageTrack.PL.Pages
{
    /// <summary>
    /// Логика взаимодействия для SelectRecipientsModal.xaml
    /// </summary>
    public partial class SelectRecipientsModal : Window
    {
        public SelectRecipientsViewModel SelectRecipientsViewModel;

        public SelectRecipientsModal(SelectRecipientsViewModel selectRecipientsViewModel)
        {
            InitializeComponent();
            DataContext = selectRecipientsViewModel;
            SelectRecipientsViewModel = selectRecipientsViewModel;
        }

        private void TopPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
