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

namespace Bachat.Pages
{
    /// <summary>
    /// Interaction logic for RebateRDEntry.xaml
    /// </summary>
    public partial class RebateRDEntry : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public RebateRDEntry()
        {
            InitializeComponent();

            dtpRebateFromDate.SelectedDateChanged += dtpRebateFromDate_SelectedDateChanged;
            dtpRebateToDate.SelectedDateChanged += dtpRebateToDate_SelectedDateChanged;
            btnSubmit.Click += btnSubmit_Click;
            btnPrint.Click += btnPrint_Click;
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewRebateRegister.Print();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewRebateRegister.Visibility = Visibility.Visible;
            showRebateDueEntry();
        }

        void dtpRebateToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSubmit.IsEnabled = true;
            flwScrollViewRebateRegister.Visibility = Visibility.Hidden;
        }

        void dtpRebateFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpRebateToDate.IsEnabled = true;
            flwScrollViewRebateRegister.Visibility = Visibility.Hidden;
        }

        void showRebateDueEntry()
        {
            if (dtpRebateFromDate.SelectedDate.Value.CompareTo(dtpRebateToDate.SelectedDate.Value) <= 0)
            {
                var rebateList = (from p in _db.RebetRDEntryDues where (p.NextDueDate >= dtpRebateFromDate.SelectedDate.Value && p.NextDueDate <= dtpRebateToDate.SelectedDate.Value) select p).ToList();
                if (rebateList.Count > 0)
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nRebeat R D Entry Due in this Month\n\n" + "-----------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;
                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    int numberOfColumns = 8;
                    for (int x = 0; x < numberOfColumns; x++)
                        rdTable.Columns.Add(new TableColumn());
                    currentRow = rdTable.RowGroups[0].Rows[1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sr No."))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Acc No."))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depositer Name"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Due Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Balance"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                    currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                    GridLength clmWidth = new GridLength(70, GridUnitType.Pixel);
                    rdTable.Columns[0].Width = clmWidth;
                    clmWidth = new GridLength(40, GridUnitType.Pixel);
                    rdTable.Columns[1].Width = clmWidth;
                    clmWidth = new GridLength(60, GridUnitType.Pixel);
                    rdTable.Columns[2].Width = clmWidth;
                    clmWidth = new GridLength(180, GridUnitType.Pixel);
                    rdTable.Columns[3].Width = clmWidth;
                    clmWidth = new GridLength(80, GridUnitType.Pixel);
                    rdTable.Columns[4].Width = clmWidth;
                    clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[5].Width = clmWidth;
                    clmWidth = new GridLength(60, GridUnitType.Pixel);
                    rdTable.Columns[6].Width = clmWidth;
                    clmWidth = new GridLength(70, GridUnitType.Pixel);
                    rdTable.Columns[7].Width = clmWidth;

                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[2];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("----------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;
                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                    for (int i = 0; i < rebateList.Count; i++)
                    {
                        int m = i + 1;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rebateList[i].RDAccountNo))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rebateList[i].DepositerName))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rebateList[i].NextDueDate.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rebateList[i].Amount))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rebateList[i].Balance))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                    }
                    btnPrint.IsEnabled = true;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;

                    flwRebateRegister.Blocks.Clear();
                    flwRebateRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRebateRegister.FontSize = 12;
                }
                else
                {
                    Table rdTable = new Table();
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwRebateRegister.Blocks.Clear();
                    flwRebateRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRebateRegister.FontSize = 12;
                    btnPrint.IsEnabled = false;
                }
            }
        }
    }
}
