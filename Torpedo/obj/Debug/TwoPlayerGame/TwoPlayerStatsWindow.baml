   M S B A M L   `   `   `  ����B  >Torpedo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:TorpedoTorpedo  5   	   "    Torpedo.TwoPlayerStatsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Torpedo     5   	     � Title$  TwoPlayerStatsWindow��5   	   $	��296��6&   $	��600��63    � 
ResizeMode$ NoResize=�6?   ��5	      �� 5
   
    �� ImageSource$9 3/Torpedo;component/Images/statisztikaBackground.png��6   .��5      � $��3,3,3,0q�6    � RowDefinitions 5   
   �� 5      $	��41* �6   �� 5      $	��28* �6   �� 5      $	��32* �6   �� 5      $	��32* �6   �� 5      $	��49* �6   �� 5      $	��49* �6   .+�5   
   �� -   5         ��Name$ nextPlayerLabel��5      $$��The players build their ships.��6)   $��0��6R   $
��Left=�6_   $��Center=�6{   
�������$��16(�6�   $��Tahoma)�6�   $��32��6�   $��0,2,0,7q�6�   $	��461��6�   $
��Bold"�6�   �� 5   
   -   5        $ roundCounterLabel��5      $��Round: ��6+   $��1��6=   $
��Left=�6J   $��Center=�6f   
�������$��13(�6�   $��Tahoma)�6�   $��26��6�   $	��0,1q�6�   $	��300��6�   �� 5   
   -   5        $ p1HitCounterLabel��5      $��P1Hits:��6+   $��2��6=   $
��Left=�6J   $��Center=�6f   
�������$��13(�6�   $��Tahoma)�6�   $��26��6�   $	��0,3q�6�   $	��300��6�   �� 5   
   -   5        $ p2HitCounterLabel��5      $��P2Hits��6+   $��3��6<   $
��Left=�6I   $��Center=�6e   
�������$��13(�6�   $��Tahoma)�6�   $��26��6�   $	��0,3q�6�   $	��300��6�   �� 5   
   -   5        $ p1ShipsSatusLabel��5      $��P1Ships��6+   $��4��6=   $
��Left=�6J   $��Center=�6e   
�������$��13(�6�   $��Tahoma)�6�   $��50��6�   $��0,5,0,5q�6�   $	��420��6�   �� 5   
   -   5        $ p2ShipsSatusLabel��5      $��P2Ships��6+   $��5��6=   $
��Left=�6J   $��Center=�6e   
�������$��13(�6�   $��Tahoma)�6�   $��50��6�   $��0,5,0,5q�6�   $	��420��6�   5      5      