   M S B A M L   `   `   `  ����A  =Bachat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Bachat.AssetsBachat.Assets  5   @   %    Bachat.Assets.ToastNotification   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -Modelclr-namespace:Bachat.Assets     6@   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Notification��5   	   $	��120��6   $	��354��6+    � ShowActivated .   � AllowsTransparency .  � WindowStyle$
 None=�6g    � ShowInTaskbar .  
������  � Topmost .  � UseLayoutRounding .  � 	Resources 5         CollapseStoryboard W� From		 W� To
 �� TargetProperty g� Duration  MessageTemplate �� DataType �Name
 � Tag �� Path �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Opacity   
BlurRadius � RowDefinitions � ColumnDefinitions&   System.Windows.Media.TextOptions   TextRenderingMode   TextFormattingMode �� Triggers :� RoutedEvent :� 
SourceName ��Name �� 
TargetName g� 	BeginTime P� Property  P� Value! N� EnterActions" �� Offset# �� BeginStoryboardName$ N� ExitActions% P� 
SourceName& �� 
TargetName' �� Property( �� Value��%  &	        &	 g     7   �� 5   
   .��5	      W� $	 100X�6   $	 0X�6)   $
 Height�60   $ 0:0:1D�6S   5
      �� 5   
   $ Model:Notification��6/   .R�5      � $ NotificationWindow��6   -   5        
������  5   /   �� $ Id�.+�5      �� $��border��6   ���#2a3345$��0q�6<   $ 10��6P   $��10q�6b    5        5      $ 0Q�6+   $	 0.6Q�6;   $ 10Q�6I   ._�5      � $	��100��6   $	��300��6(    5      �� 5      $
��Auto �6,   6<   �� 5      $��* �6,   69    5      �� 5      $
��Auto �6/   6>   �� 5      $��* �6/   6;   .+�5      �� $��1��6$   $ 	ClearType=�6Q   $ Display=�6{   
�������$��Arial)�5   $   $��14(�67   $
��Bold"�6E   $��Center=�6W   $��	10,10,5,0q�6s   $
��Wrap=�6�   $��CharacterEllipsis=�6�   ��5   4   �� $ Title�5   �   �� 5      $��1��6$   $��1��61   $ 	ClearType=�6_   $ Display=�6�   
�������$��Arial)�5   $   $��Center=�67   $��
10,-10,0,0q�6R   $
��Wrap=�6f   $��CharacterEllipsis=�6z   ��5   A   �� $ Message�5   z   5      5      5        5!      :� 5"      $ Window.Loaded��6   $ NotificationWindow��6;   .[�5#      �� $ FadeInStoryBoard��6&   .��5$      �� .��5%      W� $ NotificationWindow��6.   $
 0.01X�6Y   $	 1X�6e   $
 Opacity�6l   $ 0:0:1D�6�   W� 5&      $ NotificationWindow��6.   $ 1X�6Y   $	 0X�6b   $
 Opacity�6i   $ 0:0:2D�6�   $ 0:0:2X�6�   5'      5(      5)      P� 5*       �@J�IsMouseOver  . ! 5+      �� 5,      $" 0:0:2h�6)   $# FadeInStoryBoard��68   ?� 5-      $# FadeInStoryBoard��6*   $ 5/      �� 50      $" 0:0:2h�6)   $# FadeInStoryBoard��68   �� 51      $# FadeInStoryBoard��6+   6T   53      P� 54      $% NotificationWindow��6    �@J�Opacity$  0Q�6M   . �55      �� $& NotificationWindow��6   ' � y�$( Hidden=�6S   6d   ! 56      �� 57      8��  6*   59      5;      5<      .��5=      �� -   5        ) ��Name$) NotificationsControl��5=      #��[ 6�   * � FocusVisualStyle* 61   Z� ��6�   �� 6�   5>      