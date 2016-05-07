using FirstFloor.ModernUI.Windows.Controls;
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
using Bachat.Assets;
using Bachat.Helpers;

namespace Bachat.Dialogs
{
    /// <summary>
    /// Interaction logic for RDCustomer.xaml
    /// </summary>
    public partial class RDCustomerDialog : ModernDialog
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        private String RDAccountNo = String.Empty;
        
        public RDCustomerDialog()
        {
            InitializeComponent();
            this.Loaded += RDCustomerDialog_Loaded;
            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.OkButton.Click += OkButton_Click;
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.getRDAccountNo()))
            {
                try
                {
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Recurring deposit customer updated successfully.");
                }
                catch(Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
            else
            {
                try
                {
                    RecurringDepositCustomer newCustomer = (RecurringDepositCustomer)this.DataContext;
                    newCustomer.ClosedAccount = false;
                    var rdScheme = (from p in _db.SchemeRegisters where p.Scheme.TypeOfInvestment == "RD" && p.FromDate <= newCustomer.DateOfOpening.Value && p.ToDate >= newCustomer.DateOfOpening.Value select p).FirstOrDefault();
                    newCustomer.MaturityDate = newCustomer.DateOfOpening.Value.AddYears(rdScheme.Year).AddMonths(rdScheme.Month).AddDays(rdScheme.Day);
                    newCustomer.MaturityAmount = ((Convert.ToDouble(newCustomer.Amount) * Convert.ToDouble(rdScheme.InterestWithAmount)) / Convert.ToInt32(rdScheme.Amount)).ToString();
                    _db.RecurringDepositCustomers.InsertOnSubmit(newCustomer);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Recurring deposit customer added successfully.");
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }

        void RDCustomerDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.getRDAccountNo()))
            {
                this.DataContext = (from p in _db.RecurringDepositCustomers where p.RDAccountNo == this.getRDAccountNo() select p).FirstOrDefault();
                txtDepositerAccNo.IsReadOnly = true;
            }
            else
            {
                this.DataContext = new RecurringDepositCustomer();
            }
        }

        public void setRDAccountNo(String accountNo)
        {
            this.RDAccountNo = accountNo;
        }

        public String getRDAccountNo()
        {
            return this.RDAccountNo;
        }
    }
}
