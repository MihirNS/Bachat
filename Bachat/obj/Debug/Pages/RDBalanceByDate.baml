   M S B A M L   `   `   `  ����
  Bachat"    Bachat.Pages.RDBalanceByDate   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5      �    ContentRoot#��[  6   .+�5   
   ]� .+�5	      �� $	��Top=�6   $
��1180��6/   .+�5
      �� $%��Recurring Deposit Balance As On��6     Heading2#��[ 6C   $��0,0,0,8q�6e   � 5      $��28��6   $
��1180��6#     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      (  "System.Windows.Controls.DatePicker  -   5          Name$ dtpBalanceFromDate��5   !   
 � Tag$ 	From Date��6=   $	��150��6M   $��1��6Y   $��21,-2,909,3q�6i   $��2��6~     5      -   5        $ dtpBalanceToDate��5   !   {�.  $ To Date��6M   $	��150��6[   $��1��6g   $��192,-3,738,0q�6w   �� 5      -   5         ��Name$ 	btnSubmit��5      {�.  $��Submit��6B   $��1��6S   $��413,-2,667,2q�6c   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  5      -   5          Name$ btnPrint��5   '      EllipseStrokeThickness$ 0Q�69   {�.   � ToolTip$ Print��6f   $��1��6v   $��	881,0,0,0q�6�    � Cursor$
 Hand��6�   
�������	   
IconHeight$	 40Q�6�   
   	IconWidth$
 20Q�6�     PrinterIconData   IconData# [ 6�   5      5      ,� 5      -   5        $"��flwScrollViewBalanceRegister��5   @   $	��500��6'   $	��810��64   $y�Hidden=�6d   .��5      /� -   5         �Name$ flwBalanceRegister��5       /� 	FontStyle$ Normal$�69    /� 
FontFamily$ Segoe UI)�6L   5      5      5      5      