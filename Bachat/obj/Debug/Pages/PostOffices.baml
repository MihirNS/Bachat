   M S B A M L   `   `   `  ����
  Bachat    Bachat.Pages.PostOffices   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5	      �    ContentRoot#��[  6   .+�5
   
   ]� $
��Left=�6   .+�5      �� $	��Top=�6   $	��400��6.   .+�5      �� $��Post Offices��6     Heading2#��[ 60   $��0,0,0,8q�6R   � 5      $��28��6     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      �� -   5         ��Name$ btnAddPostOffice��5      $��Add New��67   $��1��6I   $	��120��6Y   $
��Left=�6e   5      5      &   System.Windows.Controls.DataGrid  5      -   5        $��dtgPostOffices��5         AutoGenerateColumns .  $	��373��6a   ��6-   ��    Columns 5      =  7FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ SrNo.��6D      
Visibility$ Hidden=�6S      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   �� *ID+6g     5      $ Name of Post Office��6P    6-   �� *PostOfficeName+6P   4  .System.Windows.Controls.DataGridTemplateColumn  5      $  ��6-   	   CellTemplate	 5      �� 5      .R�5    "   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  -   5        
   EllipseStrokeThickness$
 0Q�5    l    � ToolTip$ Delete��6�    � Cursor$
 Hand��6�   
���  ��  RowDeleteIconData   IconData# [ 6�   
 � Tag 63   ��  �� Path$ ID�6�   5!      5#      5%      5&      5'      5(      