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

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for AgentFile.xaml
    /// </summary>
    public partial class AgentFile : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public AgentFile()
        {
            InitializeComponent();
            btnSaveProfile.Click += btnSaveProfile_Click;
            loadData();
        }

        void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            var agentFile1 = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();
            var agentFile2 = (from p in _db.AgentProfiles where p.Type == "NKM" select p).FirstOrDefault();

            try
            {
                if (agentFile1 != null)
                {
                    agentFile1.Address = txtAgentAddress.Text;
                    agentFile1.AgentName = txtRDAgentName.Text;
                    agentFile1.AuthorityNo = txtAuthoNo.Text;
                    agentFile1.Commission = Convert.ToDouble(txtRDCommission.Text);
                    agentFile1.IssueDate = dtpRDIssueDate.SelectedDate.Value;
                    agentFile1.ValidUpto = dtpRDValidUpTo.SelectedDate.Value;
                    agentFile1.Sur = Convert.ToDouble(txtRDSur.Text);
                    agentFile1.TDS = Convert.ToDouble(txtRDTDS.Text);
                }
                else
                {
                    AgentProfile newEntry = new AgentProfile();
                    newEntry.Address = txtAgentAddress.Text;
                    newEntry.AgentName = txtRDAgentName.Text;
                    newEntry.AuthorityNo = txtAuthoNo.Text;
                    newEntry.Commission = Convert.ToDouble(txtRDCommission.Text);
                    newEntry.IssueDate = dtpRDIssueDate.SelectedDate.Value;
                    newEntry.ValidUpto = dtpRDValidUpTo.SelectedDate.Value;
                    newEntry.Sur = Convert.ToDouble(txtRDSur.Text);
                    newEntry.TDS = Convert.ToDouble(txtRDTDS.Text);
                    newEntry.Type = "RD";
                    _db.AgentProfiles.InsertOnSubmit(newEntry);
                }

                if (agentFile2 != null)
                {
                    agentFile2.Address = txtNKMAgentAddress.Text;
                    agentFile2.AgentName = txtNKMAgentName.Text;
                    agentFile2.AuthorityNo = txtNKMAuthoNo.Text;
                    agentFile2.Commission = Convert.ToDouble(txtNKMCommission.Text);
                    agentFile2.IssueDate = dtpNKMIssueDate.SelectedDate.Value;
                    agentFile2.ValidUpto = dtpNKMValidUpTo.SelectedDate.Value;
                    agentFile2.Sur = Convert.ToDouble(txtNKMSur.Text);
                    agentFile2.TDS = Convert.ToDouble(txtNKMTDS.Text);
                }
                else
                {
                    AgentProfile newEntry = new AgentProfile();
                    newEntry.Address = txtNKMAgentAddress.Text;
                    newEntry.AgentName = txtNKMAgentName.Text;
                    newEntry.AuthorityNo = txtNKMAuthoNo.Text;
                    newEntry.Commission = Convert.ToDouble(txtNKMCommission.Text);
                    newEntry.IssueDate = dtpNKMIssueDate.SelectedDate.Value;
                    newEntry.ValidUpto = dtpNKMValidUpTo.SelectedDate.Value;
                    newEntry.Sur = Convert.ToDouble(txtNKMSur.Text);
                    newEntry.TDS = Convert.ToDouble(txtNKMTDS.Text);
                    newEntry.Type = "NKM";
                    _db.AgentProfiles.InsertOnSubmit(newEntry);
                }
                _db.SubmitChanges();
                ToastNotification.Toast("Success", "Agent profile save successfully.");
            }
            catch(Exception exception)
            {
                ToastNotification.Toast("Error", exception.Message);
            }
        }

        void loadData()
        {
            var AgentFile1 = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();
            var AgentFile2 = (from p in _db.AgentProfiles where p.Type == "NKM" select p).FirstOrDefault();
            if (AgentFile1 != null)
            {
                txtRDAgentName.Text = AgentFile1.AgentName;
                txtAuthoNo.Text = AgentFile1.AuthorityNo;
                txtRDTDS.Text = AgentFile1.TDS.ToString();
                txtRDSur.Text = AgentFile1.Sur.ToString();
                txtRDCommission.Text = AgentFile1.Commission.ToString();
                txtAgentAddress.Text = AgentFile1.Address;
                dtpRDIssueDate.SelectedDate = AgentFile1.IssueDate;
                dtpRDValidUpTo.SelectedDate = AgentFile1.ValidUpto;
            }
            if (AgentFile2 != null)
            {
                txtNKMAgentName.Text = AgentFile2.AgentName;
                txtNKMAuthoNo.Text = AgentFile2.AuthorityNo;
                txtNKMTDS.Text = AgentFile2.TDS.ToString();
                txtNKMSur.Text = AgentFile2.Sur.ToString();
                txtNKMCommission.Text = AgentFile2.Commission.ToString();
                txtNKMAgentAddress.Text = AgentFile2.Address;
                dtpNKMIssueDate.SelectedDate = AgentFile1.IssueDate;
                dtpNKMValidUpTo.SelectedDate = AgentFile2.ValidUpto;
            }
        }
    }
}
