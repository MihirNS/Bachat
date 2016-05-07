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
    /// Interaction logic for NKMDatewiseEntries.xaml
    /// </summary>
    public partial class NKMDatewiseEntries : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public NKMDatewiseEntries()
        {
            InitializeComponent();

            cmbDateType.DataContext = new String[] { "Opening Datewise", "Maturity Datewise" };

            cmbDateType.SelectionChanged += cmbDateType_SelectionChanged;
            dtpTDSFromDate.SelectedDateChanged += dtpTDSFromDate_SelectedDateChanged;
            dtpTDSToDate.SelectedDateChanged += dtpTDSToDate_SelectedDateChanged;
            btnSubmit.Click += btnSubmit_Click;
            btnPrint.Click += btnPrint_Click;
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewTDSRegister.Print();
        }       

        void dtpTDSToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
 	        cmbDateType.IsEnabled = true;
        }

        void dtpTDSFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
 	        dtpTDSToDate.IsEnabled = true;
        }

        void cmbDateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
 	        btnSubmit.IsEnabled = true;
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDateType.SelectedValue.Equals("Opening Datewise"))
            {
                showOpeningDateWise();
            }
            else
            {
                showMaturityDatewise();
            }
        }

        void showMaturityDatewise()
        {
            try
            {
                Table rdTable = new Table();
                var scheme = (from p in _db.Schemes select p).ToList();
                bool flag = true;
                for (int m = 0; m < scheme.Count; m++)
                {
                    var nkmMatDateWise = (from p in _db.NKMDepositers where (p.MaturityDate >= dtpTDSFromDate.SelectedDate.Value && p.MaturityDate <= dtpTDSToDate.SelectedDate.Value && p.Scheme.TypeOfInvestment == scheme[m].TypeOfInvestment) orderby p.MaturityDate select p).ToList();

                    if (nkmMatDateWise.Count > 0)
                    {
                        flag = false;
                        rdTable.RowGroups.Add(new TableRowGroup());
                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        TableRow currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n" + scheme[m].TypeOfInvestment + " Depositers Maturity Datewise \n" +
                            "From Date " + dtpTDSFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpTDSToDate.SelectedDate.Value.ToShortDateString() + "\n" +
                            "-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 6;
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        int numberOfColumns = 8;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depositer Name"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Of Deposit"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Of Maturity"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                        GridLength clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(200, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;

                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 6;

                        for (int i = 0; i < nkmMatDateWise.Count; i++)
                        {
                            int k = i + 1;
                            rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(k.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmMatDateWise[i].DepositerName))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmMatDateWise[i].Amount))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmMatDateWise[i].DateOfDeposit.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmMatDateWise[i].MaturityDate.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmMatDateWise[i].MaturityAmount.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        }

                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 6;
                        btnPrint.IsEnabled = true;
                    }

                }

                if (flag)
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwTDSRegister.Blocks.Clear();
                    flwTDSRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwTDSRegister.FontSize = 12;
                    flwTDSRegister.IsEnabled = false;
                }
                else
                {
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

        void showOpeningDateWise()
        {
            try
            {
                Table rdTable = new Table();
                var scheme = (from p in _db.Schemes select p).ToList();
                bool flag = true;
                for (int m = 0; m < scheme.Count; m++)
                {
                    var nkmOpDateWise = (from p in _db.NKMDepositers where (p.DateOfDeposit >= dtpTDSFromDate.SelectedDate.Value && p.DateOfDeposit <= dtpTDSToDate.SelectedDate.Value && p.Scheme.TypeOfInvestment == scheme[m].TypeOfInvestment) orderby p.DateOfDeposit select p).ToList();

                    if (nkmOpDateWise.Count > 0)
                    {
                        flag = false;
                        rdTable.RowGroups.Add(new TableRowGroup());
                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        TableRow currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\n\n" + scheme[m].TypeOfInvestment + " Depositers Opening Datewise \n" +
                            "From Date " + dtpTDSFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpTDSToDate.SelectedDate.Value.ToShortDateString() + "\n" +
                            "-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[1].ColumnSpan = 6;
                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        int numberOfColumns = 8;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No."))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depositer Name"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Of Deposit"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Of Maturity"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                        GridLength clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(30, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(200, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(100, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;

                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 6;

                        for (int i = 0; i < nkmOpDateWise.Count; i++)
                        {
                            int k = i + 1;
                            rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];

                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(k.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmOpDateWise[i].DepositerName))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmOpDateWise[i].Amount))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmOpDateWise[i].DateOfDeposit.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmOpDateWise[i].MaturityDate.ToShortDateString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(nkmOpDateWise[i].MaturityAmount.ToString()))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        }

                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------------"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells[1].ColumnSpan = 6;
                        btnPrint.IsEnabled = true;
                    }

                }

                if (flag)
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwTDSRegister.Blocks.Clear();
                    flwTDSRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwTDSRegister.FontSize = 12;
                    flwTDSRegister.IsEnabled = false;
                }
                else
                {
                    flwTDSRegister.Blocks.Clear();
                    flwTDSRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwTDSRegister.FontSize = 12;
                }
                flwScrollViewTDSRegister.Visibility = Visibility.Visible;
            }
            catch
            {
                ToastNotification.Toast("Error", "Data can't be loaded");
            }
        }
    }
}
