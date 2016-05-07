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
    /// Interaction logic for UpdateSchemeRates.xaml
    /// </summary>
    public partial class UpdateSchemeRates : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public UpdateSchemeRates()
        {
            InitializeComponent();
            this.Loaded += UpdateSchemeRates_Loaded;
            cmbSchemes.SelectionChanged += cmbSchemes_SelectionChanged;
            dtgUpdateRates.PreviewKeyDown += dtgUpdateRates_PreviewKeyDown;
            dtgUpdateRates.RowEditEnding += dtgUpdateRates_RowEditEnding;
            btnNewRate.Click += btnNewRate_Click;
        }

        void UpdateSchemeRates_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSchemes.DataContext = (from p in _db.Schemes select p.TypeOfInvestment).ToList();
        }

        void btnNewRate_Click(object sender, RoutedEventArgs e)
        {
            NewRate newRate = new NewRate(cmbSchemes.SelectedValue.ToString());
            newRate.ShowDialog();
            fillGrid();
        }

        void dtgUpdateRates_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateScheme(e.Row.DataContext as SchemeRegister)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        void UpdateScheme(SchemeRegister schemesRegister)
        {
            var scheme = (from p in _db.SchemeRegisters where p.ID == schemesRegister.ID select p).FirstOrDefault();
            scheme.Amount = schemesRegister.Amount;
            scheme.Day = schemesRegister.Day;
            scheme.FromDate = schemesRegister.FromDate;
            scheme.InterestWithAmount = schemesRegister.InterestWithAmount;
            scheme.Month = schemesRegister.Month;
            scheme.ToDate = schemesRegister.ToDate;
            scheme.Year = schemesRegister.Year;

            try
            {
                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Scheme updates successfully.");
            }
            catch (Exception execption)
            {
                ToastNotification.Toast("Error", execption.Message);
            }
            fillGrid();
        }

        void dtgUpdateRates_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                fillGrid();
            }
        }

        void cmbSchemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnNewRate.IsEnabled = true;
            fillGrid();
        }

        void fillGrid()
        {
            dtgUpdateRates.DataContext = (from p in _db.SchemeRegisters where p.Scheme.TypeOfInvestment == cmbSchemes.SelectedValue.ToString() select p).ToList();
        }

        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Int64 ID = (Int64)((Button)sender).Tag;
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));

            if (result == MessageBoxResult.OK)
            {
                try
                {
                    SchemeRegister existingScheme = (SchemeRegister)dtgUpdateRates.SelectedItem;
                    var scheme = _db.SchemeRegisters.Where(p => p.ID == existingScheme.ID).FirstOrDefault();
                    _db.SchemeRegisters.DeleteOnSubmit(scheme);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Scheme Rate deleted successfully.");
                    fillGrid();
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }
    }
}
