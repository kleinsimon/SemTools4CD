   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35L5clr-namespace:System.Diagnostics;assembly=WindowsBaseSystem.Diagnostics  5      F BSEMTools4CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SEMTools4CDSEMTools4CD 5         SEMTools4CD.SEMDock   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      5      @d2http://schemas.microsoft.com/expression/blend/2008      5      Mdiagnostics5clr-namespace:System.Diagnostics;assembly=WindowsBase      5      +localclr-namespace:SEMTools4CD      5      -   5          >�Name$  _this��5	      $	��Top=�5   3    J� 	AllowDrop .  � 	Resources 5
      .  (SEMTools4CD.BooleanToVisibilityConverter   bvc�  �SEMTools4CD.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False  nbvc)  #SEMTools4CD.InverseBooleanConverter  not+  %SEMTools4CD.InvertVisibilityConverter  ivis��%�   &	        &	 6     &	 l     &	        5   
   $ Visible=�69   $ 	Collapsed=�6H     5   
   $ 	Collapsed=�6:   $ Visible=�6K     5   
   6(     5   
   6*   5      .��5      �� $��0,0,0,0q�6   $	��Top=�6"   .+�5   
   �� $��	SEM-Tools��6   $	��5,2q�6$   9� 5   
   -   5         9�Name$ ExpanderMode��5      $��Create / Import��6)   #��� ��6B    9� 
IsExpanded . $��5q�6�   .��5      �� -   5         ��Name$ tabMode��5   =    �� SelectedIndex 6   �� 	 �� Path$
	 Mode�.@�5      �� $��By TIFF��6   .��5      ]� .+�5      �� $��30��6%   $��Bottom=�62   $��Center=�6J   
 �� Orientation$
 
Horizontal=�6g   .+�5      �� -   5        $��Browse TIFF��5   %   $	��5,2q�6;   $
��Left=�6H   $
��Auto��6c   6p   �� 5      -   5        $��Clear��5   %   $	��5,2q�65   $
��Left=�6B   $
��Auto��6]   6j   5      �� 5      -   5         ��Name$ tiffList��5   #   $	��Top=�65   $	��Top=�6T    . $	��100��6z   $	��300��6�    � ToolTip$C =Drop TIFF-Images taken with LEO SEM for automatic calibration��6�    �� View 5      �� 5   "   .P�5   &   �� $��Filename��6_    �� Width$	 150��6q    �� DisplayMemberBinding 65   �� *
filename+6q   �� 5   &   $��Factor��6b    65   �� *calibration+6b   5   #   5!      5"      5#      �� 5$      $��By Calibration��6   .��5%      � ���	#FFE5E5E5 � ColumnDefinitions 5&      �� 5'      $	��125 �6/   �� 5(      $��* �6/   .+�5*      �� $��0,0,0,0q�6%   $��0��66   .+�5+      �� $��26��6$   $��Factor [µm/px]��60   $	��Top=�6I   $	��5,2q�6a   $7 1Calibration factor to use for creating micron bar��6n   5,      �� 5-      $��0,0,0,0q�6%   $��1��66   .+�5.      �� $��26��6&    �� TextWrapping$
 Wrap=�62   $	��Top=�6h   $	��5,2q�6�   $7 1Calibration factor to use for creating micron bar��6�   ��6F   �� $	 Calibration�6�   d� 5/      -   5         d�Name$ toggleCalib��5/   +   $��Measure��6@   $��Center=�6R   $	��Top=�6o   $��60��6�   $	��5,2q�6�   d� 50      -   5        $ 
