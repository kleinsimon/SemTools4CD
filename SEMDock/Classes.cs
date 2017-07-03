using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Globalization;
using System.IO;
using System.Windows;
using CorelDRAW;
using System.Runtime.Serialization;

namespace SEMTools4CD
{
    public class TIFFitem
    {
        string _path, _filename, _unit;
        double _calibration = 1d;
        long _cutBottom = 0;

        public string path { get { return _path; } set { _path = value; } }
        public string filename { get { return _filename; } set { _filename = value; } }
        public double calibration { get { return _calibration; } set { _calibration = value; } }
        public string unit { get { return _unit; } set { _unit = value; } }
        public long cutBottom { get { return _cutBottom; } set { _cutBottom = value; } }


        public TIFFitem(string file)
        {
            string cstring = "";
            if (File.Exists(file))
            {
                path = file;
                filename = Path.GetFileName(file);
                string supFile = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file) + "-tif.hdr";

                if (File.Exists(supFile))
                {
                    cstring = findKeyValuePair(supFile, "PixelSizeX");
                    string cutstring = findKeyValuePair(supFile, "ImageStripSize");
                    long valcut = 0;
                    long.TryParse(cutstring, out valcut);
                    cutBottom = valcut;
                }
                else
                {
                    cstring = findKeyValuePair(path, "AP_IMAGE_PIXEL_SIZE", '=' ,true);
                }
                if (cstring != "") parseCalibMicron(cstring);
            }
        }

        private string findKeyValuePair(string FileName, string key, char seperator = '=', bool nextLine=false)
        {
            StreamReader FS = new StreamReader(FileName, Encoding.Default);
            
            string line = "";
            string cstring = string.Empty;

            while ((line = FS.ReadLine()) != null)
            {
                if (line.Contains(key))
                {
                    if (nextLine)
                        cstring = line = FS.ReadLine().Split(seperator)[1];
                    else
                        cstring = line = line.Split(seperator)[1];
                    break;
                }
            }
            FS.Close();
            return cstring;
        }

