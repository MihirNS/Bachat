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
    /// Interaction logic for DebitRDEntry.xaml
    /// </summary>
    public partial class DebitRDEntry : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public DebitRDEntry()
        {
            InitializeComponent();
            showRDCustomers();
        }

        void showRDCustomers()
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var debitRDList = (from p in _db.RecurringDepositCustomers where p.NextDueDate.Value <= date && p.ClosedAccount == false && p.MaturityDate > DateTime.Now select p).ToList();

            if (debitRDList.Count > 0)
            {
                Table rdTable = new Table();
                rdTable.RowGroups.Add(new TableRowGroup());
                rdTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Debit R D Entry of Current Month\n\n" + "----------------------------------------------------------------------------------------------------------------------------------------------------"))));
                currentRow.Cells[0].ColumnSpan = 6;
                currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                rdTable.RowGroups[0].Rows.Add(new TableRow());
                int numberOfColumns = 6;
                for (int x = 0; x < numberOfColumns; x++)
                    rdTable.Columns.Add(new TableColumn());
                currentRow = rdTable.RowGroups[0].Rows[1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sr No."))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Acc No."))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depositer Name"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Balance"))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Last Credit Date"))));

                currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                currentRow.Cells[5].TextAlignment = TextAlignment.Center;

                GridLength clmWidth = new GridLength(4, GridUnitType.Star);
                rdTable.Columns[0].Width = clmWidth;
                clmWidth = new GridLength(10, GridUnitType.Star);
                rdTable.Columns[1].Width = clmWidth;
                clmWidth = new GridLength(16, GridUnitType.Star);
                rdTable.Columns[2].Width = clmWidth;
                clmWidth = new GridLength(8, GridUnitType.Star);
                rdTable.Columns[3].Width = clmWidth;
                clmWidth = new GridLength(8, GridUnitType.Star);
                rdTable.Columns[4].Width = clmWidth;
                clmWidth = new GridLength(8, GridUnitType.Star);
                rdTable.Columns[5].Width = clmWidth;

                rdTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = rdTable.RowGroups[0].Rows[2];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("----------------------------------------------------------------------------------------------------------------------------------------------------"))));
                currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                currentRow.Cells[0].ColumnSpan = 6;


                for (int i = 0; i < debitRDList.Count; i++)
                {
                    int m = i + 1;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(debitRDList[i].RDAccountNo))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(debitRDList[i].DepositerName))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(debitRDList[i].Amount))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(debitRDList[i].Balance))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(debitRDList[i].LastCreditDate.Value.ToShortDateString()))));

                    currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[2].TextAlignment = TextAlignment.Left;
                    currentRow.Cells[3].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                }
                rdTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("----------------------------------------------------------------------------------------------------------------------------------------------------"))));
                currentRow.Cells[0].TextAlignment = TextAlignment.Center;
                currentRow.Cells[0].ColumnSpan = 6;

                flwDebitRDEntry.Blocks.Clear();
                flwDebitRDEntry.Blocks.Add(rdTable);
                rdTable.CellSpacing = 1;
                flwDebitRDEntry.FontSize = 12;
            }
            else
            {
                Table rdTable = new Table();
                rdTable.RowGroups.Add(new TableRowGroup());
                rdTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                currentRow.Foreground = Brushes.Red;
                flwDebitRDEntry.Blocks.Clear();
                flwDebitRDEntry.Blocks.Add(rdTable);
                rdTable.CellSpacing = 1;
                flwDebitRDEntry.FontSize = 12;
            }
        }
    }
}
