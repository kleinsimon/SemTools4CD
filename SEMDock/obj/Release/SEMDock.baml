   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35L5clr-namespace:System.Diagnostics;assembly=WindowsBaseSystem.Diagnostics  F BSEMTools4CD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SEMTools4CDSEMTools4CD    SEMTools4CD.SEMDock   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      @d2http://schemas.microsoft.com/expression/blend/2008      Mdiagnostics5clr-namespace:System.Diagnostics;assembly=WindowsBase      +localclr-namespace:SEMTools4CD      -     >�Name$  _this��$	��Top=� J� 	AllowDrop .  � 	Resources .  (SEMTools4CD.BooleanToVisibilityConverter   bvc�  �SEMTools4CD.BooleanConverter`1[[System.Windows.Visibility, PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   True   False  nbvc)  #SEMTools4CD.InverseBooleanConverter  not+  %SEMTools4CD.InvertVisibilityConverter  ivis��%x   &	        &	 #     &	 F     &	 K       $ Visible=�$ 	Collapsed=�  $ 	Collapsed=�$ Visible=�    .���� $��0,0,0,0q�$	��Top=�.+��� $��	SEM-Tools��$	��5,2q�9� -    9�Name$ ExpanderMode��$��Create / Import��#��� �� 9� 
IsExpanded . $��5q�.���� -    ��Name$ tabMode�� �� SelectedIndex �� 	 �� Path$
	 Mode�.@��� $��	From File��.��]� .+��� $��30��$��Bottom=�$��Center=�
 �� Orientation$
 
Horizontal=�.+��� -   $��Browse Image��$	��5,2q�$
��Left=�$
��Auto���� -   $��Clear��$	��5,2q�$
��Left=�$
��Auto���� -    ��Name$ tiffList��$	��Top=�$	��Top=� . $	��100��$	��300�� � ToolTip$/ )Drop SEM-Images for automatic calibration�� �� View �� .P��� $��Filename�� �� Width$	 150�� �� DisplayMemberBinding �� *
filename+�� $��Factor�� �� *calibration+�� $��By Calibration��.��� ���	#FFE5E5E5 � ColumnDefinitions �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��Factor [µm/px]��$	��Top=�$	��5,2q�$7 1Calibration factor to use for creating micron bar���� $��0,0,0,0q�$��1��.+��� $��26�� �� TextWrapping$
 Wrap=�$	��Top=�$	��5,2q�$7 1Calibration factor to use for creating micron bar������ $	 Calibration�d� -    d�Name$ toggleCalib��$��Measure��$��Center=�$	��Top=�$��60��$	��5,2q�d� -   $ 
toggleList��$
��List��$��Center=�$	��Top=�$��60��$	��5,2q��� $��	By Length��.��� ���	#FFE5E5E5 �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��Bar length [cm]��$	��Top=�$	��5,2q�$  Fixed length of micron bar���� $��26��$��Title��$	��Top=�$	��5,2q�$ Fixed title of micron bar���� $��0,0,0,0q�$��1��.+��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$  Fixed length of micron bar������ $	 	BarLength��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$ Fixed title of micron bar������ $	 BarText�9� -	   $ expanderList��$��Calibration List��#��� �� . $��5q�y��� *	IsChecked+ �� ElementName$ 
toggleList�� �� 	Converter# [  .��]� .+��� $��30��$��Bottom=�$��Center=�$
 
Horizontal=�.+��� -
   $��Choose��$	��5,2q�$
��Left=�$
��Auto���� -   $��Add Current��$	��5,2q�$
��Left=�$
��Auto���� -   $��Delete��$	��5,2q�$
��Left=�$
��Auto���� -    ��Name$ changeItemName��$��30��$	��Top=�$��Center=�$
 
Horizontal=�$y�	Collapsed=�.+��� $��26��$
��Name��$	��Top=�$	��5,2q�$  Change name of the Element���� -    ��Name$ TextListViewName��$��26��$
 Wrap=�$	��100��$	��Top=�$	��5,2q�$  Change name of the Element������ $
	 Name� �� UpdateSourceTrigger$ Explicit=��� -   $ calibListView��$	��Top=�$	��Top=�$	��100��$	��300�� J� IsManipulationEnabled .  �� .P��� $
��Name�� �� *Name+�� $��Factor�� �� *Calibration+9� $��Measure Calibration��#��� �� . $��5q�y��� *	IsChecked+$ toggleCalib��# [  .��]� .+�� $	��Top=����	#FFE5E5E5 �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��	Direction��$	��5,2q��� $��26��$��Distance [µm]��$	��5,2q��� $��26��$��Measured [cm]��$	��5,2q��� $��26��$��Factor [µm/px]��$	��5,2q��� $��0,0,0,0q�$��1��.+��� -    ��Name$ CalibDirection��$��24��$	��5,1q�$��Center=�.@��� $��
Horizontal�� �� 
IsSelected . �� $��Vertical���� -   $ CalibRealWidth��$��26��$
 Wrap=�$�� ��$	��5,2q��� -   $ CalibMesWidth��$��26��$
 Wrap=�$	��5,2q� J� IsHitTestVisible .   �� 
