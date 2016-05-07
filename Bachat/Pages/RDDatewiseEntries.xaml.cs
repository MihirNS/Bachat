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
    /// Interaction logic for RDDatewiseEntries.xaml
    /// </summary>
    public partial class RDDatewiseEntries : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public RDDatewiseEntries()
        {
            InitializeComponent();
            cmbDateType.DataContext = new String[] { "Opening Datewise", "Closing Datewise" };
            cmbDateType.SelectionChanged += cmbDateType_SelectionChanged;
            dtpRDFromDate.SelectedDateChanged += dtpRDFromDate_SelectedDateChanged;
            dtpRDToDate.SelectedDateChanged += dtpRDToDate_SelectedDateChanged;
            btnSubmit.Click += btnSubmit_Click;
            btnPrint.Click += btnPrint_Click;
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewRDRegister.Print();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDateType.SelectedValue.Equals("Opening Datewise"))
            {
                showOpeningDateWise();
            }
            else
            {
                showClosingDatewise();
            }
            flwScrollViewRDRegister.Visibility = Visibility.Visible;
        }

        void dtpRDToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbDateType.IsEnabled = true;
            flwScrollViewRDRegister.Visibility = Visibility.Hidden;
        }

        void dtpRDFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpRDToDate.IsEnabled = true;
            flwScrollViewRDRegister.Visibility = Visibility.Hidden;
        }

        void cmbDateType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSubmit.IsEnabled = true;
            flwScrollViewRDRegister.Visibility = Visibility.Hidden;
        }

        void showOpeningDateWise()
        {
            try
            {
                Table rdTable = new Table();
                var rdOpDateWise = (from p in _db.RecurringDepositCustomers where (p.DateOfOpening.Value >= dtpRDFromDate.SelectedDate.Value && p.DateOfOpening.Value <= dtpRDToDate.SelectedDate.Value) orderby p.DateOfOpening.Value select p).ToList();

                if (rdOpDateWise.Count > 0)
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nRD Depositers Opening Datewise\n" +
                        "From Date " + dtpRDFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpRDToDate.SelectedDate.Value.ToShortDateString() + "\n" +
                        "---------------------------------------------------------------------------------------------------------"))));
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
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Opening Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Maturity Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                    currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                    GridLength clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[0].Width = clmWidth;
                    clmWidth = new GridLength(30, GridUnitType.Pixel);
                    rdTable.Columns[1].Width = clmWidth;
                    clmWidth = new GridLength(170, GridUnitType.Pixel);
                    rdTable.Columns[2].Width = clmWidth;
                    clmWidth = new GridLength(60, GridUnitType.Pixel);
                    rdTable.Columns[3].Width = clmWidth;
                    clmWidth = new GridLength(90, GridUnitType.Pixel);
                    rdTable.Columns[4].Width = clmWidth;
                    clmWidth = new GridLength(90, GridUnitType.Pixel);
                    rdTable.Columns[5].Width = clmWidth;
                    clmWidth = new GridLength(80, GridUnitType.Pixel);
                    rdTable.Columns[6].Width = clmWidth;
                    clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[7].Width = clmWidth;

                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;

                    for (int i = 0; i < rdOpDateWise.Count; i++)
                    {
                        int k = i + 1;
                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(k.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdOpDateWise[i].DepositerName))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdOpDateWise[i].Amount))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdOpDateWise[i].DateOfOpening.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdOpDateWise[i].MaturityDate.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdOpDateWise[i].MaturityAmount.ToString()))));
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
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;
                    btnPrint.IsEnabled = true;

                    flwRDRegister.Blocks.Clear();
                    flwRDRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRDRegister.FontSize = 12;
                }
                else
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwRDRegister.Blocks.Clear();
                    flwRDRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRDRegister.FontSize = 12;
                    btnPrint.IsEnabled = false;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Can't load data");
            }
        }

        void showClosingDatewise()
        {
            try
            {
                Table rdTable = new Table();
                var rdClosingDateWise = (from p in _db.RecurringDepositCustomers where (p.ClosingDate >= dtpRDFromDate.SelectedDate.Value && p.ClosingDate <= dtpRDToDate.SelectedDate.Value) orderby p.ClosingDate.Value select p).ToList();

                if (rdClosingDateWise.Count > 0)
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nRD Depositers Closing Datewise\n" +
                        "From Date " + dtpRDFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpRDToDate.SelectedDate.Value.ToShortDateString() + "\n" +
                        "---------------------------------------------------------------------------------------------------------"))));
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
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Closing Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Maturity Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                    currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[6].TextAlignment = TextAlignment.Right;

                    GridLength clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[0].Width = clmWidth;
                    clmWidth = new GridLength(30, GridUnitType.Pixel);
                    rdTable.Columns[1].Width = clmWidth;
                    clmWidth = new GridLength(170, GridUnitType.Pixel);
                    rdTable.Columns[2].Width = clmWidth;
                    clmWidth = new GridLength(60, GridUnitType.Pixel);
                    rdTable.Columns[3].Width = clmWidth;
                    clmWidth = new GridLength(90, GridUnitType.Pixel);
                    rdTable.Columns[4].Width = clmWidth;
                    clmWidth = new GridLength(90, GridUnitType.Pixel);
                    rdTable.Columns[5].Width = clmWidth;
                    clmWidth = new GridLength(80, GridUnitType.Pixel);
                    rdTable.Columns[6].Width = clmWidth;
                    clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[7].Width = clmWidth;

                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;

                    for (int i = 0; i < rdClosingDateWise.Count; i++)
                    {
                        int k = i + 1;
                        rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows[rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(k.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdClosingDateWise[i].DepositerName))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdClosingDateWise[i].Amount))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdClosingDateWise[i].ClosingDate.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdClosingDateWise[i].MaturityDate.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdClosingDateWise[i].MaturityAmount.ToString()))));
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
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 6;
                    btnPrint.IsEnabled = true;

                    flwRDRegister.Blocks.Clear();
                    flwRDRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRDRegister.FontSize = 12;
                }
                else
                {
                    rdTable.RowGroups.Add(new TableRowGroup());
                    rdTable.RowGroups[rdTable.RowGroups.Count - 1].Rows.Add(new TableRow());
                    TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwRDRegister.Blocks.Clear();
                    flwRDRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwRDRegister.FontSize = 12;
                    btnPrint.IsEnabled = false;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Can't load data");
            }
        }
    }
}
