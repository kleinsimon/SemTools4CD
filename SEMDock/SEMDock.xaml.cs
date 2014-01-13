﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;
using System.IO;
using CorelDRAW;
using VGCore;

namespace SEMTools4CD
{
    enum ShapeProperties
    {
        isLatexObject = 1,
        latexText = 2,
        latexTemplate = 3,
    }

    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class SEMDock : UserControl
    {
        CorelDRAW.Application CDWin;
        List<semImage> editShapes = new List<semImage>();
        semImageData _currentSettings;
        semImageData currentSettings
        {
            get { return _currentSettings; }
            set
            {
                _currentSettings = value;
                this.DataContext = _currentSettings;
            }
        }
        semImageData Settings;
        bool locked = false;
        CalibMeasure CalMes;
        string[] allowedExt = { ".tiff", ".tif" };
        ObservableCollection<TIFFitem> _TiffItems = new ObservableCollection<TIFFitem>();
        public ObservableCollection<TIFFitem> TiffItems { get { return _TiffItems; } }

        public BindingList<CalibItem> calibList = new BindingList<CalibItem>();

        public SEMDock()
        {
            InitializeComponent();
            initWin();
        }

        public SEMDock(object app)
        {
            InitializeComponent();
            initWin();
            CDWin = (CorelDRAW.Application)app;
            CDWin.SelectionChange += CDWin_SelectionChange;
        }

        void initWin()
        {
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(
                        System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag);
            tiffList.ItemsSource = TiffItems;
            try
            {
                Settings = semImageData.FromString(Properties.Settings.Default.LastData);
            }
            catch
            {
                Settings = new semImageData();
            }
            try
            {
                calibList = new BindingList<CalibItem>();
                calibList.ListChanged += calibList_ListChanged;
                foreach (CalibItem it in JsonConverter<List<CalibItem>>.Deserialize(Properties.Settings.Default.calibList))
                {
                    calibList.Add(it);
                }
            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            currentSettings = Settings;

            calibListView.ItemsSource = calibList;
        }

        void calibList_ListChanged(object sender, ListChangedEventArgs e)
        {
            Properties.Settings.Default.calibList = JsonConverter<List<CalibItem>>.Serialize(calibList.ToList());
            Properties.Settings.Default.Save();
        }

        void CDWin_SelectionChange()
        {
            if (locked) return;
            if (CDWin.ActiveSelection.Shapes.Count == 1 && CDWin.ActiveSelection.Shapes[1].Properties.Exists("semItem", 0))
            {
                try
                {
                    currentSettings = semImageData.FromString(CDWin.ActiveSelection.Shapes[1].Properties["semItem", 1]);
                    currentSettings.PropertyChanged += currentSettings_PropertyChanged;
                }
                catch { }
            }
            else
            {
                currentSettings = Settings;
            }
        }

        void currentSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DoDecorate();
        }

        private void ListView_DragEnter_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void tiffList_Drop(object sender, DragEventArgs e)
        {
            foreach (string file in ((string[])e.Data.GetData(DataFormats.FileDrop, true)))
            {
                if (allowedExt.Contains(Path.GetExtension(file)))
                {
                    _TiffItems.Add(new TIFFitem(file));
                }
            }
        }

        private void ButtonBrowseTiffFiles_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.InitialDirectory = Properties.Settings.Default.LastDir;
            dlg.DefaultExt = ".tif";
            dlg.Filter = "Tiff-File|*.tif;*.tiff";
            dlg.Multiselect = true;
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                foreach (string file in dlg.FileNames)
                {
                    _TiffItems.Add(new TIFFitem(file));
                }
                Properties.Settings.Default.LastDir = Path.GetDirectoryName(dlg.FileName);
                Properties.Settings.Default.Save();
            }
        }

        private void ButtonClearTiffList_Click(object sender, RoutedEventArgs e)
        {
            _TiffItems.Clear();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DoDecorate();
        }

        private void DoDecorate()
        {
            try
            {
                CDWin.ActiveDocument.BeginCommandGroup("SEM-Group");
                CDWin.Application.Optimization = true;
                locked = true;
                if (CDWin == null || CDWin.ActiveDocument == null)
                {
                    MessageBox.Show("No open document");
                    return;
                }
                if (tabMode.SelectedIndex == 0 && _TiffItems.Count > 0)
                {
                    editShapes.Clear();
                    foreach (TIFFitem item in TiffItems)
                    {
                        CDWin.ActiveDocument.ClearSelection();
                        CDWin.ActiveDocument.ActiveLayer.Import(item.path);
                        CorelDRAW.Shape impShape = CDWin.ActiveSelection.Shapes[1];
                        impShape.AlignToPoint(CorelDRAW.cdrAlignType.cdrAlignVCenter, CDWin.Application.ActiveWindow.ActiveView.OriginX, CDWin.Application.ActiveWindow.ActiveView.OriginY);
                        impShape.AlignToPoint(CorelDRAW.cdrAlignType.cdrAlignHCenter, CDWin.Application.ActiveWindow.ActiveView.OriginX, CDWin.Application.ActiveWindow.ActiveView.OriginY);
                        semImage imp = new semImage(CDWin, currentSettings.Clone(), impShape);
                        imp.imgData.filename = Path.GetFileName(item.path);
                        imp.imgData.Calibration = item.calibration;
                        imp.imgData.Mode = (int)CreateMode.Calib;
                        editShapes.Add(imp);
                    }
                    _TiffItems.Clear();
                }
                else if (tabMode.SelectedIndex > 0 && CDWin.ActiveSelection.Shapes.Count > 0)
                {
                    foreach (Shape sh in CDWin.ActiveSelection.Shapes)
                    {
                        if (sh.Properties.Exists("semItem", 0))
                        {
                            foreach (Shape sh_child in sh.PowerClip.Shapes)
                            {
                                if (sh_child.Name == "semItemContent")
                                {
                                    editShapes.Add(new semImage(CDWin, currentSettings.Clone(), sh_child));
                                }
                                else
                                {
                                    sh_child.Delete();
                                }
                            }
                            sh.PowerClip.ExtractShapes();
                            sh.Delete();
                        }
                        else
                        {
                            editShapes.Add(new semImage(CDWin, currentSettings.Clone(), sh));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No image selected");
                    return;
                }

                try
                {
                    foreach (semImage img in editShapes)
                    {
                        decorateShape(img);
                    }
                    editShapes.Clear();
                }
                catch
                {
                    editShapes.Clear();
                }
                CDWin.Application.Optimization = false;
                CDWin.ActiveWindow.Refresh();
                CDWin.ActiveDocument.EndCommandGroup();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            locked = false;
        }

        private void decorateShape(semImage curShape)
        {
            ShapeRange SR = new ShapeRange();
            ShapeRange TextShapes = new ShapeRange();
            Shape Brect, Lrect, Wline, Ttext, Gr;
            double LmarginH, LmarginV, TmarginH, TmarginV, Theight;
            double Lwidth, Wwidth, Wheight, Lheight;
            double Left, Bottom, Width, Height;
            double oldCenterX, oldCenterY;
            double cimgratio;
            bool NoBar = false;
            Shape s = curShape.imgShape;
            semImageData d = curShape.imgData;
            Color White, Black;

            White = new Color();
            Black = new Color();
            Black.CMYKAssign(0, 0, 0, 100);
            White.CMYKAssign(0, 0, 0, 0);

            oldCenterX = s.CenterX;
            oldCenterY = s.CenterY;

            Left = s.LeftX;
            Bottom = s.BottomY;

            cimgratio = s.SizeHeight / s.SizeWidth;

            if (d.Width == 0d && d.Height == 0d)
            {
                Width = s.SizeWidth;
                Height = s.SizeHeight;
            }
            else if (d.Width == 0d)
            {
                Height = cmToUnit(d.Height);
                Width = Height / cimgratio;
            }
            else if (d.Height == 0d)
            {
                Width = cmToUnit(d.Width);
                Height = Width * cimgratio;
            }
            else
            {
                Width = cmToUnit(d.Width);
                Height = cmToUnit(d.Height);
            }
            double newRatio = Height / Width;

            if (newRatio <= cimgratio)
            {
                s.SizeWidth = Width;
                s.SizeHeight = Width * cimgratio;
            }
            else
            {
                s.SizeHeight = Height;
                s.SizeWidth = Height / cimgratio;
            }
            s.LeftX = Left;
            s.BottomY = Bottom;

            if (d.Mode == (int)CreateMode.Calib)
            {
                curShape.calcScale();
            }
            else if (d.Width == 0 || d.BarText == "")
            {
                NoBar = true;
            }

            Lwidth = cmToUnit(curShape.imgData.BarLength);

            Lheight = ptToUnit(8);
            LmarginH = ptToUnit(8);
            LmarginV = ptToUnit(4);
            TmarginH = ptToUnit(4);
            TmarginV = ptToUnit(4);

            Theight = ptToUnit(d.FontSize);
            Wwidth = Lwidth + 2 * LmarginH;
            Wheight = Lheight + LmarginV + TmarginV;

            Brect = CDWin.ActiveLayer.CreateRectangle2(Left, Bottom, Width, Height);
            Brect.Outline.Color = Black;
            Brect.Outline.Width = ptToUnit(d.BorderWidth);

            s.Name = "semItemContent";
            s.AddToPowerClip(Brect, CorelDRAW.cdrTriState.cdrFalse);

            if (d.ValInBar == true)
            {
                Wheight = Theight + 2 * TmarginV;
            }

            if (!NoBar)
            {
                Lrect = CDWin.ActiveLayer.CreateRectangle2(Left + Width - Wwidth, Bottom, Wwidth, Wheight + ((d.ValInBar != true) ? Theight : 0));
                Wline = CDWin.ActiveLayer.CreateLineSegment(Left + Width - LmarginH - Lwidth, Bottom + Wheight / 2, Left + Width - LmarginH, Bottom + Wheight / 2);
                Ttext = CDWin.ActiveLayer.CreateArtisticText(Left + Width - Wwidth / 2, Bottom + Wheight / 2 + TmarginV, d.BarText, Alignment: CorelDRAW.cdrAlignment.cdrCenterAlignment, Size: d.FontSize, Font: d.FontName);
                //Ttext.Text.Story.Font

                Ttext.Text.Story.Bold = d.TextBold == true;
                Lrect.Outline.Width = 0;
                Lrect.Fill.ApplyUniformFill(White);

                Wline.Outline.Width = ptToUnit(d.BarWidth);
                Wline.Outline.Color = Black;
                Wline.Outline.EndArrow = CDWin.ArrowHeads[59];
                Wline.Outline.StartArrow = CDWin.ArrowHeads[59];

                if (d.ValInBar == true)
                {
                    Shape Trect;
                    Trect = CDWin.ActiveLayer.CreateRectangle2(Left + Width, Bottom, 2d * TmarginH + Ttext.SizeWidth, Ttext.SizeHeight);
                    Ttext.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignVCenter, Wline, CorelDRAW.cdrTextAlignOrigin.cdrTextAlignBoundingBox);
                    Ttext.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignHCenter, Wline, CorelDRAW.cdrTextAlignOrigin.cdrTextAlignBoundingBox);
                    Trect.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignVCenter, Ttext);
                    Trect.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignHCenter, Ttext);
                    Trect.Fill.ApplyUniformFill(White);
                    Trect.Outline.Width = 0;
                    Trect.OrderBackOf(Ttext);
                    SR.Add(Trect);
                }

                SR.Add(Lrect);
                SR.Add(Wline);
                SR.Add(Ttext);
                Gr = SR.Group();
                Gr.AddToPowerClip(Brect, CorelDRAW.cdrTriState.cdrFalse);
            }

            if (d.ULtext.Trim() != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + TmarginH, Bottom + Height - Theight - TmarginV, d.ULtext, Alignment: CorelDRAW.cdrAlignment.cdrLeftAlignment, Size: d.FontSize, Font: d.FontName);
                back = CDWin.ActiveLayer.CreateRectangle2(Left, Bottom + Height - Theight - 2d * TmarginV, text.SizeWidth + 2d * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignHCenter, back);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignBottom, back, CorelDRAW.cdrTextAlignOrigin.cdrTextAlignFirstBaseline);
                text.BottomY += TmarginV;
                back.Outline.Width = 0;
                back.OrderBackOf(text);
                text.Text.Story.Bold = d.TextBold == true;
                TextShapes.Add(text);
                TextShapes.Add(back);
            }

            if (d.URtext != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + Width - TmarginH, Bottom + Height - Theight - TmarginV, d.URtext, Alignment: CorelDRAW.cdrAlignment.cdrRightAlignment, Size: d.FontSize, Font: d.FontName);
                back = CDWin.ActiveLayer.CreateRectangle2(Left + Width - text.SizeWidth - 2 * TmarginH, Bottom + Height - Theight - 2 * TmarginV, text.SizeWidth + 2 * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignHCenter, back);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignBottom, back, CorelDRAW.cdrTextAlignOrigin.cdrTextAlignFirstBaseline);
                text.BottomY += TmarginV;
                back.Outline.Width = 0;
                back.OrderBackOf(text);
                text.Text.Story.Bold = d.TextBold == true;
                TextShapes.Add(text);
                TextShapes.Add(back);
            }

            if (d.BLtext != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + TmarginH, Bottom + TmarginV, d.BLtext, Alignment: CorelDRAW.cdrAlignment.cdrLeftAlignment, Size: d.FontSize, Font: d.FontName);
                back = CDWin.ActiveLayer.CreateRectangle2(Left, Bottom, text.SizeWidth + 2 * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignHCenter, back);
                text.AlignToShape(CorelDRAW.cdrAlignType.cdrAlignBottom, back, CorelDRAW.cdrTextAlignOrigin.cdrTextAlignFirstBaseline);
                text.BottomY += TmarginV;
                back.Outline.Width = 0;
                back.OrderBackOf(text);
                text.Text.Story.Bold = d.TextBold == true;
                TextShapes.Add(text);
                TextShapes.Add(back);
            }

            TextShapes.AddToPowerClip(Brect, CorelDRAW.cdrTriState.cdrFalse);

            Brect.Properties["semItem", 0] = true;
            Brect.Properties["semItem", 1] = curShape.imgData.ToString();
            if (d.filename != "") Brect.Name = d.filename;

            Brect.CenterX = oldCenterX;
            Brect.CenterY = oldCenterY;

            CDWin.ActiveWindow.Refresh();
            CDWin.Application.Refresh();
            CDWin.ActiveDocument.ClearSelection();

            Brect.Selected = true;
        }

        double cmToUnit(double centimeter)
        {
            return CDWin.ConvertUnits(centimeter, CorelDRAW.cdrUnit.cdrCentimeter, CDWin.ActiveDocument.Unit);
        }

        double ptToUnit(double centimeter)
        {
            return CDWin.ConvertUnits(centimeter, CorelDRAW.cdrUnit.cdrPoint, CDWin.ActiveDocument.Unit);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LastData = currentSettings.ToString();

            Properties.Settings.Default.Save();
        }

        private void SaveButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            Settings = new semImageData();
            currentSettings = Settings;
        }

        private void ButtonAddToCalibList_Click(object sender, RoutedEventArgs e)
        {
            calibList.Add(new CalibItem("New Item", currentSettings.Calibration));
        }

        private void ButtonChooseCalib_Click(object sender, RoutedEventArgs e)
        {
            CalibItem item = calibListView.SelectedItem as CalibItem;
            if (item != null)
                currentSettings.Calibration = item.Calibration;
        }

        private void ButtonDeleteCalib_Click(object sender, RoutedEventArgs e)
        {
            CalibItem item = calibListView.SelectedItem as CalibItem;
            if (item != null)
                calibList.Remove(item);
        }

        private void ButtonShowList_Click(object sender, RoutedEventArgs e)
        {
            expanderList.Visibility = System.Windows.Visibility.Visible;
        }

        private void ButtonStartMeasure_Click(object sender, RoutedEventArgs e)
        {
            if (CreateMeasureRect())
            {
                ButStartMes.Visibility = System.Windows.Visibility.Collapsed;
                ButStopMes.Visibility = System.Windows.Visibility.Visible;
            }

        }

        bool CreateMeasureRect()
        {
            if (CDWin == null || CDWin.ActiveSelection == null || CDWin.ActiveSelection.Shapes.Count == 0 || CDWin.ActiveSelection.Shapes[1].Type != CorelDRAW.cdrShapeType.cdrBitmapShape)
            {
                MessageBox.Show("No Image selected. Select an Image first");
                return false;
            }
            CalMes = new CalibMeasure();

            CalMes.img = CDWin.ActiveSelection.Shapes[1];
            CalMes.mesRect = CDWin.ActiveLayer.CreateRectangle2(
                CDWin.ActiveWindow.ActiveView.OriginX,
                CDWin.ActiveWindow.ActiveView.OriginY,
                CDWin.ConvertUnits(3d, CorelDRAW.cdrUnit.cdrCentimeter, CDWin.ActiveDocument.Unit),
                CDWin.ConvertUnits(1d, CorelDRAW.cdrUnit.cdrCentimeter, CDWin.ActiveDocument.Unit)
                );

            Color back = new Color();
            back.CMYKAssign(0, 60, 100, 0);

            CalMes.mesRect.Selected = true;
            CalMes.mesRect.Outline.Width = 0;
            CalMes.mesRect.Fill.ApplyUniformFill(back);
            CalMes.mesRect.Transparency.ApplyUniformTransparency(0);
            CalMes.mesRect.Transparency.MergeMode = CorelDRAW.cdrMergeMode.cdrMergeXOR;

            return true;
        }

        private void StopMesClick(object sender, RoutedEventArgs e)
        {
            double rw = 0d;
            CalMes.vertical = (CalibDirection.SelectedIndex == 0) ? false : true;
            if (double.TryParse(CalibRealWidth.Text, out rw))
            {
                CalMes.realWidth = rw;
                CalibMesWidth.Text = CDWin.ConvertUnits((CalMes.vertical) ? CalMes.mesRect.SizeHeight : CalMes.mesRect.SizeWidth, CDWin.ActiveDocument.Unit, CorelDRAW.cdrUnit.cdrCentimeter).ToString();
                CalibMesFactor.Text = CalMes.calibrationFactor.ToString();

                CalMes.Delete();

                ButStartMes.Visibility = System.Windows.Visibility.Visible;
                ButStopMes.Visibility = System.Windows.Visibility.Collapsed;
                ButApplyMes.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Insert the real distance first");
            }
        }

        private void MesApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentSettings.Calibration = double.Parse(CalibMesFactor.Text);
                toggleCalib.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserControl_DragEnter(object sender, DragEventArgs e)
        {
            currentSettings.Mode = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(calibList[0].Name);
        }

        private void calibListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CalibItem item = ((FrameworkElement)e.OriginalSource).DataContext as CalibItem;
            if (item != null)
            {
                changeItemName.Visibility = System.Windows.Visibility.Visible;
                TextListViewName.DataContext = item;
                e.Handled = true;
            }
        }

        private void TextListViewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    TextListViewName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    changeItemName.Visibility = System.Windows.Visibility.Collapsed;
                }
                catch { }
                e.Handled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            about ab = new about();
            ab.ShowDialog();
        }
    }

    enum CreateMode
    {
        Tiff = 0,
        Calib = 1,
        Length = 2,
    }
}
