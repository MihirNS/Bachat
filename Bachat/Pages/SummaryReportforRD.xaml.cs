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
    /// Interaction logic for SummaryReportforRD.xaml
    /// </summary>
    public partial class SummaryReportforRD : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public SummaryReportforRD()
        {
            InitializeComponent();

            cmbAccountNo.SelectionChanged += cmbAccountNo_SelectionChanged;
            dtpReportFromDate.SelectedDateChanged += dtpReportFromDate_SelectedDateChanged;
            dtpReportToDate.SelectedDateChanged += dtpReportToDate_SelectedDateChanged;
            btnSubmit.Click += btnSubmit_Click;

            cmbAccountNo.DataContext = (from p in _db.RecurringDepositCustomers orderby p.RDAccountNo select p.RDAccountNo).ToList();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewSummaryReportRegister.Visibility = Visibility.Visible;
            showSummaryReport();
        }

        void dtpReportToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSubmit.IsEnabled = true;
            flwScrollViewSummaryReportRegister.Visibility = Visibility.Hidden;
        }

        void dtpReportFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpReportToDate.IsEnabled = true;
            flwScrollViewSummaryReportRegister.Visibility = Visibility.Hidden;
        }

        void cmbAccountNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpReportFromDate.IsEnabled = true;
            flwScrollViewSummaryReportRegister.Visibility = Visibility.Hidden;
        }

        void showSummaryReport()
        {
            try
            {
                var PartAcc = (from p in _db.RDRegisters where (p.RDAccountNo == cmbAccountNo.SelectedValue.ToString() && p.Date >= dtpReportFromDate.SelectedDate.Value && p.Date <= dtpReportToDate.SelectedDate.Value) select p).ToList();

                if (PartAcc.Count > 0)
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Account No : " + cmbAccountNo.SelectedValue.ToString()))));
                    currentRow.Cells[0].ColumnSpan = 6;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Customer Name : " + PartAcc[0].Name))));
                    currentRow.Cells[0].ColumnSpan = 6;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[2];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("From Date " + dtpReportFromDate.SelectedDate.Value.ToShortDateString() + " To Date " + dtpReportToDate.SelectedDate.Value.ToShortDateString()))));
                    currentRow.Cells[0].ColumnSpan = 6;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[3];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("--------------------------------------------------------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells[0].ColumnSpan = 6;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[4];

                    int numberOfColumns = 6;
                    for (int x = 0; x < numberOfColumns; x++)
                        rdTable.Columns.Add(new TableColumn());
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Balance"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Due"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Rebate"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Remark"))));

                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[5];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("--------------------------------------------------------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells[0].ColumnSpan = 6;

                    for (int i = 0; i < PartAcc.Count; i++)
                    {
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Date.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Amount))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Balance.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Due.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Rebate.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(PartAcc[i].Remarks))));
                    }


                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("--------------------------------------------------------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells[0].ColumnSpan = 6;
                    flwSummaryReportRegister.Blocks.Clear();
                    flwSummaryReportRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwSummaryReportRegister.FontSize = 12;
                }
                else
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwSummaryReportRegister.Blocks.Clear();
                    flwSummaryReportRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwSummaryReportRegister.FontSize = 12;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Can't Load Data");
            }
        }
    }
}
