﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
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
        string[] allowedExt = { ".tiff", ".tif" };
        ObservableCollection<TIFFitem> _TiffItems = new ObservableCollection<TIFFitem>();
        public ObservableCollection<TIFFitem> TiffItems { get { return _TiffItems; } }

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
            tiffList.ItemsSource = TiffItems;
            try
            {
                //currentSettings = new semImageData();
                Settings = semImageData.FromString(Properties.Settings.Default.LastData);
            }
            catch
            {
                Settings = new semImageData();
            }
            currentSettings = Settings;
        }

        void CDWin_SelectionChange()
        {
            if (locked) return;
            if (CDWin.ActiveSelection.Shapes.Count == 1 && CDWin.ActiveSelection.Shapes[1].Properties.Exists("semItem", 0))
            {
                try
                {
                    currentSettings = semImageData.FromString(CDWin.ActiveSelection.Shapes[1].Properties["semItem", 1]);
                }
                catch
                {

                }
            }
            else
            {
                currentSettings = Settings;
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _TiffItems.Clear();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            locked = true;
            if (CDWin == null || CDWin.ActiveDocument == null)
            {
                MessageBox.Show("No open document");
                return;
            }
            try
            {
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

                CDWin.ActiveDocument.BeginCommandGroup("semItem");
                CDWin.Application.Optimization = true;

                foreach (semImage img in editShapes)
                {
                    decorateShape(img);
                }
                editShapes.Clear();
                CDWin.ActiveDocument.EndCommandGroup();
                CDWin.Application.Optimization = false;
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

            if (!NoBar)
            {
                Lrect = CDWin.ActiveLayer.CreateRectangle2(Left + Width - Wwidth, Bottom, Wwidth, Wheight + Theight);
                Wline = CDWin.ActiveLayer.CreateLineSegment(Left + Width - LmarginH - Lwidth, Bottom + Wheight / 2, Left + Width - LmarginH, Bottom + Wheight / 2);
                Ttext = CDWin.ActiveLayer.CreateArtisticText(Left + Width - Wwidth / 2, Bottom + Wheight / 2 + TmarginV, d.BarText, Alignment: CorelDRAW.cdrAlignment.cdrCenterAlignment, Size: d.FontSize);

                Ttext.Text.Story.Bold = d.TextBold == true;
                Lrect.Outline.Width = 0;
                Lrect.Fill.ApplyUniformFill(White);

                Wline.Outline.Width = ptToUnit(d.BarWidth);
                Wline.Outline.Color = Black;
                Wline.Outline.EndArrow = CDWin.ArrowHeads[59];
                Wline.Outline.StartArrow = CDWin.ArrowHeads[59];
                SR.Add(Lrect);
                SR.Add(Wline);
                SR.Add(Ttext);
                Gr = SR.Group();
                Gr.AddToPowerClip(Brect, CorelDRAW.cdrTriState.cdrFalse);
            }

            if (d.ULtext.Trim() != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + TmarginH, Bottom + Height - Theight - TmarginV, d.ULtext, Alignment: CorelDRAW.cdrAlignment.cdrLeftAlignment, Size: d.FontSize);
                back = CDWin.ActiveLayer.CreateRectangle2(Left, Bottom + Height - Theight - 2 * TmarginV, text.SizeWidth + 2 * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
                back.Outline.Width = 0;
                back.OrderBackOf(text);
                text.Text.Story.Bold = d.TextBold == true;
                TextShapes.Add(text);
                TextShapes.Add(back);
            }

            if (d.URtext != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + Width - TmarginH, Bottom + Height - Theight - TmarginV, d.URtext, Alignment: CorelDRAW.cdrAlignment.cdrRightAlignment, Size: d.FontSize);
                back = CDWin.ActiveLayer.CreateRectangle2(Left + Width - text.SizeWidth - 2 * TmarginH, Bottom + Height - Theight - 2 * TmarginV, TextShapes[TextShapes.Count].SizeWidth + 2 * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
                back.Outline.Width = 0;
                back.OrderBackOf(text);
                text.Text.Story.Bold = d.TextBold == true;
                TextShapes.Add(text);
                TextShapes.Add(back);
            }

            if (d.BLtext != "")
            {
                Shape back, text;
                text = CDWin.ActiveLayer.CreateArtisticText(Left + TmarginH, Bottom + TmarginV, d.BLtext, Alignment: CorelDRAW.cdrAlignment.cdrLeftAlignment, Size: d.FontSize);
                back = CDWin.ActiveLayer.CreateRectangle2(Left, Bottom, text.SizeWidth + 2 * TmarginH, Theight + 2 * TmarginV);
                back.Fill.ApplyUniformFill(White);
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
    }

    enum CreateMode
    {
        Tiff = 0,
        Calib = 1,
        Length = 2,
    }
}