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
    /// Interaction logic for Schemes.xaml
    /// </summary>
    public partial class Schemes : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public Schemes()
        {
            InitializeComponent();
            dtgSchemes.PreviewKeyDown += dtgSchemes_PreviewKeyDown;
            dtgSchemes.RowEditEnding += dtgSchemes_RowEditEnding;
            LoadSchemesData();
        }

        void dtgSchemes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateSchemes(e.Row.DataContext as Scheme)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        void UpdateSchemes(Scheme scheme)
        {
            try
            {
                var SC = (from p in _db.Schemes where p.SchemeNo == scheme.SchemeNo select p).FirstOrDefault();
                if (SC != null)
                {
                    SC.TypeOfInvestment = scheme.TypeOfInvestment;
                }
                else
                {
                    _db.Schemes.InsertOnSubmit(scheme);
                }
                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Scheme updated successfully.");
            }
            catch (Exception exception)
            {
                ToastNotification.Toast("Error", exception.Message);
            }
            LoadSchemesData();
        }

        void dtgSchemes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                LoadSchemesData();
            } 
        }

        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Int32 SchemeNo = (Int32)((Button)sender).Tag;
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var scheme = _db.Schemes.Where(p => p.SchemeNo == SchemeNo).FirstOrDefault();
                    _db.Schemes.DeleteOnSubmit(scheme);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Post Office deleted successfully.");
                    LoadSchemesData();
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }

        void LoadSchemesData()
        {
            dtgSchemes.DataContext = (from p in _db.Schemes select p).ToList();
        }
    }
}