toggleList��50   +   $
��List��6?   $��Center=�6N   $	��Top=�6k   $��60��6�   $	��5,2q�6�   51      52      53      �� 54      $��	By Length��6   .��55      � ���	#FFE5E5E5 56      �� 57      $	��125 �6/   �� 58      $��* �6/   .+�5:      �� $��0,0,0,0q�6%   $��0��66   .+�5;      �� $��26��6$   $��Bar length [cm]��60   $	��Top=�6J   $	��5,2q�6b   $  Fixed length of micron bar��6o   �� 5<      $��26��6$   $��Title��60   $	��Top=�6@   $	��5,2q�6X   $ Fixed title of micron bar��6e   5=      �� 5>      $��0,0,0,0q�6%   $��1��66   .+�5?      �� $��26��6&   $
 Wrap=�62   $	��Top=�6f   $	��5,2q�6~   $  Fixed length of micron bar��6�   ��6F   �� $	 	BarLength�6�   �� 5@      $��26��6&   $
 Wrap=�62   $	��Top=�6d   $	��5,2q�6|   $ Fixed title of micron bar��6�   ��6F   �� $	 BarText�6�   5A      5B      5C      5E      5F      9� 5G   
   -	   5        $ expanderList��5G      $��Calibration List��6)   #��� ��6C    . $��5q�6�   y�6�   �� *	IsChecked+ �� ElementName$ 
toggleList�� �� 	Converter# [  .��5H      ]� .+�5I      �� $��30��6   $��Bottom=�6*   $��Center=�6B   $
 
Horizontal=�6_   .+�5J      �� -
   5        $��Choose��5J      $	��5,2q�6.   $
