   M S B A M L   `   `   `  ����B  >Torpedo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:TorpedoTorpedo  5   	       Torpedo.GameResult   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Torpedo     5   	     � Title$  
GameResult��5   	   $	��450��6   $	��800��6)   
������� � 
ResizeMode$ NoResize=�6H   ��5	      �� 5
   
    �� ImageSource$ Images/background.png��6   .��5      � $
��Left=�6   $	��392��6&   $��	10,10,0,0q�63   $	��Top=�6F   $	��759��6^    � ColumnDefinitions 5   
   �� 5      6!    � RowDefinitions 5   
   �� 5      $	��83* �6   6+   �� 5      $	��15* �6   6+   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ Results��5      $
��Left=�6@   $	��310��6[   $��	10,10,0,0q�6h   $	��Top=�6{   $	��739��6�      
IsReadOnly .    AutoGenerateColumns .  
����������	#FF509A9F
���   ���6$   �� *...+   RowStyle 5      �� 5      	 �� 
TargetType)  #System.Windows.Controls.DataGridRow"	  6   .�5      �� 
 �� Property
 � �� �� Value 6/   �� *	RowHeight+5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Győztes neve��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 5* 6V   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *winner+6c     5      $ Vesztes neve��6%   $ 5* 6U    6;   �� *loser+6b     5      $ Körök száma��6%   $ 4* 6Z    6:   �� *numOfRounds+6g     5      $ 	P1 Hajók��6%   $	 10* 6W    67   �� *p1ShipsSats+6e     5      $ 	P2 Hajók��6%   $	 10* 6W    67   �� *p2ShipsSats+6e   5!      �� 5"   
   -   5        $��	Kilépés��5"      $
��Left=�64   $��
260,10,0,0q�6O   $��1��6c   $	��Top=�6p   $	��106��6�   $��40��6�   $��Tahoma)�6�   ���	#FF509A9F���	#FF509A9F
�������6�   �� 5#   
   -   5        $��Új játék��5#      $
��Left=�64   $��
420,10,0,0q�6O   $��1��6c   $	��Top=�6p   $	��106��6�   $��40��6�   $��Tahoma)�6�   ���	#FF509A9F���	#FF509A9F
�������6�   5$      5%      