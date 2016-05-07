using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using Bachat.Dialogs;
using Bachat.Assets;
using Bachat.Helpers;

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for PostOffices.xaml
    /// </summary>
    public partial class PostOffices : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public PostOffices()
        {
            InitializeComponent();
            loadPostOffices();
            dtgPostOffices.PreviewKeyDown += dtgPostOffices_PreviewKeyDown;
            dtgPostOffices.RowEditEnding += dtgPostOffices_RowEditEnding;
            btnAddPostOffice.Click += btnAddPostOffice_Click;
        }

        void dtgPostOffices_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => UpdatePostOfficeData(e.Row.DataContext as PostOffice)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        void UpdatePostOfficeData(PostOffice postOffice)
        {
            try
            {
                var PO = (from p in _db.PostOffices where p.ID == postOffice.ID select p).FirstOrDefault();
                if (PO != null)
                {
                    PO.PostOfficeName = postOffice.PostOfficeName;
                }
                else
                {
                    _db.PostOffices.InsertOnSubmit(postOffice);
                }
                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Post Office updated successfully.");
            }
            catch (Exception exception)
            {
                ToastNotification.Toast("Error", exception.Message);
            }
            loadPostOffices();
        }

        void btnAddPostOffice_Click(object sender, RoutedEventArgs e)
        {
            AddPostOffice addPostOfficeDialog = new AddPostOffice();
            addPostOfficeDialog.ShowDialog();
            loadPostOffices();
        }

        void dtgPostOffices_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                loadPostOffices();
            }
        }

        void loadPostOffices()
        {
            dtgPostOffices.DataContext = (from p in _db.PostOffices select p).ToList();
        }

        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Int32 ID = (Int32)((Button)sender).Tag;
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var postOffice = _db.PostOffices.Where(p => p.ID == ID).FirstOrDefault();
                    _db.PostOffices.DeleteOnSubmit(postOffice);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Post Office deleted successfully.");
                    loadPostOffices();
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }
    }
}
