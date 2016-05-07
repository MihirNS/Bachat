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
    /// Interaction logic for RDBalanceByDate.xaml
    /// </summary>
    public partial class RDBalanceByDate : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public RDBalanceByDate()
        {
            InitializeComponent();

            dtpBalanceFromDate.SelectedDateChanged += dtpBalanceFromDate_SelectedDateChanged;
            dtpBalanceToDate.SelectedDateChanged += dtpBalanceToDate_SelectedDateChanged;
            btnSubmit.Click += btnSubmit_Click;
            btnPrint.Click += btnPrint_Click;
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewBalanceRegister.Print();
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewBalanceRegister.Visibility = Visibility.Visible;
            showBalanceByDate();
        }

        void dtpBalanceToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSubmit.IsEnabled = true;
            flwScrollViewBalanceRegister.Visibility = Visibility.Hidden;
        }

        void dtpBalanceFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpBalanceToDate.IsEnabled = true;
            flwScrollViewBalanceRegister.Visibility = Visibility.Hidden;
        }

        void showBalanceByDate()
        {
            try
            {
                Table rdTable = new Table();
                rdTable.RowGroups.Add(new TableRowGroup());
                rdTable.RowGroups[0].Rows.Add(new TableRow());
                TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                List<RDRegister> RDAccBalanceList = new List<RDRegister>();

                var data = (from p in _db.RecurringDepositCustomers where p.DateOfOpening <= dtpBalanceToDate.SelectedDate.Value && p.ClosedAccount == false select p).ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    var newReg = (from p in _db.RDRegisters where (p.RDAccountNo == data[i].RDAccountNo && p.Date <= dtpBalanceToDate.SelectedDate.Value) select p).ToList();
                    if (newReg.Count != 0)
                        RDAccBalanceList.Add(newReg[newReg.Count - 1]);
                }

                if (RDAccBalanceList.Count > 0)
                {
                    var agent = (from p in _db.AgentProfiles where p.Type == "RD" select p).FirstOrDefault();
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nRecurring Deposit Account Balance As On\n\n" +
                        "From Date " + dtpBalanceFromDate.SelectedDate.Value.ToShortDateString() + "   To Date " + dtpBalanceToDate.SelectedDate.Value.ToShortDateString() + "\n\n" +
                        "Agent Name : " + agent.AgentName + "         Agency No : " + agent.AuthorityNo + "\n\n" +
                        "-----------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 7;
                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    int numberOfColumns = 9;
                    for (int x = 0; x < numberOfColumns; x++)
                        rdTable.Columns.Add(new TableColumn());
                    currentRow = rdTable.RowGroups[0].Rows[1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Sr No."))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Acc No."))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depositer Name"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date Of Opening"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Last Credit Date"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Balance"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                    GridLength clmWidth = new GridLength(80, GridUnitType.Pixel);
                    rdTable.Columns[0].Width = clmWidth;
                    clmWidth = new GridLength(30, GridUnitType.Pixel);
                    rdTable.Columns[1].Width = clmWidth;
                    clmWidth = new GridLength(70, GridUnitType.Pixel);
                    rdTable.Columns[2].Width = clmWidth;
                    clmWidth = new GridLength(180, GridUnitType.Pixel);
                    rdTable.Columns[3].Width = clmWidth;
                    clmWidth = new GridLength(70, GridUnitType.Pixel);
                    rdTable.Columns[4].Width = clmWidth;
                    clmWidth = new GridLength(70, GridUnitType.Pixel);
                    rdTable.Columns[5].Width = clmWidth;
                    clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[6].Width = clmWidth;
                    clmWidth = new GridLength(60, GridUnitType.Pixel);
                    rdTable.Columns[7].Width = clmWidth;
                    clmWidth = new GridLength(50, GridUnitType.Pixel);
                    rdTable.Columns[8].Width = clmWidth;

                    currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                    currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                    currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                    currentRow.Cells[7].TextAlignment = TextAlignment.Right;

                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[2];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 7;


                    for (int i = 0; i < RDAccBalanceList.Count; i++)
                    {
                        int m = i + 1;
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(m.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(RDAccBalanceList[i].RDAccountNo))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(RDAccBalanceList[i].Name))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(data[i].DateOfOpening.Value.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(RDAccBalanceList[i].Date.ToShortDateString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(RDAccBalanceList[i].Amount))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(RDAccBalanceList[i].Balance.ToString()))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                        currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                        currentRow.Cells[4].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[5].TextAlignment = TextAlignment.Center;
                        currentRow.Cells[6].TextAlignment = TextAlignment.Right;
                        currentRow.Cells[7].TextAlignment = TextAlignment.Right;
                    }

                    btnPrint.IsEnabled = true;
                    rdTable.RowGroups[0].Rows.Add(new TableRow());
                    currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("-----------------------------------------------------------------------------------------------------------"))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                    currentRow.Cells[1].ColumnSpan = 7;

                    flwBalanceRegister.Blocks.Clear();
                    flwBalanceRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwBalanceRegister.FontSize = 12;
                }
                else
                {
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                    currentRow.Foreground = Brushes.Red;
                    flwBalanceRegister.Blocks.Clear();
                    flwBalanceRegister.Blocks.Add(rdTable);
                    rdTable.CellSpacing = 1;
                    flwBalanceRegister.FontSize = 12;
                    btnPrint.IsEnabled = false;
                }
            }
            catch
            {
                ToastNotification.Toast("Error", "Can't Load Data");
            }
        }
    }
}
