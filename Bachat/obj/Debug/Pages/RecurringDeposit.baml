   M S B A M L   `   `   `  ����
  Bachat#    Bachat.Pages.RecurringDeposit   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5      �    ContentRoot#��[  6   .+�5   
   ]� .+�5	      �� $	��Top=�6   $
��1180��6.   .+�5
      �� $��Recurring Deposit��6     Heading2#��[ 65   $��0,0,0,8q�6W   � 5      $��28��6   $
��1180��6#     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      �� -   5         ��Name$ cmbSearchType��5   I    �� 
IsEditable . $	��200��6`   ��61   �� 6n   �� 5      -   5        $ cmbSearchValue��5   I   {�.  $	��200��6a   $��1��6m   $��
20,0,760,0q�6}   ��61   �� 6}   �� 5      -   5         ��Name$ btnClear��5      $��Clear Filter��6/   {�.  $��243,0,637,0q�6X   $��1��6m   �� 5      -   5        $ btnAddRdCustomer��5      $��Add New��67   $��1��6h   $��	881,0,0,0q�6x   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��dtgRDCustomer��5         AutoGenerateColumns .  $
��1180��6`   ��6,   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Account No.��6M      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6-   �� *RDAccountNo+6b     5      $ Family Code��6L   	 6-   �� *
FamilyCode+6L     5      $ Depositer Name��6O   	 6-   �� *DepositerName+6O     5      $ Amount��6H   	 6-   �� *Amount+6H     5       $ Balance��6I   	 6-   �� *	Balance+6I   A  ;FirstFloor.ModernUI.Windows.Controls.DataGridCheckBoxColumn  5!      $ Closed��6S   	 61   �� *ClosedAccount+6S     5"      $ Closing Date��6l   	 6-   �� *ClosingDate+
 �� StringFormat$
 {0:MM/dd/yyyy}��6l     5#      $ LastCreditDate��6o   	 6-   �� *LastCreditDate+$
 {0:MM/dd/yyyy}��6o     5$      $ NextDueDate��6l   	 6-   �� *NextDueDate+$
 {0:MM/dd/yyyy}��6l   4  .System.Windows.Controls.DataGridTemplateColumn  5%      $  ��6-      CellTemplate 5&      5  -System.Windows.Controls.ItemContainerTemplate�  5'      .R�5(   "   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5           EllipseStrokeThickness$ 0Q�5(   s    � ToolTip$
 Edit��6�    � Cursor$
 Hand��6�   
�������  RowEditIconData   IconData# [ 6�   
 � Tag 6R   ��  �� Path$ RDAccountNo�6�   5)      5+        5,      $  ��6-    5-      �� 5.      .R�5/   "     -   5        $ 0Q�5/   u   $ Delete��6�   $
 Hand��6�   
���  ��  RowDeleteIconData# [ 6�    6T   �� $ RDAccountNo�6�   50      52      54      55      56      57      