��Left=�6;   $
��Auto��6V   6c   �� 5K      -   5        $��Add Current��5K      $	��5,2q�63   $
��Left=�6@   $
��Auto��6[   6h   �� 5L      -   5        $��Delete��5L      $	��5,2q�6.   $
��Left=�6;   $
��Auto��6V   6c   5M      �� 5N      -   5         ��Name$ changeItemName��5N      $��30��65   $	��Top=�6B   $��Center=�6W   $
 
Horizontal=�6t   $y�	Collapsed=�6�   .+�5O      �� $��26��6   $
��Name��6(   $	��Top=�67   $	��5,2q�6O   $  Change name of the Element��6\   �� 5P      -   5         ��Name$ TextListViewName��5P      $��26��6q   $
 Wrap=�6}   $	��100��6�   $	��Top=�6�   $	��5,2q�6�   $  Change name of the Element��6�   ��68   �� $
	 Name� �� UpdateSourceTrigger$ Explicit=�6�   5Q      �� 5R      -   5        $ calibListView��5R      $	��Top=�62   $	��Top=�6Q   $	��100��6f   $	��300��6v    J� IsManipulationEnabled .  5S      �� 5T      .P�5U      �� $
��Name��6S    6-   �� *Name+6S   �� 5V      $��Factor��6Z    6-   �� *Calibration+6Z   5W      5Y      5Z      5[      9� 5\   
   $��Measure Calibration��6   #��� ��60    . $��5q�6�   y�6�   �� *	IsChecked+$ toggleCalib��# [  .��5]      ]� .+�5^      � $	��Top=�6   ���	#FFE5E5E5 5_      �� 5`      $	��125 �6+   �� 5a      $��* �6+   .+�5c      �� $��0,0,0,0q�6!   $��0��62   .+�5d      �� $��26��6    $��	Direction��6,   $	��5,2q�6@   �� 5e      $��26��6    $��Distance [µm]��6,   $	��5,2q�6D   �� 5f      $��26��6    $��Measured [cm]��6,   $	��5,2q�6D   �� 5g      $��26��6    $��Factor [µm/px]��6,   $	��5,2q�6E   5h      �� 5i      $��0,0,0,0q�6!   $��1��62   .+�5j      �� -   5         ��Name$ CalibDirection��5j   #   $��24��6;   $	��5,1q�6G   $��Center=�6T   .@�5k      �� $��
Horizontal��6+    �� 
IsSelected . 6@   �� 5l      $��Vertical��6+   5m      �� 5n      -   5        $ CalibRealWidth��5n   "   $��26��6:   $
 Wrap=�6F   $�� ��6Z   $	��5,2q�6b   �� 5o      -   5        $ CalibMesWidth��5o   "   $��26��69   $
 Wrap=�6E   $	��5,2q�6Y    J� IsHitTestVisible .   �� 
IsReadOnly . 6   �� 5p      -   5        $ CalibMesFactor��5p   "   $��26��6:   $
 Wrap=�6F   $	��5,2q�6Z    .   . 6�   5q      5r      �� 5s      $��30��6   $��Bottom=�6*   $��Center=�6B   $
 
Horizontal=�6_   .+�5t      �� -   5         ��Name$ ButStartMes��5t      $��Start Measuring��62   $	��5,2q�6L   $
��Left=�6Y   $
��Auto��6t   6�   �� 5u      -   5        $ 
ButStopMes��5u      $��Stop Measuring��61   $	��5,2q�6J   $
��Left=�6W   $
��Auto��6r   $y�	Collapsed=�6�   �� 5v      -   5        $ ButApplyMes��5v      $��Apply��62   $	��5,2q�6B   $
��Left=�6O   $
��Auto��6j   {�.  6�   5w      5x      5y      9� 5z   
   $��
Properties��6   #��� ��6'   $��5q�6�    6r   �� *	IsChecked+$ Explicit=�$ 
toggleList��# [ .��5{      �� .@�5|      �� $��
Dimensions��6   .��5}      � ���	#FFE5E5E5 5~      �� 5      $	��125 �6/   �� 5�      $��* �6/   .+�5�      �� $��0,0,0,0q�6%   $��0��66   .+�5�      �� $��26��6$   $��
Width [cm]��60   $	��Top=�6E   $	��5,2q�6]   $# Default for publications 11cm��6j   �� 5�      $��26��6$   $��Height [cm]��60   $	��Top=�6F   $	��5,2q�6^   �� 5�      $��26��6$   $��Bar line [pt]��60   $	��Top=�6H   $	��5,2q�6`   $ Default 1.5 pt��6m   �� 5�      $��26��6$   $��Border line [pt]��60   $	��Top=�6K   $	��5,2q�6c   $ Default 1.5 pt��6p   5�      �� 5�      $��0,0,0,0q�6%   $��1��66   .+�5�      �� $��26��6&   $
 Wrap=�62   $	��Top=�6b   $	��5,2q�6z   $# Default for publications 11cm��6�   ��6F   �� $	 Width�6�   �� 5�      $��26��6&   $
 Wrap=�62   $	��Top=�6c   $	��5,2q�6{   ��6F   �� $	 Height�6{   �� 5�      $��26��6&   $
 Wrap=�62   $	��Top=�6e   $	��5,2q�6}   $ Default 1.5pt��6�   ��6F   �� $	 BarWidth�6�   �� 5�      $��26��6&   $
 Wrap=�62   $	��Top=�6h   $	��5,2q�6�   $ Default 1.5pt��6�   ��6F   �� $	 BorderWidth�6�   5�      5�      5�      �� 5�      $��Annotations��6   .��5�      � ���	#FFE5E5E5 5�      �� 5�      $	��125 �6/   �� 5�      $��* �6/   .+�5�      �� $��0,0,0,0q�6%   $��0��66   .+�5�      �� $��26��6$   $��
Upper-Left��60   $	��Top=�6E   $	��5,2q�6]   $) #Annotation in the upper-left corner��6j   �� 5�      $��26��6$   $��Upper-Right��60   $	��Top=�6F   $	��5,2q�6^   $* $Annotation in the upper-right corner��6k   �� 5�      $��26��6$   $��
Lower-Left��60   $	��Top=�6E   $	��5,2q�6]   $) #Annotation in the lower-left corner��6j   �� 5�      $��26��6$   $��Font-Size [pt]��60   $	��Top=�6I   $	��5,2q�6a   $ Default 10pt��6n   �� 5�      $��26��6$   $
��Font��60   $	��Top=�6?   $	��5,2q�6W   $ Default 10pt��6d   5�      �� 5�      $��0,0,0,0q�6%   $��1��66   .+�5�      �� $��26��6&   $
 Wrap=�62   $	��Top=�6c   $	��5,2q�6{   $) #Annotation in the upper-left corner��6�   ��6F   �� $	 ULtext�6�   �� 5�      $��26��6&   $
 Wrap=�62   $	��Top=�6c   $	��5,2q�6{   $* $Annotation in the upper-right corner��6�   ��6F   �� $	 URtext�6�   �� 5�      $��26��6&   $
 Wrap=�62   $	��Top=�6c   $	��5,2q�6{   $) #Annotation in the lower-left corner��6�   ��6F   �� $	 BLtext�6�   �� 5�      $��26��6)   $	��5,2q�65   $
 
Horizontal=�6B   .+�5�   "   �� $
 Wrap=�6*   $��0,0,5,0q�6]   $��30��6n   $ Default 10pt��6y   $��26��6�   $��Bottom=�6�   ��6>   �� $	 FontSize�6�   �� 5�   "   $��B��6O   $��1q�6[   $	��2,5q�6o   $
