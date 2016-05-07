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
using Bachat.Helpers;
using FirstFloor.ModernUI.Windows.Controls;
using LinqKit;

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for RecurringDeposit.xaml
    /// </summary>
    public partial class RecurringDeposit : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);

        public RecurringDeposit()
        {
            InitializeComponent();
            dtgRDCustomer.RowEditEnding += dtgRDCustomer_RowEditEnding;
            dtgRDCustomer.PreviewKeyDown += dtgRDCustomer_PreviewKeyDown;            

            LoadCustomerData();

            cmbSearchType.DataContext = new String[] { "Account No.", "Depositer Name", "Family Code" };
            cmbSearchType.SelectionChanged += cmbSearchType_SelectionChanged;
            cmbSearchValue.SelectionChanged += cmbSearchValue_SelectionChanged;
            btnClear.Click += btnClear_Click;
        }

        void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbSearchType.SelectedIndex = -1;
        }

        void cmbSearchValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCustomerData();
        }

        void cmbSearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSearchType.SelectedIndex != -1)
            {
                try { 
                    var index = cmbSearchType.SelectedIndex;
                    if (index == 0)
                    {
                        cmbSearchValue.DataContext = (from p in _db.RecurringDepositCustomers orderby p.RDAccountNo select p.RDAccountNo).ToList();
                    }
                    else if (index == 1)
                    {
                        cmbSearchValue.DataContext = (from p in _db.RecurringDepositCustomers orderby p.DepositerName select p.DepositerName).Distinct().ToList();
                    }
                    else
                    {
                        cmbSearchValue.DataContext = (from p in _db.RecurringDepositCustomers orderby p.FamilyCode select p.FamilyCode).Distinct().ToList();
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

        void dtgRDCustomer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                LoadCustomerData();
            }
        }

        void dtgRDCustomer_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Dispatcher.BeginInvoke(new Action(() => UpdateRDCustomerData(e.Row.DataContext as RecurringDepositCustomer)), System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        public void LoadCustomerData()
        {
            var predicate = PredicateBuilder.True<RecurringDepositCustomer>();
            String searchValue = (cmbSearchValue.SelectedValue != null) ? cmbSearchValue.SelectedValue.ToString() : String.Empty;
            String searchType = (cmbSearchType.SelectedValue != null) ? cmbSearchType.SelectedValue.ToString() : String.Empty;
            if (!String.IsNullOrEmpty(searchValue))
            {
                if (String.Equals(searchType, "Account No."))
                {
                    predicate = predicate.And(c => c.RDAccountNo.StartsWith(searchValue));
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
            dtgRDCustomer.DataContext = _db.RecurringDepositCustomers.Where(predicate).OrderBy(p => p.RDAccountNo).ToList();
        }

        void UpdateRDCustomerData(RecurringDepositCustomer rdCustomer)
        {
            try
            {
                var RD = (from p in _db.RecurringDepositCustomers where p.RDAccountNo == rdCustomer.RDAccountNo select p).FirstOrDefault();
                RD.Address = rdCustomer.Address;
                RD.CardNo = rdCustomer.CardNo;
                RD.City = rdCustomer.City;
                RD.ClosedAccount = rdCustomer.ClosedAccount;
                if (rdCustomer.ClosedAccount == true)
                {
                    if (rdCustomer.ClosingDate.HasValue)
                    {
                        RD.ClosingDate = rdCustomer.ClosingDate.Value;
                    }
                    else
                        RD.ClosingDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                }
                else
                    RD.ClosingDate = null;

                RD.DateOfBirth = rdCustomer.DateOfBirth;
                RD.DepositerName = rdCustomer.DepositerName;
                RD.FamilyCode = rdCustomer.FamilyCode;
                RD.NameOfNominee = rdCustomer.NameOfNominee;
                RD.PhoneNo = rdCustomer.PhoneNo;
                RD.SecondName = rdCustomer.SecondName;
                RD.NextDueDate = rdCustomer.NextDueDate;
                RD.Balance = rdCustomer.Balance;
                RD.LastCreditDate = rdCustomer.LastCreditDate;

                var rdRegister = from p in _db.RDRegisters where p.RDAccountNo == rdCustomer.RDAccountNo select p;
                foreach (RDRegister reg in rdRegister)
                {
                    reg.Name = rdCustomer.DepositerName;
                    reg.CardNo = rdCustomer.CardNo;
                }

                var dueRebate = (from p in _db.RebetRDEntryDues where p.RDAccountNo == rdCustomer.RDAccountNo select p).FirstOrDefault();
                if (dueRebate != null)
                {
                    dueRebate.DepositerName = rdCustomer.DepositerName;
                    dueRebate.NextDueDate = rdCustomer.NextDueDate.Value;
                    dueRebate.Balance = rdCustomer.Balance;
                }

                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Recurring deposit customer updated successfully.");
            }
            catch (Exception execption)
            {
                ToastNotification.Toast("Error", execption.Message);
            }
        }

        private void btnAddRdCustomer_Click(object sender, RoutedEventArgs e)
        {
            openDialog("Add", "");
        }

        private void ModernEditButton_Click(object sender, RoutedEventArgs e)
        {
            String AccountNo = (String)((Button)sender).Tag;
            openDialog("Edit", AccountNo);
        }

        private void openDialog(String type, String args)
        {
            RDCustomerDialog rdCustomer = new RDCustomerDialog();
            rdCustomer.Title = type + " RD Customer";
            if (type == "Edit")
            {
                rdCustomer.setRDAccountNo(args);
            }
            rdCustomer.ShowDialog();
            LoadCustomerData();
        }

        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            String AccountNo = (String)((Button)sender).Tag;
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    var customer = _db.RecurringDepositCustomers.Where(p => p.RDAccountNo == AccountNo).FirstOrDefault();
                    _db.RecurringDepositCustomers.DeleteOnSubmit(customer);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Recurring deposit customer deleted successfully.");
                    LoadCustomerData();
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }
    }
}
