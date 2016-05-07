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
using Bachat.Dialogs;
using Bachat.Assets;
using FirstFloor.ModernUI.Windows.Controls;
using LinqKit;

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for NKMDeposit.xaml
    /// </summary>
    public partial class NKMDeposit : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public NKMDeposit()
        {
            InitializeComponent();
            dtgDepositers.RowEditEnding += dtgDepositers_RowEditEnding;
            dtgDepositers.PreviewKeyDown += dtgDepositers_PreviewKeyDown;
            LoadDepositerData();

            cmbSearchType.DataContext = new String[] { "Account No.", "Depositer Name", "Family Code" };
            cmbSearchType.SelectionChanged += cmbSearchType_SelectionChanged;
            cmbSearchValue.SelectionChanged += cmbSearchValue_SelectionChanged;
            btnClear.Click += btnClear_Click;
        }

        void dtgDepositers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                LoadDepositerData();
            }
        }

        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbSearchType.SelectedIndex = -1;
        }

        void cmbSearchValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDepositerData();
        }

        void cmbSearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSearchType.SelectedIndex != -1)
            {
                try
                {
                    var index = cmbSearchType.SelectedIndex;
                    if (index == 0)
                    {
                        cmbSearchValue.DataContext = (from p in _db.NKMDepositers orderby Convert.ToInt32(p.DepositerNo) select p.DepositerNo).ToList();
                    }
                    else if (index == 1)
                    {
                        cmbSearchValue.DataContext = (from p in _db.NKMDepositers orderby p.DepositerName select p.DepositerName).Distinct().ToList();
                    }
                    else
                    {
                        cmbSearchValue.DataContext = (from p in _db.NKMDepositers orderby p.FamilyCode select p.FamilyCode).ToList().Distinct();
                    }
                }
                catch (Exception exception)
                {
                    ToastNotification.Toast("Error", exception.Message);
                }
                cmbSearchValue.IsEditable = true;
                cmbSearchValue.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                cmbSearchValue.SelectedIndex = -1;
                cmbSearchValue.IsEditable = false;
                cmbSearchValue.IsEnabled = false;
                btnClear.IsEnabled = false;
            }
        }

        void dtgDepositers_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateDepositersData(e.Row.DataContext as NKMDepositer)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        void UpdateDepositersData(NKMDepositer depoister)
        {
            try
            {
                var DP = (from p in _db.NKMDepositers where p.DepositerNo == depoister.DepositerNo select p).FirstOrDefault();
                DP.Address = depoister.Address;
                DP.CertificateNo = depoister.CertificateNo;
                DP.City = depoister.City;
                DP.DepositerName = depoister.DepositerName;
                DP.FamilyCode = depoister.FamilyCode;
                DP.Nomination = depoister.Nomination;
                DP.RegistrationNo = depoister.RegistrationNo;
                DP.SecondName = depoister.SecondName;

                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Deposit customer updated successfully.");
            }
            catch(Exception exception)
            {
                ToastNotification.Toast("Error", exception.Message);
            }
            LoadDepositerData();
        }

        public void LoadDepositerData()
        {
            var predicate = PredicateBuilder.True<NKMDepositer>();
            String searchValue = (cmbSearchValue.SelectedValue != null) ? cmbSearchValue.SelectedValue.ToString() : String.Empty;
            String searchType = (cmbSearchType.SelectedValue != null) ? cmbSearchType.SelectedValue.ToString() : String.Empty;
            if (!String.IsNullOrEmpty(searchValue))
            {
                if (String.Equals(searchType, "Account No."))
                {
                    predicate = predicate.And(c => c.DepositerNo.StartsWith(searchValue));
                }
                else if (String.Equals(searchType, "Depositer Name"))
                {
                    predicate = predicate.And(c => c.DepositerName.StartsWith(searchValue));
                }
                else
                {
                    predicate = predicate.And(c => c.FamilyCode.StartsWith(searchValue));
                }
            }
            dtgDepositers.DataContext = _db.NKMDepositers.Where(predicate).OrderBy(p => Convert.ToInt32(p.DepositerNo)).ToList();
        }
        private void ModernEditButton_Click(object sender, RoutedEventArgs e)
        {
            String AccountNo = (String)((Button)sender).Tag;
            openDialog("Edit", AccountNo);
        }

        private void openDialog(String type, String args)
        {
            DepositCustomerDialog depositCustomer = new DepositCustomerDialog();
            depositCustomer.Title = type + " Deposit Customer";
            if (type == "Edit")
            {
                depositCustomer.setDepositAccNo(args);
            }
            depositCustomer.ShowDialog();
            LoadDepositerData();
        }
        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            String AccountNo = (String)((Button)sender).Tag;
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var depositer = _db.NKMDepositers.Where(p => p.DepositerNo == AccountNo).FirstOrDefault();
                    var depositerTDS = _db.NKMTDSRegisters.Where(p => p.DepositerNo == AccountNo).FirstOrDefault();

                    _db.NKMDepositers.DeleteOnSubmit(depositer);
                    _db.NKMTDSRegisters.DeleteOnSubmit(depositerTDS);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Deposit customer deleted successfully");
                    LoadDepositerData();
                }
                catch(Exception exception)
                {
                    ToastNotification.Toast("Error", exception.Message);
                }
            }
        }

        private void btnAddDepositCustomer_Click(object sender, RoutedEventArgs e)
        {
            openDialog("Add", "");
        }

    }
}