��Bold"�6|   $ Default off��6�    d� 	IsChecked 6+   �� $	 TextBold�6�   5�      R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  5�      �� 5�      SComponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase      5�        FontTemplate#���  5�       �� SelectedValue 69   �� $
	 Font� 5�   "     myFonts  �� Source   System.Windows.Media.Fonts!   SystemFontFamilies" �� Source# �� SortDescriptions+   %System.ComponentModel.SortDescription$   PropertyName  	FontStyle% �� Property& �� Value' $� IsVirtualizing��%p  &	       &	 Y     &	 �     7  �� 5�   &     6K   �� #" Z! # 5�   *     5�   .   $$ Source��6M   5�   '   �� 5�   &   .�5�   *   �� % � ��& 6O   �� *Source+�� 5�   *   % � ��$& 12(�6M   5�   '   �� 5�   &   .R�5�   *   �� ' . .+�5�   .   �� 8��  ��5�      �� *Source+ 5�      �� *Source+5�   +   5�   '   5�   #   ��5�   "   �� 5�   &   #" [ 6.   5�      5�      5�      5�      �� 5�      $	��Bar��6   .��5�      ]� .+�5�      � ���	#FFE5E5E5 5�      �� 5�   "   $	��125 �63   �� 5�   "   $��* �63   .+�5�      �� $��0,0,0,0q�6)   $��0��6:   .+�5�   "   �� $��26��6(   $��Min Width [cm]��64   $	��Top=�6M   $	��5,2q�6e   $A ;Minimum wdith of micron bar (when using calibration factor)��6r   �� 5�   "   $��26��6(   $��Max Width [cm]��64   $	��Top=�6M   $	��5,2q�6e   $A ;Maximum wdith of micron bar (when using calibration factor)��6r   �� 5�   "   $��26��6(   $��Value On Bar��64   $	��Top=�6K   $	��5,2q�6c   $0 *Display the Value on the Bar (not over it)��6p   5�      �� 5�      $��0,0,0,0q�6)   $��1��6:   .+�5�   "   �� $��26��6*   $
 Wrap=�66   $	��Top=�6l   $	��5,2q�6�   $A ;Minimum wdith of micron bar (when using calibration factor)��6�   ��6J   �� $	 BarMinWidth�6�   �� 5�   "   $��26��6*   $
 Wrap=�66   $	��Top=�6l   $	��5,2q�6�   $A ;Maximum wdith of micron bar (when using calibration factor)��6�   ��6J   �� $	 BarMaxWidth�6�   �� 5�   "   $�� ��6J   $��1q�6U   $	��5,5q�6i   $ Default off��6v    6+   �� *
ValInBar+6v   5�      5�      5�      5�      5�      5�      �� 5�   
   $��30��6   $��Center=�6!   $
 
Horizontal=�6>   $��0,0,0,0q�6W   .+�5�      �� -   5        $ 
SaveButton��5�      $��Save Default��6)   $	��5,2q�6@   $
��Left=�6M   $
��Auto��6h   6u   �� 5�      -   5        $ SaveButton_Copy��5�      $��Reset��6.   $	��5,2q�6>   $
��Left=�6K   $
��Auto��6f   6s   �� 5�      -   5        $��About��5�      $	��5,2q�6%   62   5�      �� 5�   
   -   5        $ CreateButton��5�      $��Create/Save��6'   $��0,15,0,0q�6=   $��Center=�6O   $
��Auto��6l   ( �� 	IsDefault( . $��26��6�   5�      5�      