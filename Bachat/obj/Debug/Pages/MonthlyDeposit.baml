   M S B A M L   `   `   `  ����
  Bachat!    Bachat.Pages.MonthlyDeposit   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      $
��1206��6A   .��5      �    ContentRoot#��[  6   .+�5   
   ]� .+�5	      �� $	��Top=�6   $
��1180��6.   .+�5
      �� $%��Recurring Deposit Monthly Entry��6     Heading2#��[ 6C   $��0,0,0,8q�6e   � 5      $��28��6   $
��1180��6#     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      (  "System.Windows.Controls.DatePicker  -   5          Name$ dtpDate��5   !   $	��150��62   
 � Tag$ Select a Date��6>   6T   �� 5      -   5         ��Name$ cmbLotNo��5   I   {�.  $��1��6[   $��
17,0,944,0q�6k   ��61   �� 6k   �� 5      -   5         ��Name$ 	chkScreen��5      {�.  $��Print Preview��6D   $��106,0,774,0q�6\   $��1��6q   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  5      -   5          Name$ btnPrint��5   '      EllipseStrokeThickness$ 0Q�69   {�.   � ToolTip$ Print��6f   $��1��6v   $��223,0,477,0q�6�   	 � Cursor$
	 Hand��6�   
�������
   
IconHeight$
 40Q�6�      	IconWidth$ 20Q�6�     PrinterIconData   IconData# [ 6�   �� 5      -   5         ��Name$ btnAddMonthlyEntry��5      $��Add New��69   $��1��6K   $��
919,0,12,0q�6[   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��dtgRDRegister��5         AutoGenerateColumns .  $
��1180��6`   ��6,   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Lot No.��6G   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   �� *LotNo+6G     5      $ 
Account No��6M      
IsReadOnly .  6-   �� *RDAccountNo+6a     5      $
 Name��6F    .  6-   �� *Name+6T     5       $
 Date��6F    6-   �� *Date+6F     5!      $ Card No.��6H    6-   �� *CardNo+6H     5"      $ Amount��6H    6-   �� *Amount+6H     5#      $	 Due��6E    6-   �� *Due+6E     5$      $ Rebate��6H    6-   �� *Rebate+6H     5%      $ Balance��6I    6-   �� *	Balance+6I     5&      $ Remarks��6I    6-   �� *	Remarks+6I   4  .System.Windows.Controls.DataGridTemplateColumn  5'      $  ��6-      CellTemplate 5(      �� 5)      .R�5*   "     -   5        $ 0Q�5*   T   $ Delete��6o   $
	 Hand��6�   
���  ��  RowDeleteIconData# [ 6�   5+      5-      5/      ,� 50      -   5        $��flwScrollViewRDList��50   T   $y�Hidden=�6'   $	��500��6;   $	��800��6H   .��51      /� -	   5         �Name$ flwDocRDList��51       /� 	FontStyle$ Normal$�63    /� 
FontFamily$ Segoe UI)�6F   52      53      54      55      