        private void parseCalibMicron(string Cstring)
        {
            double val = 0d;
            string[] tmp;

            NumberFormatInfo NF = new NumberFormatInfo();
            NF.NumberDecimalSeparator = ".";
            NF.NumberGroupSeparator = "";

            tmp = Cstring.Trim().Split(' ');
            if (double.TryParse(tmp[0], NumberStyles.Any, NF, out val))
            {
                if (tmp.Length > 1)
                {
                    unit = tmp[1].Trim();
                    unit.Replace('\u00b5', 'µ');
                    unit.Replace('\u03bc', 'µ');

                }
                else
                {
                    unit = "m";
                    MessageBox.Show("Unit " + unit.ToString() + " could not be parsed" );
                }

                ValWithUnit t = new ValWithUnit(val, unit);
                calibration = t.getInUnit("µm").Value;
            } else
            {
                MessageBox.Show("Number could not be parsed");
            }
        }
    }

    [Serializable, DataContract]
    public class semImageData : INotifyPropertyChanged
    {
        [DataMember(Name = "ULtext")]
        private string _ULtext = "";
        [DataMember(Name = "URtext")]
        private string _URtext = "";
        [DataMember(Name = "BLtext")]
        private string _BLtext = "";
        [DataMember(Name = "BarText")]
        private string _BarText = "";
        [DataMember(Name = "FileName")]
        private string _filename = "";
        [DataMember(Name = "FontSize")]
        private float _FontSize = 10f;
        [DataMember(Name = "BarWidth")]
        private float _BarWidth = 1.5f;
        [DataMember(Name = "BorderWidth")]
        private float _BorderWidth = 1.5f;
        [DataMember(Name = "Width")]
        private float _Width = 11f;
        [DataMember(Name = "Height")]
        private float _Height = 0f;
        [DataMember(Name = "BarLength")]
        private double _BarLength = 3f;
        [DataMember(Name = "Calibration")]
        private double _Calibration = 1d;
        [DataMember(Name = "Unit")]
        private string _Unit = "";
        [DataMember(Name = "Mode")]
        private int _Mode = 1;
        [DataMember(Name = "TextBold")]
        private bool? _TextBold = false;
        [DataMember(Name = "BarMinWidth")]
        private float _BarMinWidth = 3f;
        [DataMember(Name = "BarMaxWidth")]
        private float _BarMaxWidth = 5f;
        [DataMember(Name = "ValInBar")]
        private bool? _ValInBar = false;
        [DataMember(Name = "BarBelowImage")]
        private bool? _BarBelowImage = false;
        [DataMember(Name = "Font")]
        private string _Font = "Arial";
        [DataMember(Name = "CutBottom")]
        private double _CutBottom = 0d;

        public string ULtext { get { return _ULtext; } set { _ULtext = value; NotifyPropertyChanged("ULtext"); } }
        public string URtext { get { return _URtext; } set { _URtext = value; NotifyPropertyChanged("URtext"); } }
        public string BLtext { get { return _BLtext; } set { _BLtext = value; NotifyPropertyChanged("BLtext"); } }
        public string BarText { get { return _BarText; } set { _BarText = value; NotifyPropertyChanged("BarText"); } }
        public string filename { get { return _filename; } set { _filename = value; NotifyPropertyChanged("filename"); } }
        public float FontSize { get { return _FontSize; } set { _FontSize = value; NotifyPropertyChanged("FontSize"); } }
        public float BarWidth { get { return _BarWidth; } set { _BarWidth = value; NotifyPropertyChanged("BarWidth"); } }
        public float BorderWidth { get { return _BorderWidth; } set { _BorderWidth = value; NotifyPropertyChanged("BorderWidth"); } }
        public float Width { get { return _Width; } set { _Width = value; NotifyPropertyChanged("Width"); } }
        public float Height { get { return _Height; } set { _Height = value; NotifyPropertyChanged("Height"); } }
        public double BarLength { get { return _BarLength; } set { _BarLength = value; NotifyPropertyChanged("BarLength"); } }
        public double Calibration { get { return _Calibration; } set { _Calibration = value; NotifyPropertyChanged("Calibration"); } }
        public string Unit { get { return _Unit; } set { _Unit = value; NotifyPropertyChanged("Unit"); } }
        public int Mode { get { return _Mode; } set { _Mode = value; NotifyPropertyChanged("Mode"); } }
        public bool? TextBold { get { return _TextBold; } set { _TextBold = value; NotifyPropertyChanged("TextBold"); } }
        public float BarMinWidth { get { return _BarMinWidth; } set { _BarMinWidth = value; NotifyPropertyChanged("BarMinWidth"); } }
        public float BarMaxWidth { get { return _BarMaxWidth; } set { _BarMaxWidth = value; NotifyPropertyChanged("BarMaxWidth"); } }
        public bool? ValInBar { get { return _ValInBar; } set { _ValInBar = value; NotifyPropertyChanged("ValInBar"); } }
        public bool? BarBelowImage { get { return _BarBelowImage; } set { _BarBelowImage = value; NotifyPropertyChanged("BarBelowImage"); } }
        public System.Windows.Media.FontFamily Font { get { return new System.Windows.Media.FontFamily(_Font); } set { _Font = value.Source; NotifyPropertyChanged("Font"); } }
        public string FontName { get { return _Font; } set { _Font = value; NotifyPropertyChanged("FontName"); } }
        public double CutBottom { get { return _CutBottom; } set { _CutBottom = value; NotifyPropertyChanged("CutBottom"); } }


        public override string ToString()
        {
            return JsonConverter<semImageData>.Serialize(this);
        }

        public static semImageData FromString(string JsonData)
        {
            return JsonConverter<semImageData>.Deserialize(JsonData);
        }

        public semImageData Clone()
        {
            return semImageData.FromString(this.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public static class JsonConverter<T>
    {
        public static string Serialize(T input)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            Stream s = new MemoryStream();
            js.WriteObject(s, input);
            s.Position = 0;
            StreamReader sr = new StreamReader(s);
            return sr.ReadToEnd();
        }

        public static T Deserialize(string JsonData)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            byte[] bA = Encoding.UTF8.GetBytes(JsonData);
            Stream s = new MemoryStream(bA);
            s.Position = 0;
            T tmp = (T)js.ReadObject(s);
            return tmp;
        }
    }

    public class CalibMeasure
    {
        public bool vertical { get; set; }
        public double realWidth { get; set; }
        public double imgResolution
        {
            get
            {
                if (vertical)
                    return img.Bitmap.SizeHeight / img.SizeHeight;
                else
                    return img.Bitmap.SizeWidth / img.SizeWidth;
            }
        }
        public double calibrationFactor
        {
            get
            {
                if (vertical)
                    return realWidth / (mesRect.SizeHeight * imgResolution);
                else
                    return realWidth / (mesRect.SizeWidth * imgResolution);
            }
        }
        public Shape mesRect { get; set; }
        public Shape img { get; set; }

        public void Delete()
        {
            mesRect.Delete();
        }
    }

    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public T True { get; set; }
        public T False { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }

    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed) { }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean. Is " + targetType.ToString());

            return !(bool)value;
        }

        #endregion
    }

    [ValueConversion(typeof(bool?), typeof(bool))]
    public class BooleanToNullableConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool?))
            {
                throw new InvalidOperationException("The target must be a nullable boolean");
            }
            bool? b = (bool?)value;
            return b.HasValue && b.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class InvertVisibilityConverter : IValueConverter
    {

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                Visibility vis = (Visibility)value;
                return vis == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new InvalidOperationException("Converter can only convert to value of type Visibility.");
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new Exception("Invalid call - one way only");
        }
    }
    

    public class semImage
    {
        private CorelDRAW.Application CDWin;
        public CorelDRAW.Shape imgShape;
        public semImageData imgData;

        public semImage(CorelDRAW.Application _CDWin, semImageData Data, CorelDRAW.Shape _imgShape)
        {
            CDWin = _CDWin;
            imgShape = _imgShape;
            imgData = Data;
        }

        public void calcScale()
        {
            try
            {
                double[] res = new double[2];
                double OffsetResolution = CDWin.ConvertUnits(
                    imgShape.Bitmap.SizeWidth / imgShape.SizeWidth,
                    cdrUnit.cdrCentimeter,
                    CDWin.ActiveDocument.Unit);

                double minVal = imgData.BarMinWidth * OffsetResolution * imgData.Calibration;
                double pot = Math.Pow(10d, Math.Floor(Math.Log10(minVal)));

                for (int i = 1; i <= 19; i++)
                {
                    double l, z;

                    z = i * pot;
                    l = z / (OffsetResolution * imgData.Calibration);

                    if (l >= imgData.BarMinWidth && l <= imgData.BarMaxWidth)
                    {
                        imgData.BarLength = l;
                        imgData.BarText = getScaleText(z);
                        return;
                    }
                    else
                    {
                        z *= 10;
                        l = z / (OffsetResolution * imgData.Calibration);
                        if (l >= imgData.BarMinWidth && l <= imgData.BarMaxWidth)
                        {
                            imgData.BarLength = l;
                            imgData.BarText = getScaleText(z);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            imgData.BarLength = 2d;
            imgData.BarText = "Error";
            return;
        }

        private string getScaleText(double scale)
        {
            string res = "";

            if (scale >= 1000000000d) res = (scale / 1000000000d).ToString() + " km";
            else if (scale >= 1000000d) res = (scale / 1000000d).ToString() + " m";
            else if (scale >= 10000d) res = (scale / 10000d).ToString() + " cm";
            else if (scale >= 1000d) res = (scale / 1000d).ToString() + " mm";
            else if (scale < 0.001d) res = (scale * 1000000d).ToString() + " pm";
            else if (scale < 1d) res = (scale * 1000d).ToString() + " nm";
            else res = scale.ToString() + " µm";

            return res;
        }
    }

    [DataContract]
    public class CalibItem : INotifyPropertyChanged
    {
        [DataMember(Name = "Name")]
        private string _name = "New Item";
        [DataMember(Name = "Calibration")]
        private double _calibration = 1d;
        [DataMember(Name = "Unit")]
        private string _unit = "";

        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged("Name"); } }
        public double Calibration { get { return _calibration; } set { _calibration = value; NotifyPropertyChanged("Calib"); } }
        public string Unit { get { return _unit; } set { _unit = value; NotifyPropertyChanged("Unit"); } }

        public CalibItem(string name, double calib)
        {
            Name = name;
            Calibration = calib;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string n)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(n));
        }
    }

    public class ValWithUnit
    {
        static Dictionary<string, double> Units = new Dictionary<string, double>()
            { // All relative to mm
                {"fm", 1e-12d},
                {"pm", 1e-9d},
                {"nm", 1e-6d},
                {"µm", 1e-3d},
                {"mm", 1d},
                {"cm", 1e+1d},
                {"dm", 1e+2d},
                {"m" , 1e+3d},
                {"hm", 1e+5d},
                {"km", 1e+6d},
            };

        private double _value = 0d;
        private string _unit = "";

        public double Value { get { return _value; } set { _value = value; } }
        public string Unit { get { return _unit; } set { _unit = value; } }
        public double Coefficient { get { return ValWithUnit.Units.GetValueOrDefault(Unit, 1d); } }


        public ValWithUnit(double value, string unit)
        {
            Value = value;
            Unit = unit;
        }

        public ValWithUnit getInUnit(string targetUnit)
        {
            string tu = targetUnit.Trim();
            double exp = this.Coefficient / ValWithUnit.Units.GetValueOrDefault(tu, 1d);
            return new ValWithUnit(Value * exp, tu);
        }

        public static ValWithUnit operator *(ValWithUnit src, double pot)
        {
            double difpot = src.Coefficient * pot;
            var Srt = Units.OrderBy(v => (v.Value - difpot));
            KeyValuePair<string, double> nextUnit;
            foreach (KeyValuePair<string, double> kvp in Srt)
            {
                if (kvp.Value >= 0d)
                {
                    nextUnit = kvp;
                    break;
                }
            }

            return new ValWithUnit(src.Value * pot, src.Unit);
        }
    }

    public static class Extensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>
                (this IDictionary<TKey, TValue> dictionary,
                 TKey key,
                 TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }
    }

}