IsReadOnly . �� -   $ CalibMesFactor��$��26��$
 Wrap=�$	��5,2q� .   . �� $��30��$��Bottom=�$��Center=�$
 
Horizontal=�.+��� -    ��Name$ ButStartMes��$��Start Measuring��$	��5,2q�$
��Left=�$
��Auto���� -   $ 
ButStopMes��$��Stop Measuring��$	��5,2q�$
��Left=�$
��Auto��$y�	Collapsed=��� -   $ ButApplyMes��$��Apply��$	��5,2q�$
��Left=�$
��Auto��{�.  9� $��
Properties��#��� ��$��5q� �� *	IsChecked+$ Explicit=�$ 
toggleList��# [ .���� .@��� $��
Dimensions��.��� ���	#FFE5E5E5 �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��
Width [cm]��$	��Top=�$	��5,2q�$# Default for publications 11cm���� $��26��$��Height [cm]��$	��Top=�$	��5,2q��� $��26��$��Cut Bottom [px]��$	��Top=�$	��5,2q��� $��26��$��Bar line [pt]��$	��Top=�$	��5,2q�$ Default 1.5 pt���� $��26��$��Border line [pt]��$	��Top=�$	��5,2q�$ Default 1.5 pt���� $��0,0,0,0q�$��1��.+��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$# Default for publications 11cm������ $	 Width��� $��26��$
 Wrap=�$	��Top=�$	��5,2q����� $	 Height��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$' !Cut image at bottom (data region)������ $	 	CutBottom��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$ Default 1.5pt������ $	 BarWidth��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$ Default 1.5pt������ $	 BorderWidth��� $��Annotations��.��� ���	#FFE5E5E5 �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��
Upper-Left��$	��Top=�$	��5,2q�$) #Annotation in the upper-left corner���� $��26��$��Upper-Right��$	��Top=�$	��5,2q�$* $Annotation in the upper-right corner���� $��26��$��
Lower-Left��$	��Top=�$	��5,2q�$) #Annotation in the lower-left corner���� $��26��$��Font-Size [pt]��$	��Top=�$	��5,2q�$ Default 10pt���� $��26��$
��Font��$	��Top=�$	��5,2q�$ Default 10pt���� $��0,0,0,0q�$��1��.+��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$) #Annotation in the upper-left corner������ $	 ULtext��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$* $Annotation in the upper-right corner������ $	 URtext��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$) #Annotation in the lower-left corner������ $	 BLtext��� $��26��$	��5,2q�$
 
Horizontal=�.+��� $
 Wrap=�$��0,0,5,0q�$��30��$ Default 10pt��$��26��$��Bottom=����� $	 FontSize��� $��B��$��1q�$	��2,5q�$
��Bold"�$ Default off�� d� 	IsChecked �� $	 TextBold�R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  �� SComponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase        FontTemplate#���   �� SelectedValue �� $
	 Font�   myFonts  �� Source   System.Windows.Media.Fonts!   SystemFontFamilies" �� Source# �� SortDescriptions+   %System.ComponentModel.SortDescription$   PropertyName  	FontStyle% �� Property& �� Value' $� IsVirtualizing��%�   &	       &	 +     &	 j     7  ��   �� #" Z! #   $$ Source���� .��� % � ��& �� *Source+�� % � ��$& 12(��� .R��� ' . .+��� 8��  ���� *Source+ �� *Source+���� #" [ �� $	��Bar��.��]� .+�� ���	#FFE5E5E5 �� $	��125 ��� $��* �.+��� $��0,0,0,0q�$��0��.+��� $��26��$��Min Width [cm]��$	��Top=�$	��5,2q�$A ;Minimum wdith of micron bar (when using calibration factor)���� $��26��$��Max Width [cm]��$	��Top=�$	��5,2q�$A ;Maximum wdith of micron bar (when using calibration factor)���� $��26��$��Value On Bar��$	��Top=�$	��5,2q�$0 *Display the Value on the Bar (not over it)���� $��26��$��Bar below Image��$	��Top=�$	��5,2q�$0 *Display the Value on the Bar (not over it)���� $��0,0,0,0q�$��1��.+��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$A ;Minimum wdith of micron bar (when using calibration factor)������ $	 BarMinWidth��� $��26��$
 Wrap=�$	��Top=�$	��5,2q�$A ;Maximum wdith of micron bar (when using calibration factor)������ $	 BarMaxWidth��� $�� ��$��1q�$	��5,2q�$ Default off��$��26��$��Center=� �� *
ValInBar+�� $�� ��$��1q�$	��5,2q�$ Default off��$��26��$��Center=� �� *BarBelowImage+�� $��30��$��Center=�$
 
Horizontal=�$��0,0,0,0q�.+��� -   $ 
SaveButton��$��Save Default��$	��5,2q�$
��Left=�$
��Auto���� -   $ SaveButton_Copy��$��Reset��$	��5,2q�$
��Left=�$
��Auto���� -   $��About��$	��5,2q��� -   $ CreateButton��$��Create/Save��$��0,15,0,0q�$��Center=�$
��Auto��( �� 	IsDefault( . $��26��