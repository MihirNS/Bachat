   M S B A M L   `   `   `  ����
  Bachat    Bachat.Pages.NKMDeposit   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5      �    ContentRoot#��[  6   .+�5   
   ]� .+�5	      �� $	��Top=�6   $
��1180��6.   .+�5
      �� $��NSC / KVP / MIS��6     Heading2#��[ 63   $��0,0,0,8q�6U   � 5      $��28��6   $
��1180��6#     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      �� -   5         ��Name$ cmbSearchType��5   I    �� 
IsEditable . $	��200��6`   ��61   �� 6n   �� 5      -   5        $ cmbSearchValue��5   I   {�.  $	��200��6a   $��1��6m   $��
20,0,760,0q�6}   ��61   �� 6}   �� 5      -   5         ��Name$ btnClear��5      $��Clear Filter��6/   {�.  $��243,0,637,0q�6X   $��1��6m   �� 5      -   5        $ btnAddDepositCustomer��5      $��Add New��6<   $��1��6r   $��	881,0,0,0q�6�   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��dtgDepositers��5   K      AutoGenerateColumns .  $
��1180��6`   ��63   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Depo. No.��6M      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6-   �� *DepositerNo+6`   0  *System.Windows.Controls.DataGridTextColumn  5      
   
Visibility$
 Hidden=�6N    . 	 6)   �� *TypeOfInvestment+6b     5      $
 Name��6K   	 6)   �� *DepositerName+6K     5      $ Family Code��6H   	 6)   �� *
FamilyCode+6H     5       $ Amount��6D   	 6)   �� *Amount+6D     5!      $ Maturity Amount��6L    . 	 6)   �� *MaturityAmount+6e     5"      $ Maturity Date��6i   	 6)   �� *MaturityDate+ �� StringFormat$ {0:MM/dd/yyyy}��6i     5#       . $ Scheme��6g   	 6)   �� *Scheme.TypeOfInvestment+6g     5$      $ Date Of Deposit��6j   	 6)   �� *DateOfDeposit+$ {0:MM/dd/yyyy}��6j   4  .System.Windows.Controls.DataGridTemplateColumn  5%      $  ��6-      CellTemplate 5&      5  -System.Windows.Controls.ItemContainerTemplate�  5'      .R�5(   "   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5           EllipseStrokeThickness$ 0Q�5(   s    � ToolTip$
 Edit��6�    � Cursor$
 Hand��6�   
�������  RowEditIconData   IconData# [ 6�   
 � Tag 6R   ��  �� Path$ DepositerNo�6�   5)      5+        5,      $  ��6-    5-      �� 5.      .R�5/   "     -   5        $ 0Q�5/   u   $ Delete��6�   $
 Hand��6�   
���  ��  RowDeleteIconData# [ 6�    6T   �� $ DepositerNo�6�   50      52      54      55      56      57      