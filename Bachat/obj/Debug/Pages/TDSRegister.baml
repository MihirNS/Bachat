   M S B A M L   `   `   `  ����
  Bachat    Bachat.Pages.TDSRegister   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.6.0, Culture=neutral, PublicKeyToken=2d21ec3cd074c59a0mui&http://firstfloorsoftware.com/ModernUI  5      .��5      �    ContentRoot#��[  6   .+�5   
   ]� .+�5	      �� $	��Top=�6   $
��1180��6/   .+�5
      �� $��TDS Register��6     Heading2#��[ 60   $��0,0,0,8q�6R   � 5      $��30��6   $
��1180��6#     � RowDefinitions  5      �� 5      $
��Auto �6(   68   �� 5      $��* �6(   65    � ColumnDefinitions 5      �� 5      $
��Auto �6+   6:   �� 5      $��* �6+   67   .+�5      �� -   5         ��Name$ cmbSchemeType��5   J    �� 
IsEditable .  $	��100��6a   $��0,1,0,2q�6m   ��62   �� 6�   �� 5      -   5        $ cmbReportType��5   \    .  {�.  $��1��6s   $��360,1,557,2q�6�   ��6D   �� 6�   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dtpTDSFromDate��5   !   {�.  
 � Tag$ 	From Date��6K   $	��150��6[   $��1��6g   $��
21,1,909,2q�6w     5      -   5        $ dtpTDSToDate��5   !   {�.  $ To Date��6I   $	��150��6W   $��1��6c   $��192,0,738,1q�6s   �� 5      -   5        $ cmbPostOffice��5   ^    .  $y�Hidden=�62   $��1��6u   $��542,1,392,3q�6�   ��6F   �� 6�   �� 5      -   5         ��Name$ 	btnSubmit��5      {�.  $��Submit��6B   $��1��6S   $��706,2,272,2q�6c   7  1FirstFloor.ModernUI.Windows.Controls.ModernButton  5      -   5          Name$ btnPrint��5   '      EllipseStrokeThickness$ 0Q�69   {�.  	 � ToolTip$	 Print��6f   $��1��6v   $��	881,0,0,0q�6�   
 � Cursor$

 Hand��6�   
�������   
IconHeight$ 40Q�6�      	IconWidth$ 20Q�6�     PrinterIconData   IconData# [ 6�   5      5      ,� 5      -   5        $��flwScrollViewTDSRegister��5   @   $	��500��6'   $	��810��64   $y�Hidden=�6`   .��5      /� -	   5         �Name$ flwTDSRegister��5       /� 	FontStyle$ Normal$�65    /� 
FontFamily$ Segoe UI)�6H   5      5       5!      5"      