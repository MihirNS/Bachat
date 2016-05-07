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
    /// Interaction logic for AddPostOffice.xaml
    /// </summary>
    public partial class AddPostOffice : ModernDialog
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public AddPostOffice()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };

            this.OkButton.Click += OkButton_Click;
            this.DataContext = new PostOffice();
        }

        void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PostOffice newPostOffice = (PostOffice)this.DataContext;
                _db.PostOffices.InsertOnSubmit(newPostOffice);
                _db.SubmitChanges();
                ToastNotification.Toast("Success", "PostOffice added successfully.");
            }
            catch (Exception execption)
            {
                ToastNotification.Toast("Error", execption.Message);
            }
        }
    }
}
