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

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for TDSRegister.xaml
    /// </summary>
    public partial class TDSRegister : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public TDSRegister()
        {
            InitializeComponent();
            cmbReportType.DataContext = new String[] { "Datewise", "Monthwise" };
            cmbSchemeType.DataContext = new String[] { "RD", "NKM" };

            cmbReportType.SelectionChanged += cmbReportType_SelectionChanged;
            cmbSchemeType.SelectionChanged += cmbSchemeType_SelectionChanged;
            cmbPostOffice.SelectionChanged += cmbPostOffice_SelectionChanged;
            dtpTDSFromDate.SelectedDateChanged += dtpTDSFromDate_SelectedDateChanged;
            dtpTDSToDate.SelectedDateChanged += dtpTDSToDate_SelectedDateChanged;
            btnPrint.Click += btnPrint_Click;
            btnSubmit.Click += btnSubmit_Click;

            var postOffices = (from p in _db.PostOffices select p.PostOfficeName).ToList();;
            postOffices.Insert(0, "All");
            cmbPostOffice.DataContext = postOffices;
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewTDSRegister.Visibility = Visibility.Visible;
            var scheme = cmbSchemeType.SelectedValue.ToString();
            if (scheme.Equals("RD"))
            {
                showRDTDSData();
            }
            else
            {
                showNKMTDSData();
            }
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewTDSRegister.Print();
        }
      
        void cmbPostOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSubmit.IsEnabled = true;
            flwScrollViewTDSRegister.Visibility = Visibility.Hidden;
        }

        void dtpTDSToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbReportType.IsEnabled = true;
            flwScrollViewTDSRegister.Visibility = Visibility.Hidden;
        }

        void dtpTDSFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpTDSToDate.IsEnabled = true;
            flwScrollViewTDSRegister.Visibility = Visibility.Hidden;
        }

        void cmbSchemeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSchemeType.SelectedIndex != -1)
            {
                dtpTDSFromDate.IsEnabled = true;
                flwScrollViewTDSRegister.Visibility = Visibility.Hidden;
                if (cmbSchemeType.SelectedValue.ToString().Equals("RD"))
                {
                    cmbPostOffice.Visibility = Visibility.Hidden;
                }
                else
                {
                    cmbPostOffice.IsEnabled = false;
                    if (cmbReportType.IsEnabled)
                    {
                        cmbReportType_SelectionChanged(sender, null);
                    }
                }
            }
        }

        void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            if (cmbReportType.SelectedIndex != -1)
            {
                var scheme = cmbSchemeType.SelectedValue.ToString();
                var reportType = cmbReportType.SelectedValue.ToString();
                flwScrollViewTDSRegister.Visibility = Visibility.Hidden;
                if (scheme.Equals("RD"))
                {
                    cmbPostOffice.Visibility = Visibility.Hidden;
                    btnSubmit.IsEnabled = true;
                }
                else if (scheme.Equals("NKM") && reportType.Equals("Monthwise"))
                {
                    cmbPostOffice.Visibility = Visibility.Visible;
                    cmbPostOffice.SelectedIndex = 0;
                }
                else
                {
                    cmbPostOffice.Visibility = Visibility.Visible;
                    cmbPostOffice.IsEnabled = true;
                }
            }
        }

        void showRDTDSData()
        {
            try
            {
                var tdsRegister = (from p in _db.RDTDSRegisters where (p.Date >= dtpTDSFromDate.SelectedDate.Value && p.Date <= dtpTDSToDate.SelectedDate.Value) select p).ToList();
                if (tdsRegister.Count > 0)
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    var agent = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\t\t\tStatement Showing the Details of RD TDS Deducted \n\n" +
                        "\t\t\t  From Date " + dtpTDSFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpTDSToDate.SelectedDate.Value.ToShortDateString() + "\n\n" +
                        "\t         Agent Name : " + agent.AgentName + "         Agency No : " + agent.AuthorityNo + "\n\n" +
                        "-----------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    if (cmbReportType.SelectedValue.ToString().Equals("Datewise"))
                    {
                        currentRow.Cells[1].ColumnSpan = 9;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        int numberOfColumns = 11;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[0].Rows[1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Type"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gross AMT"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Commission"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TDS"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sur"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Net Comm."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        GridLength clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(90, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(40, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[8].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[9].Width = clmWidth;
                        clmWidth = new GridLength(20, GridUnitType.Pixel);
                        rdTable.Columns[10].Width = clmWidth;

                        currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[9].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[10].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[2];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 9;

                        int GrossAMT = 0;
                        int commission = 0;
                        int TDS = 0;
                        int total = 0;
                        int netcommission = 0;
                        int totalSur = 0;

                        for (int i = 0; i < tdsRegister.Count; i++)
                        {
                            int m = i + 1;
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            GrossAMT += Convert.ToInt32(tdsRegister[i].GrossAMT);
                            commission += Convert.ToInt32(tdsRegister[i].Commission);
                            TDS += Convert.ToInt32(tdsRegister[i].TDS);
                            total += Convert.ToInt32(tdsRegister[i].Total);
                            netcommission += Convert.ToInt32(tdsRegister[i].NetCommission);
                            totalSur += Convert.ToInt32(tdsRegister[i].Sur);

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].Type))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].Date.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].GrossAMT))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].Commission))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].TDS.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].Sur.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].Total.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(tdsRegister[i].NetCommission.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[9].TextAlignment = TextAlignment.Right;
                        }

                        btnPrint.IsEnabled = true;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 9;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(GrossAMT.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(commission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(TDS.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalSur.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(total.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(netcommission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[9].TextAlignment = TextAlignment.Right;

                        flwTDSRegister.Blocks.Clear();
                        flwTDSRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwTDSRegister.FontSize = 12;
                    }
                    else
                    {
                        currentRow.Cells[1].ColumnSpan = 7;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        int numberOfColumns = 9;

                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[0].Rows[1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Month"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gross AMT"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Commission"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TDS"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sur"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Net Comm."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        GridLength clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(120, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(84, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[8].Width = clmWidth;

                        currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[2];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 7;

                        int totalGrossAMT = 0;
                        int totalCommission = 0;
                        int totalTDS = 0;
                        int totalOftotal = 0;
                        int totalnetcommission = 0;
                        int totalSur = 0;

                        int months = (dtpTDSToDate.SelectedDate.Value.Month - dtpTDSFromDate.SelectedDate.Value.Month) + ((dtpTDSToDate.SelectedDate.Value.Year - dtpTDSFromDate.SelectedDate.Value.Year) * 12);

                        for (int i = 0; i <= months; i++)
                        {
                            DateTime newDate = dtpTDSFromDate.SelectedDate.Value.AddMonths(i);
                            string monthyear = newDate.Month.ToString() + newDate.Year.ToString();

                            int GrossAMT = 0;
                            int commission = 0;
                            int TDS = 0;
                            int total = 0;
                            int netcommission = 0;
                            int sur = 0;

                            for (int j = 0; j < tdsRegister.Count; j++)
                            {
                                if (tdsRegister[j].MonthYearValue == monthyear)
                                {
                                    GrossAMT += Convert.ToInt32(tdsRegister[j].GrossAMT);
                                    commission += Convert.ToInt32(tdsRegister[j].Commission);
                                    TDS += Convert.ToInt32(tdsRegister[j].TDS);
                                    total += Convert.ToInt32(tdsRegister[j].Total);
                                    netcommission += Convert.ToInt32(tdsRegister[j].NetCommission);
                                    sur += Convert.ToInt32(tdsRegister[j].Sur);
                                }
                            }

                            totalGrossAMT += GrossAMT;
                            totalCommission += commission;
                            totalTDS += TDS;
                            totalOftotal += total;
                            totalnetcommission += netcommission;
                            totalSur += sur;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            string month = CommonUtil.getMonthName(newDate.Month);

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(month + " - " + newDate.Year))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(GrossAMT.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(commission.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(TDS.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(sur.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(total.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(netcommission.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        }

                        btnPrint.IsEnabled = true;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 7;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalGrossAMT.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalCommission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalTDS.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalSur.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalOftotal.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalnetcommission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;

                        flwTDSRegister.Blocks.Clear();
                        flwTDSRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwTDSRegister.FontSize = 12;
                    }
                }
                else
                {
                    btnPrint.IsEnabled = false;
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwTDSRegister.Blocks.Clear();
                    flwTDSRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwTDSRegister.FontSize = 12;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Data can't be loaded");
            }
        }

        void showNKMTDSData()
        {
            var NKMTDSRegister = new List<NKMTDSRegister>();
            try
            {
                if (cmbPostOffice.SelectedValue.ToString() == "All")
                    NKMTDSRegister = (from p in _db.NKMTDSRegisters where p.IssueDate >= dtpTDSFromDate.SelectedDate.Value && p.IssueDate <= dtpTDSToDate.SelectedDate.Value select p).ToList();
                else
                    NKMTDSRegister = (from p in _db.NKMTDSRegisters where (p.NKMDepositer.PostOffice1.PostOfficeName == cmbPostOffice.SelectedValue.ToString() && p.IssueDate >= dtpTDSFromDate.SelectedDate.Value && p.IssueDate <= dtpTDSToDate.SelectedDate.Value) select p).ToList();


                if (NKMTDSRegister.Count > 0)
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    var agent = (from p in _db.AgentProfiles where p.Type == "NKM" select p).FirstOrDefault();

                    if (cmbReportType.SelectedValue.ToString().Equals("Datewise"))
                    {
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n\t\t\t\t Statement Showing the Details of TDS Deducted \n\n" +
                            "\t\t\t\t      From Date " + dtpTDSFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpTDSToDate.SelectedDate.Value.ToShortDateString() + "\n\n" +
                            "\t\t              Agent Name : " + agent.AgentName + "         SS Autho No : " + agent.AuthorityNo + "\n\n" +
                            "-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 10;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        int numberOfColumns = 12;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[0].Rows[1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Type"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Issue Date"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Post Office Name"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gross AMT"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Commission"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TDS"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sur"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Net Comm."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        GridLength clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(120, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[8].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[9].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[10].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[11].Width = clmWidth;

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[9].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[10].TextAlignment = TextAlignment.Right;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[2];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 10;

                        int GrossAMT = 0;
                        int commission = 0;
                        int TDS = 0;
                        int total = 0;
                        int netcommission = 0;
                        int totalSur = 0;

                        for (int i = 0; i < NKMTDSRegister.Count; i++)
                        {
                            int m = i + 1;
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            GrossAMT += Convert.ToInt32(NKMTDSRegister[i].GrossAmount);
                            commission += Convert.ToInt32(NKMTDSRegister[i].Commission);
                            TDS += Convert.ToInt32(NKMTDSRegister[i].TDS);
                            total += Convert.ToInt32(NKMTDSRegister[i].Total);
                            netcommission += Convert.ToInt32(NKMTDSRegister[i].NetCommission);
                            totalSur += Convert.ToInt32(NKMTDSRegister[i].Sur);

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMTDSRegister[i].Type))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMTDSRegister[i].IssueDate.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMTDSRegister[i].NKMDepositer.PostOffice1.PostOfficeName))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMTDSRegister[i].GrossAmount))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMTDSRegister[i].Commission))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToInt32(NKMTDSRegister[i].TDS).ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToInt32(NKMTDSRegister[i].Sur).ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToInt32(NKMTDSRegister[i].Total).ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Convert.ToInt32(NKMTDSRegister[i].NetCommission).ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[9].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[10].TextAlignment = TextAlignment.Right;
                        }

                        btnPrint.IsEnabled = true;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 10;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(GrossAMT.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(commission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(TDS.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalSur.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(total.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(netcommission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[8].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[9].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[10].TextAlignment = TextAlignment.Right;

                        flwTDSRegister.Blocks.Clear();
                        flwTDSRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwTDSRegister.FontSize = 12;
                    }
                    else
                    {

                        //var schemes = (from p in _db.Schemes where p.TypeOfInvestment != "RD" select p).ToList();
                        var schemes = (from p in NKMTDSRegister select p.NKMDepositer.Scheme.TypeOfInvestment).Distinct().ToList();
                        int totalSchemes = schemes.Count;
                        int numberOfColumns = 0;

                        numberOfColumns = 7 + totalSchemes;

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\nStatement Showing the Details of TDS Deducted"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = numberOfColumns - 2;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("From Date " + dtpTDSFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpTDSToDate.SelectedDate.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = numberOfColumns - 2;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Agent Name : " + agent.AgentName + "         SS Autho No : " + agent.AuthorityNo))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = numberOfColumns - 2;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        string line = "";
                        for (double k = 4.80; k < (315 + (totalSchemes * 50)); k += 4.80)
                            line += "-";
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(line))));
                        currentRow.Cells[0].ColumnSpan = numberOfColumns;
                        //Line a = new Line();

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        //  currentRow.Cells[0].ColumnSpan = numberOfColumns;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Month"))));
                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(schemes[k].ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gro.AMT"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Commi."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TDS"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sur"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Net"))));

                        GridLength clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        for (int k = 0; k < totalSchemes; k++)
                        {
                            clmWidth = new GridLength(50, GridUnitType.Pixel);
                            rdTable.Columns[k + 1].Width = clmWidth;
                        }
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 1].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 2].Width = clmWidth;
                        clmWidth = new GridLength(40, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 3].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 4].Width = clmWidth;
                        clmWidth = new GridLength(40, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 5].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[totalSchemes + 6].Width = clmWidth;

                        currentRow.Cells[0].TextAlignment = TextAlignment.Left;
                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells[k + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 2].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 6].TextAlignment = TextAlignment.Right;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Number Of Applications"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Comm."))));
                        currentRow.Cells[1].ColumnSpan = numberOfColumns - 2;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Right;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(line))));
                        currentRow.Cells[0].ColumnSpan = numberOfColumns;

                        int months = (dtpTDSToDate.SelectedDate.Value.Month - dtpTDSFromDate.SelectedDate.Value.Month) + ((dtpTDSToDate.SelectedDate.Value.Year - dtpTDSFromDate.SelectedDate.Value.Year) * 12);

                        int totalGrossAMT = 0;
                        int totalCommission = 0;
                        int totalTDS = 0;
                        int totalSur = 0;
                        int totalOftotal = 0;
                        int totalnetcommission = 0;

                        List<int> schemeAmountTotal = new List<int>(totalSchemes);
                        List<int> schemeCountTotal = new List<int>(totalSchemes);
                        for (int k = 0; k < totalSchemes; k++)
                        {
                            schemeAmountTotal.Add(0);
                            schemeCountTotal.Add(0);
                        }

                        for (int i = 0; i <= months; i++)
                        {
                            DateTime newDate = dtpTDSFromDate.SelectedDate.Value.AddMonths(i);
                            string monthyear = newDate.Month.ToString() + newDate.Year.ToString();

                            int GrossAMT = 0;
                            int commission = 0;
                            int TDS = 0;
                            int total = 0;
                            int netcommission = 0;
                            int sur = 0;

                            List<int> schemeCount = new List<int>(totalSchemes);
                            List<int> monthlySchemeAmountTotal = new List<int>(totalSchemes);

                            for (int k = 0; k < totalSchemes; k++)
                            {
                                schemeCount.Add(0);
                                monthlySchemeAmountTotal.Add(0);
                            }

                            for (int j = 0; j < NKMTDSRegister.Count; j++)
                            {
                                if (NKMTDSRegister[j].MonthYearValue == monthyear)
                                {
                                    GrossAMT += Convert.ToInt32(NKMTDSRegister[j].GrossAmount);
                                    commission += Convert.ToInt32(NKMTDSRegister[j].Commission);
                                    TDS += Convert.ToInt32(NKMTDSRegister[j].TDS);
                                    total += Convert.ToInt32(NKMTDSRegister[j].Total);
                                    netcommission += Convert.ToInt32(NKMTDSRegister[j].NetCommission);
                                    sur += Convert.ToInt32(NKMTDSRegister[j].Sur);

                                    for (int k = 0; k < schemes.Count; k++)
                                    {
                                        if (NKMTDSRegister[j].NKMDepositer.Scheme.TypeOfInvestment == schemes[k].ToString())
                                        {
                                            schemeCount[k] += 1;
                                            monthlySchemeAmountTotal[k] += Convert.ToInt32(NKMTDSRegister[j].GrossAmount);
                                        }
                                    }
                                }
                            }

                            for (int k = 0; k < totalSchemes; k++)
                                schemeCountTotal[k] += schemeCount[k];
                            for (int k = 0; k < totalSchemes; k++)
                                schemeAmountTotal[k] += monthlySchemeAmountTotal[k];

                            totalGrossAMT += GrossAMT;
                            totalCommission += commission;
                            totalTDS += TDS;
                            totalOftotal += total;
                            totalnetcommission += netcommission;
                            totalSur += sur;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            string month = CommonUtil.getMonthName(newDate.Month);

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(month + " - " + newDate.Year))));

                            for (int k = 0; k < totalSchemes; k++)
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(monthlySchemeAmountTotal[k].ToString()))));

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(GrossAMT.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(commission.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(TDS.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(sur.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(total.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(netcommission.ToString()))));

                            currentRow.Cells[0].TextAlignment = TextAlignment.Left;
                            for (int k = 0; k < totalSchemes; k++)
                                currentRow.Cells[k + 1].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 1].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 2].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 6].TextAlignment = TextAlignment.Right;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            for (int k = 0; k < totalSchemes; k++)
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("( " + schemeCount[k].ToString() + " )"))));

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[0].TextAlignment = TextAlignment.Left;
                            for (int k = 0; k < totalSchemes; k++)
                                currentRow.Cells[k + 1].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 1].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 2].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[totalSchemes + 6].TextAlignment = TextAlignment.Right;
                        }

                        btnPrint.IsEnabled = true;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(line))));
                        currentRow.Cells[0].ColumnSpan = numberOfColumns;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(schemeAmountTotal[k].ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalGrossAMT.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalCommission.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalTDS.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalSur.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalOftotal.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(totalnetcommission.ToString()))));

                        currentRow.Cells[0].TextAlignment = TextAlignment.Left;
                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells[k + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 2].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 6].TextAlignment = TextAlignment.Right;

                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("( " + schemeCountTotal[k].ToString() + " )"))));

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[0].TextAlignment = TextAlignment.Left;
                        for (int k = 0; k < totalSchemes; k++)
                            currentRow.Cells[k + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 1].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 2].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 4].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[totalSchemes + 6].TextAlignment = TextAlignment.Right;

                        flwTDSRegister.Blocks.Clear();
                        flwTDSRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwTDSRegister.FontSize = 12;
                    }
                }
                else
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    btnPrint.IsEnabled = false;
                    flwTDSRegister.Blocks.Clear();
                    flwTDSRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwTDSRegister.FontSize = 12;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Data can't be loaded");
            }
        }
    }
}
