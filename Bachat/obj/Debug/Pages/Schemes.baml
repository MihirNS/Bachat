   M S B A M L   `   `   `  ����
  Bachat    Bachat.Pages.Schemes   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5	      �    ContentRoot#��[  6   .+�5
   
   ]� $
��Left=�6   .+�5      �� $	��Top=�6   $	��400��6.   .+�5      �� $��Schemes��6     Heading2#��[ 6+   $��0,0,0,8q�6M   5      &   System.Windows.Controls.DataGrid  5      -   5        $��
dtgSchemes��5          AutoGenerateColumns  .  $	��373��6]   ��6)   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ SrNo.��6J      
Visibility$ Hidden=�6Y      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   �� *
SchemeNo+6m     5      $ Scheme Name��6R    6-   �� *TypeOfInvestment+6R   4  .System.Windows.Controls.DataGridTemplateColumn  5      $  ��6-      CellTemplate 5      �� 5      .R�5   "   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5           EllipseStrokeThickness$ 0Q�5   r    � ToolTip$ Delete��6�   	 � Cursor$
	 Hand��6�   
���  ��  RowDeleteIconData
   IconData#
 [ 6�   
 � Tag 63   ��  �� Path$ SchemeNo�6�   5      5      5      5      5      5      