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
using Bachat.Dialogs;
using Bachat.Assets;

namespace Bachat.Dialogs
{
    /// <summary>
    /// Interaction logic for NewRate.xaml
    /// </summary>
    public partial class NewRate : ModernDialog
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public NewRate()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.OkButton.Click += OkButton_Click;
        }

        public NewRate(String schemeName)
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            txtScheme.Text = schemeName;
            this.DataContext = new SchemeRegister();
            this.OkButton.Click += OkButton_Click;
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtpFromDate.Text != string.Empty && dtpToDate.Text != string.Empty && txtAmount.Text != string.Empty && txtDay.Text != string.Empty && txtInterest.Text != string.Empty && txtMonth.Text != string.Empty && txtYear.Text != string.Empty)
            {
                SchemeRegister newEntry = new SchemeRegister();
                var existing = (from p in _db.Schemes where txtScheme.Text == p.TypeOfInvestment select p.SchemeNo).FirstOrDefault();
                newEntry.TypeOfInvestment = existing;
                newEntry.Amount = txtAmount.Text;
                newEntry.InterestWithAmount = txtInterest.Text;
                newEntry.FromDate = dtpFromDate.SelectedDate.Value;
                newEntry.ToDate = dtpToDate.SelectedDate.Value;
                newEntry.Year = Convert.ToInt32(txtYear.Text);
                newEntry.Month = Convert.ToInt32(txtMonth.Text);
                newEntry.Day = Convert.ToInt32(txtDay.Text);

                try
                {
                    _db.SchemeRegisters.InsertOnSubmit(newEntry);
                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Scheme Rates added successfully.");
                }
                catch (Exception execption)
                {
                    ToastNotification.Toast("Error", execption.Message);
                }
            }
        }
    }
}
