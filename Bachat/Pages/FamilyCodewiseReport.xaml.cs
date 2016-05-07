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
    /// Interaction logic for FamilyCodewiseReport.xaml
    /// </summary>
    public partial class FamilyCodewiseReport : UserControl
    {
        private PostOfficeDataContextDataContext _db = new PostOfficeDataContextDataContext(Bachat.Properties.Settings.Default.PostOfficeAccountManagementConnectionString);
        public FamilyCodewiseReport()
        {
            InitializeComponent();

            btnPrint.Click += btnPrint_Click;
            btnSubmit.Click += btnSubmit_Click;
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewFCRegister.Visibility = Visibility.Visible;
            showDataFamilyCodewise();
        }

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            flwScrollViewFCRegister.Print();
        }

        void showDataFamilyCodewise()
        {
            if (txtFamilyCode.Text != string.Empty)
            {
                try
                {
                    var rdRegister = (from p in _db.RecurringDepositCustomers where (p.FamilyCode == txtFamilyCode.Text) && p.ClosedAccount.Value == false && p.MaturityDate > DateTime.Now select p).ToList();
                    var NKMRegister = (from p in _db.NKMDepositers where (p.FamilyCode == txtFamilyCode.Text) && p.MaturityDate > DateTime.Now select p).ToList();

                    if (rdRegister.Count == 0 && NKMRegister.Count == 0)
                    {
                        Table rdTable = new Table();
                        rdTable.RowGroups.Add(new TableRowGroup());
                        rdTable.RowGroups[0].Rows.Add(new TableRow());
                        TableRow currentRow = rdTable.RowGroups[0].Rows[0];
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("No Data Found !!"))));
                        currentRow.Foreground = Brushes.Red;
                        btnPrint.IsEnabled = false;
                        flwFCRegister.Blocks.Clear();
                        flwFCRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwFCRegister.FontSize = 12;
                    }
                    else
                    {
                        Table rdTable = new Table();
                        rdTable.RowGroups.Add(new TableRowGroup());
                        TableRow currentRow = new TableRow();

                        int numberOfColumns = 9;
                        for (int x = 0; x < numberOfColumns; x++)
                            rdTable.Columns.Add(new TableColumn());

                        GridLength clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[0].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[1].Width = clmWidth;
                        clmWidth = new GridLength(60, GridUnitType.Pixel);
                        rdTable.Columns[2].Width = clmWidth;
                        clmWidth = new GridLength(170, GridUnitType.Pixel);
                        rdTable.Columns[3].Width = clmWidth;
                        clmWidth = new GridLength(70, GridUnitType.Pixel);
                        rdTable.Columns[4].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[5].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[6].Width = clmWidth;
                        clmWidth = new GridLength(80, GridUnitType.Pixel);
                        rdTable.Columns[7].Width = clmWidth;
                        clmWidth = new GridLength(50, GridUnitType.Pixel);
                        rdTable.Columns[8].Width = clmWidth;

                        if (rdRegister.Count > 0)
                        {
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nStatement Showing the Details for Family Code \n" +
                                "Family Code : " + txtFamilyCode.Text + "         Type : RD\n" +
                                "---------------------------------------------------------------------------------------------------------------------------"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;
                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());

                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Account No."))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Card No."))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gross AMT"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Opening Date"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Maturity Date"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Center;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------------------------"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;

                            for (int i = 0; i < rdRegister.Count; i++)
                            {
                                int m = i + 1;
                                rdTable.RowGroups[0].Rows.Add(new TableRow());
                                currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].RDAccountNo))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].CardNo))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].DepositerName))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].Amount))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].MaturityAmount))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].DateOfOpening.Value.ToShortDateString()))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(rdRegister[i].MaturityDate.Value.ToShortDateString()))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                                currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                                currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                                currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                                currentRow.Cells[6].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[7].TextAlignment = TextAlignment.Center;
                            }
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------------------------"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;
                        }

                        if (NKMRegister.Count > 0)
                        {
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\n\nStatement Showing the Details for Family Code\n" +
                               "Family Code : " + txtFamilyCode.Text + "         Type : NKM\n" +
                               "---------------------------------------------------------------------------------------------------------------------------"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;
                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Depo. No."))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Type"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Gross AMT"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Mat. Amount"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Opening Date"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Maturity Date"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                            currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                            currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                            currentRow.Cells[6].TextAlignment = TextAlignment.Center;
                            currentRow.Cells[7].TextAlignment = TextAlignment.Center;

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------------------------"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;

                            for (int i = 0; i < NKMRegister.Count; i++)
                            {
                                int m = i + 1;
                                rdTable.RowGroups[0].Rows.Add(new TableRow());
                                currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];

                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].DepositerNo))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].Scheme.TypeOfInvestment))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].DepositerName))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].Amount))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].MaturityAmount))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].DateOfDeposit.ToShortDateString()))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(NKMRegister[i].MaturityDate.ToShortDateString()))));
                                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));

                                currentRow.Cells[1].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[2].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[3].TextAlignment = TextAlignment.Left;
                                currentRow.Cells[4].TextAlignment = TextAlignment.Right;
                                currentRow.Cells[5].TextAlignment = TextAlignment.Right;
                                currentRow.Cells[6].TextAlignment = TextAlignment.Center;
                                currentRow.Cells[7].TextAlignment = TextAlignment.Center;
                            }

                            rdTable.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = rdTable.RowGroups[0].Rows[rdTable.RowGroups[0].Rows.Count - 1];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("---------------------------------------------------------------------------------------------------------------------------\n\n\n\n"))));
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].ColumnSpan = 7;
                        }

                        flwFCRegister.Blocks.Clear();
                        flwFCRegister.Blocks.Add(rdTable);
                        rdTable.CellSpacing = 1;
                        flwFCRegister.FontSize = 12;
                        btnPrint.IsEnabled = true;
                    }
                }
                catch
                {
                    ToastNotification.Toast("Error", "Can't load data");
                }
            }
            else
                ToastNotification.Toast("Error", "Please enter family code");
        }
    }
}
