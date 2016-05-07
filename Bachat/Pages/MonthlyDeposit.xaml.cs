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
using FirstFloor.ModernUI.Windows.Controls;
using Bachat.Helpers;
using Bachat.Dialogs;

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for MonthlyDeposit.xaml
    /// </summary>
    public partial class MonthlyDeposit : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public MonthlyDeposit()
        {
            InitializeComponent();
            dtpDate.SelectedDateChanged += dtpDate_SelectedDateChanged;
            cmbLotNo.SelectionChanged += cmbLotNo_SelectionChanged;
            chkScreen.Click += chkScreen_Click;
            btnPrint.Click += btnPrint_Click;
            btnAddMonthlyEntry.Click += btnAddMonthlyEntry_Click;
        }

        void btnAddMonthlyEntry_Click(object sender, RoutedEventArgs e)
        {
            AddMonthlyEntry newMonthlyEntry = new AddMonthlyEntry();
            newMonthlyEntry.ShowDialog();
            cmbLotNo_SelectionChanged(sender, null);
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var reg = (from p in _db.RDTDSRegisters where (p.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) && p.LotNo == Convert.ToInt32(cmbLotNo.SelectedValue)) select p).ToList();

            var entry = (from p in _db.RDRegisters where p.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) && p.LotNo == Convert.ToInt32(cmbLotNo.SelectedValue) select p).ToList();

            int totalAmount = 0;
            for (int i = 0; i < entry.Count; i++)
            {
                totalAmount += Convert.ToInt32(entry[i].Amount);
            }

            var agent = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();

            double commision = totalAmount * (agent.Commission / 100);

            int TDS = Convert.ToInt32((agent.TDS * commision) / 100);

            int sur = Convert.ToInt32((agent.Sur * TDS) / 100);

            double netcommission = commision - TDS - sur;

            if (reg.Count == 0)
            {
                try
                {
                    RDTDSRegister tdsreg = new RDTDSRegister();
                    tdsreg.Commission = commision.ToString();
                    tdsreg.Date = entry[0].Date;
                    tdsreg.GrossAMT = totalAmount.ToString();
                    tdsreg.LotNo = Convert.ToInt32(cmbLotNo.SelectedValue);
                    tdsreg.MonthYearValue = entry[0].MonthYearValue;
                    tdsreg.NetCommission = netcommission;
                    tdsreg.Sur = sur;
                    tdsreg.TDS = TDS;
                    tdsreg.Total = TDS + sur;
                    tdsreg.Type = "RD";

                    _db.RDTDSRegisters.InsertOnSubmit(tdsreg);
                    _db.SubmitChanges();
                }
                catch
                {
                    ToastNotification.Toast("Error", "Can't Update TDS Register");
                }
            }
            else
            {
                try
                {
                    var tdsreg = (from p in _db.RDTDSRegisters where (p.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) && p.LotNo == Convert.ToInt32(cmbLotNo.SelectedValue)) select p).FirstOrDefault();
                    tdsreg.Commission = commision.ToString();
                    tdsreg.Date = entry[0].Date;
                    tdsreg.GrossAMT = totalAmount.ToString();
                    tdsreg.LotNo = Convert.ToInt32(cmbLotNo.SelectedValue);
                    tdsreg.MonthYearValue = entry[0].MonthYearValue;
                    tdsreg.NetCommission = netcommission;
                    tdsreg.Sur = sur;
                    tdsreg.TDS = TDS;
                    tdsreg.Total = TDS + sur;
                    tdsreg.Type = "RD";

                    _db.SubmitChanges();
                }
                catch
                {
                    ToastNotification.Toast("Error", "Can't Update TDS Register");
                }
            }

            flwScrollViewRDList.Print();
        }

        void chkScreen_Click(object sender, RoutedEventArgs e)
        {
            if (chkScreen.IsChecked.Value)
            {
                dtgRDRegister.Visibility = Visibility.Collapsed;
                flwScrollViewRDList.Visibility = Visibility.Visible;
                btnPrint.IsEnabled = true;
                btnPrint.Foreground = Brushes.Black;
            }
            else
            {
                flwScrollViewRDList.Visibility = Visibility.Collapsed;
                dtgRDRegister.Visibility = Visibility.Visible;
                btnPrint.IsEnabled = false;
                btnPrint.Foreground = Brushes.Gray;
            }
        }

        void cmbLotNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLotNo.SelectedIndex > -1)
            {
                try
                {
                    chkScreen.IsEnabled = true;
                    var customerRegister = (from p in _db.RDRegisters where p.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) && p.LotNo == Convert.ToInt32(cmbLotNo.SelectedValue) orderby p.RDAccountNo select p).ToList();
                    dtgRDRegister.ItemsSource = customerRegister;

                    if (dtgRDRegister.IsVisible)
                        chkScreen.IsEnabled = true;
                    int totalAmount = 0;
                    int totalDue = 0;
                    int totalRebate = 0;
                    if (customerRegister.Count > 0)
                    {
                        for (int i = 0; i < customerRegister.Count; i++)
                        {
                            totalAmount += Convert.ToInt32(customerRegister[i].Amount);
                            if (customerRegister[i].Due != null)
                                totalDue += Convert.ToInt32(customerRegister[i].Due);
                            if (customerRegister[i].Rebate != null)
                                totalRebate += Convert.ToInt32(customerRegister[i].Rebate);
                        }
                        Double commision = totalAmount * 0.04;

                        var agent = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();

                        int TDS = Convert.ToInt32((agent.TDS * commision) / 100);

                        Table rdTable = new Table();
                        rdTable.RowGroups.Add(new TableRowGroup());
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        TableRow currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        /*    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\n\t\tSCHEDULE FOR DEPOSIT IN PO RD ACCOUNT\t\t\t\t\n" +
                                               "\t\tAgent Name:- " + agent.AgentName + "\t\t\t(1) Amt. of Gross Depo.\tRs. " + totalAmount + "\n" +
                                               "\t\tM.P.K.B.Y.AUTHO.NO." + agent.AuthorityNo + "\t\t\t\t\t(2) Amt. of Commi.Recd.\tRs. " + commision + "\n" +
                                               "\t\tISSUE DT. " + agent.IssueDate.ToShortDateString() + "\t\t\t\t\t\t(3) Net Amount\t\tRs. " + (totalAmount - commision) + "\n" +
                                               "\t\tVALID UPTO DT. " + agent.ValidUpto.ToShortDateString() + "\t\t\t\t\t(4) Due\t\t\tRs. " + totalDue + "\n\t\t" +
                                               agent.Address + "\t\t\t\t(5) Rebate\t\tRs. " + totalRebate + "\n" +
                                               "\t\t\t\t\t\t\t\t\t\t(6) T.D.S\t\t\tRs. " + TDS + "\n" +
                                               "\t\t\t\t\t\t\t\t\t\t(7) Net Amt. to be Tend.\tRs. " + (totalAmount - commision + totalDue - totalRebate + TDS) + "\n" +
                                               "\n" +
                                               "\t\tDate : " + customerRegister[0].Date.ToShortDateString() + "\t\t\tLot No : " + customerRegister[0].LotNo + "\t\tSingature of Agent : _____________________________"))));
                            currentRow.Cells[0].ColumnSpan = 9;*/
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\n"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\nSCHEDULE FOR DEPOSIT IN PO RD ACCOUNT"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\n(1) Amt. of Gross Depo.\tRs. " + totalAmount))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Agent Name:- " + agent.AgentName))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(2) Amt. of Commi.Recd.\tRs. " + commision))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("M.P.K.B.Y.AUTHO.NO." + agent.AuthorityNo))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(3) Net Amount\t\tRs. " + (totalAmount - commision)))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("ISSUE DT. " + agent.IssueDate.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(4) Due\t\t\tRs. " + totalDue))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("VALID UPTO DT. " + agent.ValidUpto.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(5) Rebate\t\tRs. " + totalRebate))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(agent.Address))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(6) T.D.S\t\t\tRs. " + TDS))));
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[2].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("(7) Net Amt. to be Tend.\tRs. " + (totalAmount - commision + totalDue - totalRebate + TDS)))));
                        currentRow.Cells[0].ColumnSpan = 5;
                        currentRow.Cells[1].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\nDate : " + customerRegister[0].Date.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\nLot No : " + customerRegister[0].LotNo))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\nSingature of Agent : _______________________________"))));
                        currentRow.Cells[1].ColumnSpan = 2;
                        currentRow.Cells[2].ColumnSpan = 2;
                        currentRow.Cells[3].ColumnSpan = 4;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells[1].ColumnSpan = 8;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        int numberOfColumns = 9;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sr No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Name Of Depositer"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("A/C No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Due/Reb."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Balance"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Card No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Remark"))));
                        GridLength clmWidth = new GridLength(90, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(35, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(180, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;
                        clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[8].Width = clmWidth;

                        currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Left;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells[1].ColumnSpan = 8;

                        for (int i = 0; i < customerRegister.Count; i++)
                        {
                            int m = i + 1;
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Name))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].RDAccountNo))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Amount))));
                            if (customerRegister[i].Due != null && customerRegister[i].Due != 0.0)
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Due.ToString()))));
                            else if (customerRegister[i].Rebate != null && customerRegister[i].Rebate != 0.0)
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Rebate.ToString()))));
                            else
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Balance.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].CardNo))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(customerRegister[i].Remarks))));

                            currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[8].TextAlignment = TextAlignment.Left;
                        }


                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells[1].ColumnSpan = 8;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalAmount.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;

                        #region Code for Converting Amount In Words

                        string amountInWords = "";

                        bool flag = true;
                        for (int count = 0; count < totalAmount.ToString().Count(); count++)
                        {
                            if (Convert.ToInt32(totalAmount.ToString()[count].ToString()) != 0)
                            {
                                if (totalAmount.ToString().Count() == 6)
                                {
                                    if (count == 0)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " LAKH ";
                                    }
                                    if (count == 1)
                                    {
                                        if (Convert.ToInt32(totalAmount.ToString()[count].ToString() + totalAmount.ToString()[count + 1].ToString()) > 19)
                                        {
                                            amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                            if (totalAmount.ToString()[count + 1].ToString() == "0")
                                            {
                                                amountInWords += "THOUSAND ";
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                            amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString() + totalAmount.ToString()[count + 1].ToString())) + " THOUSAND ";
                                        }
                                    }
                                    if (count == 2 && flag)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " THOUSAND ";
                                    }
                                    if (count == 3)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " HUNDRED ";
                                    }
                                    if (count == 4)
                                    {
                                        amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                    if (count == 5)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                }
                                else if (totalAmount.ToString().Count() == 5)
                                {
                                    if (count == 0)
                                    {
                                        if (Convert.ToInt32(totalAmount.ToString()[count].ToString() + totalAmount.ToString()[count + 1].ToString()) > 19)
                                        {
                                            amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                            if (totalAmount.ToString()[count + 1].ToString() == "0")
                                            {
                                                amountInWords += "THOUSAND ";
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                            amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString() + totalAmount.ToString()[count + 1].ToString())) + " THOUSAND ";
                                        }
                                    }
                                    if (count == 1 && flag)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " THOUSAND ";
                                    }
                                    if (count == 2)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " HUNDRED ";
                                    }
                                    if (count == 3)
                                    {
                                        amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                    if (count == 4)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                }
                                else if (totalAmount.ToString().Count() == 4)
                                {
                                    if (count == 0)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " THOUSAND ";
                                    }
                                    if (count == 1)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " HUNDRED ";
                                    }
                                    if (count == 2)
                                    {
                                        amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                    if (count == 3)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                }
                                else if (totalAmount.ToString().Count() == 3)
                                {
                                    if (count == 0)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " HUNDRED ";
                                    }
                                    if (count == 1)
                                    {
                                        amountInWords += CommonUtil.getWord(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                    if (count == 2)
                                    {
                                        amountInWords += CommonUtil.getDecimalValue(Convert.ToInt32(totalAmount.ToString()[count].ToString())) + " ";
                                    }
                                }
                            }
                        }

                        amountInWords += "ONLY";

                        #endregion

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\t\tSignature of the MPKBY Agent\n\t\t" +
                                             agent.AgentName + "\n" +
                                             "\t\t\t\t\t\t\tCERTIFICATES BY POST OFFICE\n\n" +
                                             "\t\tIt is certified that a total Sum of Rupees " + totalAmount + " RUPEES in word \n\t\t" + amountInWords + "\n\t\tHas been received and deposited/credited/as shown in the R.D. \n\t\tAccount Pass books of the investors concerned.\n\n\n" +
                                             "\t\tDt. " + customerRegister[0].Date.ToShortDateString() + "\t\t\t\t\t\t\t\tSignature of the Post Master\n" +
                                             "\t\tSeal of the Post Office\t\t\t\t\t\t\tPATEL COLONY SUB.P.O.\n" +
                                             "\t\t\t\t\t\t\t\t\t\t\tJAMNAGAR."))));
                        currentRow.Cells[0].ColumnSpan = 9;
                        flwDocRDList.Blocks.Clear();
                        flwDocRDList.Blocks.Add(rdTable);
                        flwDocRDList.ColumnWidth = 800;
                        rdTable.CellSpacing = 3;
                        flwDocRDList.FontSize = 12;
                    }
                    else
                        chkScreen.IsEnabled = false;
                }
                catch(Exception exception)
                {
                    ToastNotification.Toast("Error", exception.Message);
                }
            }
        }

        void dtpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpDate.Text != string.Empty)
            {
                var LotNo = (from p in _db.RDRegisters where p.MonthYearValue == (dtpDate.SelectedDate.Value.Month.ToString() + dtpDate.SelectedDate.Value.Year.ToString()) select p.LotNo).Distinct();
                if (LotNo.ToList().Count > 0)
                {
                    cmbLotNo.IsEnabled = true;
                    chkScreen.IsEnabled = true;
                    cmbLotNo.ItemsSource = LotNo.ToList();
                    cmbLotNo.SelectedIndex = 0;
                    cmbLotNo_SelectionChanged(this, e);
                }
                else
                {
                    dtgRDRegister.ItemsSource = null;
                    cmbLotNo.IsEnabled = false;
                    chkScreen.IsEnabled = false;
                }
            }
        }
        private void ModernDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = ModernDialog.ShowMessage("Are You sure? You want to delete?", "Delete Confirmation", MessageBoxButton.OKCancel, MainWindow.GetWindow(this));
            if (result == MessageBoxResult.OK)
            {
                try
                {
                    RDRegister existingData = (RDRegister)dtgRDRegister.SelectedItem;
                    var count = (from p in _db.RDRegisters where p.MonthYearValue == existingData.MonthYearValue && p.LotNo == existingData.LotNo select p).ToList().Count;
                    if (count == 1)
                    {
                        var tdsReg = (from p in _db.RDTDSRegisters where p.MonthYearValue == existingData.MonthYearValue && p.LotNo == existingData.LotNo select p).FirstOrDefault();
                        if (tdsReg != null)
                            _db.RDTDSRegisters.DeleteOnSubmit(tdsReg);
                        dtpDate.Text = "";
                        cmbLotNo.IsEnabled = false;
                        chkScreen.IsEnabled = false;
                        dtgRDRegister.ItemsSource = "";
                    }

                    var customerInRegister = _db.RDRegisters.Where(p => p.MonthYearValue == existingData.MonthYearValue && p.RDAccountNo == existingData.RDAccountNo && p.Balance == existingData.Balance).FirstOrDefault();
                    _db.RDRegisters.DeleteOnSubmit(customerInRegister);
                    var customer = (from p in _db.RecurringDepositCustomers where p.RDAccountNo == existingData.RDAccountNo select p).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.Balance = Convert.ToString(existingData.Balance - (Convert.ToInt32(existingData.Amount)));
                        int sub = (Convert.ToInt32(existingData.Amount) / (Convert.ToInt32(customer.Amount)));
                        customer.NextDueDate = customer.NextDueDate.Value.Subtract(TimeSpan.FromDays((sub * 31) - (sub / 2)));
                        customer.LastCreditDate = customer.NextDueDate.Value.Subtract(TimeSpan.FromDays(30));
                    }
                    var rebateEntry = (from p in _db.RebetRDEntryDues where p.RDAccountNo == existingData.RDAccountNo && Convert.ToInt32(p.Balance) == existingData.Balance select p).FirstOrDefault();
                    if (rebateEntry != null)
                        _db.RebetRDEntryDues.DeleteOnSubmit(rebateEntry);

                    _db.SubmitChanges();
                    ToastNotification.Toast("Success", "Monthly entry deleted successfully.");
                    if (count != 1)
                        cmbLotNo_SelectionChanged(this, null);
                }
                catch (Exception ex)
                {
                    ToastNotification.Toast("Error", ex.Message);
                    cmbLotNo_SelectionChanged(this, null);
                }
            }
            else
            {
                cmbLotNo_SelectionChanged(this, null);
            }
        }

    }
}
