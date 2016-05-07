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
    /// Interaction logic for AddMonthlyEntry.xaml
    /// </summary>
    public partial class AddMonthlyEntry : ModernDialog
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        int individualAmount = 0;
        int balance = 0;
        DateTime nxtDueDate = new DateTime();
        public AddMonthlyEntry()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            cmbRDAccountNo.ItemsSource = (from p in _db.RecurringDepositCustomers where p.ClosedAccount == false && DateTime.Compare(p.MaturityDate.Value, DateTime.Now) > 0 select p.RDAccountNo).ToList();
            var lotNO = 0;
            if (DateTime.Now.Month > 3)
                lotNO += (from p in _db.RDRegisters where (p.Date >= DateTime.Parse("01/04/" + DateTime.Now.Year)) select p.LotNo).Distinct().Count();
            else if (DateTime.Now.Month <= 3)
                lotNO += (from p in _db.RDRegisters where (p.Date >= DateTime.Parse("01/04/" + (DateTime.Now.Year - 1)) && p.Date <= DateTime.Parse("31/03/" + DateTime.Now.Year)) select p.LotNo).Distinct().Count();
            // int count = _db.RDRegisters.Where(m => m.MonthYearValue == (DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString())).Distinct().Count();
            if (lotNO > 0)
            {
                txtLotNo.Text = Convert.ToString(lotNO + 1);
            }
            else
                txtLotNo.Text = "1";
            cmbRDAccountNo.SelectionChanged += cmbRDAccountNo_SelectionChanged;
            txtAmount.LostKeyboardFocus += txtAmount_LostKeyboardFocus;
            dtpDate.SelectedDate = DateTime.Now;
            this.OkButton.Click += OkButton_Click;
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtLotNo.Text != string.Empty && cmbRDAccountNo.SelectedIndex != -1 && txtDepositerName.Text != string.Empty && txtAmount.Text != string.Empty && txtBalance.Text != string.Empty && txtCardNumber.Text != string.Empty && dtpLastCreditDate.Text != string.Empty && dtpNextDueDate.Text != string.Empty && dtpDate.Text != string.Empty)
            {
                int amount = 0;
                var SumOfAmounts = _db.RDRegisters.Where(c => c.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) && c.LotNo == Convert.ToInt32(txtLotNo.Text));
                foreach (RDRegister reg in SumOfAmounts)
                    amount += Convert.ToInt32(reg.Amount);
                bool flag = true;
                if (amount + Convert.ToInt32(txtAmount.Text) > 20000)
                {
                    var result = ModernDialog.ShowMessage("Amount is more than 20000 rs. Do you want to continue?", "Confirmation", MessageBoxButton.YesNo, MainWindow.GetWindow(this));
                    if (result == MessageBoxResult.No)
                    {
                        flag = false;
                    }
                }

                var existingCustomer = _db.RDRegisters.Where(c => c.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()));
                foreach (RDRegister reg in existingCustomer)
                {
                    if (reg.RDAccountNo == cmbRDAccountNo.SelectedValue.ToString())
                    {
                        var result = ModernDialog.ShowMessage("Do you want same entry again in this month?", "Confirmation", MessageBoxButton.YesNo, MainWindow.GetWindow(this));
                        if (result == MessageBoxResult.No)
                        {
                            flag = false;                            
                        }
                    }
                }

                if (flag)
                {
                    try
                    {
                        RDRegister newRegister = new RDRegister();
                        newRegister.LotNo = Convert.ToInt32(txtLotNo.Text);
                        newRegister.Name = txtDepositerName.Text;
                        newRegister.RDAccountNo = cmbRDAccountNo.SelectedValue.ToString();
                        if (txtRebate.Text != string.Empty)
                            newRegister.Rebate = Convert.ToInt32(txtRebate.Text);
                        if (txtDue.Text != string.Empty)
                            newRegister.Due = Convert.ToDouble(txtDue.Text);
                        newRegister.Amount = txtAmount.Text;
                        newRegister.Balance = Convert.ToInt32(txtBalance.Text);
                        newRegister.CardNo = txtCardNumber.Text;
                        newRegister.Remarks = txtRemarks.Text;
                        newRegister.Date = dtpDate.SelectedDate.Value;
                        newRegister.MonthYearValue = newRegister.Date.Month.ToString() + newRegister.Date.Year.ToString();

                        var customer = (from p in _db.RecurringDepositCustomers where p.RDAccountNo == cmbRDAccountNo.SelectedValue.ToString() select p).FirstOrDefault();
                        customer.LastCreditDate = dtpLastCreditDate.SelectedDate.Value;
                        customer.Balance = txtBalance.Text;
                        customer.NextDueDate = dtpNextDueDate.SelectedDate.Value;

                        if (Convert.ToInt32(txtAmount.Text) > individualAmount)
                        {
                            var existingEntry = (from p in _db.RebetRDEntryDues where (p.RDAccountNo == customer.RDAccountNo) select p).FirstOrDefault();
                            if (existingEntry == null)
                            {
                                RebetRDEntryDue newRebetRDEntryDue = new RebetRDEntryDue();
                                newRebetRDEntryDue.RDAccountNo = customer.RDAccountNo;
                                newRebetRDEntryDue.Amount = customer.Amount;
                                newRebetRDEntryDue.Balance = customer.Balance;
                                newRebetRDEntryDue.DepositerName = customer.DepositerName;
                                newRebetRDEntryDue.NextDueDate = customer.NextDueDate.Value;
                                _db.RebetRDEntryDues.InsertOnSubmit(newRebetRDEntryDue);
                            }
                            else
                            {
                                existingEntry.Balance = customer.Balance;
                                existingEntry.NextDueDate = customer.NextDueDate.Value;
                            }
                        }

                        _db.RDRegisters.InsertOnSubmit(newRegister);
                        _db.SubmitChanges();
                    }
                    catch(Exception exception)
                    {
                        ToastNotification.Toast("Error", exception.Message);
                    }

                }
            }
        }

        void txtAmount_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (cmbRDAccountNo.SelectedIndex != -1)
            {
                if (Convert.ToInt32(txtAmount.Text) >= individualAmount)
                {
                    txtBalance.Text = ((balance - individualAmount) + Convert.ToInt32(txtAmount.Text)).ToString();
                    dtpNextDueDate.SelectedDate = nxtDueDate.AddMonths(Convert.ToInt32(txtAmount.Text) / individualAmount);
                    if (Convert.ToInt32(txtAmount.Text) / individualAmount == 12)
                    {
                        txtRebate.Text = (individualAmount * 40 / 100).ToString();
                    }
                    else if (Convert.ToInt32(txtAmount.Text) / individualAmount >= 6 && Convert.ToInt32(txtAmount.Text) / individualAmount < 12)
                    {
                        txtRebate.Text = (individualAmount * 10 / 100).ToString();
                    }
                }
            }
        }

        void cmbRDAccountNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var RDCustomer = (from p in _db.RecurringDepositCustomers where p.RDAccountNo == cmbRDAccountNo.SelectedValue.ToString() select p).FirstOrDefault();
            this.DataContext = RDCustomer;
            txtDue.Text = "0";
            txtRebate.Text = "0";
            txtBalance.Text = (Convert.ToInt32(RDCustomer.Balance) + Convert.ToInt32(RDCustomer.Amount)).ToString();
            dtpLastCreditDate.SelectedDate = DateTime.Now.Date;
            if (RDCustomer.NextDueDate != null)
            {
                dtpNextDueDate.SelectedDate = RDCustomer.NextDueDate.Value.AddMonths(1);
                if (dtpNextDueDate.SelectedDate.Value.Month == dtpLastCreditDate.SelectedDate.Value.Month)
                    dtpNextDueDate.SelectedDate = dtpNextDueDate.SelectedDate.Value.AddMonths(1);
            }
            else
                dtpNextDueDate.SelectedDate = DateTime.Now.Date.AddMonths(1);
            individualAmount = Convert.ToInt32(RDCustomer.Amount);
            balance = Convert.ToInt32(txtBalance.Text);
            if (RDCustomer.NextDueDate != null)
                nxtDueDate = RDCustomer.NextDueDate.Value;
            else
                nxtDueDate = DateTime.Now.Date;

            if (RDCustomer.NextDueDate != null)
            {
                if (RDCustomer.NextDueDate.Value.Year < DateTime.Now.Year)
                {
                    int months = ((DateTime.Now.Year - RDCustomer.NextDueDate.Value.Year) * 12) + DateTime.Now.Month - RDCustomer.NextDueDate.Value.Month + 1;
                    int due = (individualAmount / 100) * 2;
                    int dueAmount = 0;
                    for (int i = months; i > 1; i--)
                        dueAmount += (i - 1) * due;
                    txtDue.Text = dueAmount.ToString();
                    txtAmount.Text = (individualAmount * months).ToString();
                    txtBalance.Text = (Convert.ToInt32(RDCustomer.Balance) + (individualAmount * months)).ToString();
                    dtpNextDueDate.SelectedDate = DateTime.Now.AddMonths(1);
                }
                else if (RDCustomer.NextDueDate.Value.Month < DateTime.Now.Month)
                {
                    int months = DateTime.Now.Month - RDCustomer.NextDueDate.Value.Month + 1;
                    int due = (individualAmount / 100) * 2;
                    int dueAmount = 0;
                    for (int i = months; i > 1; i--)
                        dueAmount += (i - 1) * due;
                    txtDue.Text = dueAmount.ToString();
                    txtAmount.Text = (individualAmount * months).ToString();
                    txtBalance.Text = (Convert.ToInt32(RDCustomer.Balance) + (individualAmount * months)).ToString();
                    dtpNextDueDate.SelectedDate = DateTime.Now.AddMonths(1);
                }
            }
        }
    }
}
