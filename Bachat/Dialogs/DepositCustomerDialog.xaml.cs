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
using Bachat.Helpers;
using Bachat.Assets;

namespace Bachat.Dialogs
{
    /// <summary>
    /// Interaction logic for DepositCustomerDialog.xaml
    /// </summary>
    public partial class DepositCustomerDialog : ModernDialog
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        private String DepositAccNo = String.Empty;

        public DepositCustomerDialog()
        {
            InitializeComponent();
            this.Loaded += DepositCustomerDialog_Loaded;
            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.OkButton.Click += OkButton_Click;
            cmbTypeOfInvestment.ItemsSource = (from p in _db.Schemes where p.TypeOfInvestment != "RD" select p.TypeOfInvestment).ToList();
            cmbPostOffice.ItemsSource = (from p in _db.PostOffices select p.PostOfficeName).ToList();
        }

        void DepositCustomerDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.getDepositeAccNo()))
            {
                this.DataContext = (from p in _db.NKMDepositers where p.DepositerNo == this.getDepositeAccNo() select p).FirstOrDefault();
                txtDepositerAccNo.IsReadOnly = true;
                cmbTypeOfInvestment.IsEnabled = false;
            }
            else
            {
                var customer = new NKMDepositer();
                customer.DateOfDeposit = DateTime.Now;
                this.DataContext = customer;                
            }
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(this.getDepositeAccNo()))
            {
                try
                {
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Recurring deposit customer updated successfully.");
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
            else
            {
                try
                {
                    NKMDepositer newCustomer = (NKMDepositer)this.DataContext;
                    var scheme = (from p in _db.Schemes where p.TypeOfInvestment == cmbTypeOfInvestment.SelectedValue.ToString() select p.SchemeNo).FirstOrDefault();
                    newCustomer.TypeOfInvestment = scheme;
                    var postOffice = (from p in _db.PostOffices where p.PostOfficeName == cmbPostOffice.SelectedValue.ToString() select p.ID).FirstOrDefault();
                    newCustomer.PostOffice = postOffice;
                    var schemeReg = (from p in _db.SchemeRegisters where (p.Scheme.TypeOfInvestment == cmbTypeOfInvestment.SelectedValue.ToString() && p.FromDate <= newCustomer.DateOfDeposit && p.ToDate >= newCustomer.DateOfDeposit) select p).FirstOrDefault();
                    newCustomer.MaturityDate = newCustomer.DateOfDeposit.AddYears(schemeReg.Year).AddMonths(schemeReg.Month).AddDays(schemeReg.Day);
                    newCustomer.MaturityAmount = (Convert.ToInt32((Convert.ToInt32(newCustomer.Amount) * Convert.ToDouble(schemeReg.InterestWithAmount)) / Convert.ToInt32(schemeReg.Amount))).ToString();
                    var agent = (from p in _db.AgentProfiles where (p.Type == "NKM") select p).FirstOrDefault();
                    NKMTDSRegister newTDSEntry = new NKMTDSRegister();
                    newTDSEntry.DepositerNo = newCustomer.DepositerNo;
                    newTDSEntry.Commission = ((agent.Commission * Convert.ToInt32(newCustomer.Amount)) / 100).ToString();
                    newTDSEntry.GrossAmount = newCustomer.Amount;
                    newTDSEntry.IssueDate = newCustomer.DateOfDeposit;
                    newTDSEntry.TDS = Convert.ToDouble((Convert.ToInt32(newTDSEntry.Commission) * agent.TDS)) / 100;
                    newTDSEntry.Sur = (newTDSEntry.TDS * agent.Sur) / 100;
                    newTDSEntry.Total = newTDSEntry.TDS + newTDSEntry.Sur;
                    newTDSEntry.NetCommission = Convert.ToDouble(newTDSEntry.Commission) - newTDSEntry.Total;
                    newTDSEntry.Type = cmbTypeOfInvestment.SelectedValue.ToString();
                    newTDSEntry.MonthYearValue = newCustomer.DateOfDeposit.Month.ToString() + newCustomer.DateOfDeposit.Year.ToString();
                    _db.NKMTDSRegisters.InsertOnSubmit(newTDSEntry);
                    _db.NKMDepositers.InsertOnSubmit(newCustomer);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Recurring deposit customer added successfully.");
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }

        public void setDepositAccNo(String accNo)
        {
            this.DepositAccNo = accNo;
        }

        public String getDepositeAccNo()
        {
            return this.DepositAccNo;
        }
    }
}
