   M S B A M L   `   `   `  ����
  Bachat$    Bachat.Pages.UpdateSchemeRates   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5	      �    ContentRoot#��[  6   .+�5
   
   ]� $
��Left=�6   .+�5      �� $	��Top=�6   $	��800��6.   .+�5      �� $��Update Scheme Rates��6     Heading2#��[ 67   $��0,0,0,8q�6Y   � 5      $��28��6     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      �� -   5         ��Name$ 
cmbSchemes��5      $��1��6K   $	��120��6[   $
��Left=�6g   ��63   �� 6g   �� 5      -   5         ��Name$ 
btnNewRate��5      {�.  $��New Rate��6C   $��1��6V   $	��120��6f   $��Right=�6r   $��0,0,67,0q�6�   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��dtgUpdateRates��5         AutoGenerateColumns .  $	��800��6a   ��6-   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   
Visibility$ Hidden=�6D   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   �� *ID+6D     5      $ Hidden=�6R    6-   �� *TypeOfInvestment+6R     5         Header$ 	From Date��6J    6-   �� *
FromDate+6J     5      $ To Date��6H    6-   �� *ToDate+6H     5       $ Amount��6H    6-   �� *Amount+6H     5!      $ Interest + Amount��6T    6-   �� *InterestWithAmount+6T     5"      $
 Year��6F    6-   �� *Year+6F     5#      $ Month��6G    6-   �� *Month+6G     5$      $	 Day��6E    6-   �� *Day+6E   4  .System.Windows.Controls.DataGridTemplateColumn  5%      $  ��6-   	   CellTemplate	 5&      �� 5'      .R�5(   "   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5        
   EllipseStrokeThickness$
 0Q�5(   l    � ToolTip$ Delete��6�    � Cursor$
 Hand��6�   
���  ��  RowDeleteIconData   IconData# [ 6�   
 � Tag 63   ��  �� Path$ ID�6�   5)      5+      5-      5.      5/      50      