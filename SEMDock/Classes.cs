using System;
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
        string _path, _filename;
        double _calibration = 1d;

        public string path { get { return _path; } set { _path = value; } }
        public string filename { get { return _filename; } set { _filename = value; } }
        public double calibration { get { return _calibration; } set { _calibration = value; } }

        public TIFFitem(string file)
        {
            if (File.Exists(file))
            {
                path = file;
                filename = Path.GetFileName(file);

                StreamReader FS = new StreamReader(file);
                string line = "";
                string cstring = "";

                while ((line = FS.ReadLine()) != null)
                {
                    if (line.Contains("AP_IMAGE_PIXEL_SIZE"))
                    {
                        cstring = FS.ReadLine().Split('=')[1];
                        break;
                    }
                }

                if (cstring != "") calibration = parseCalibMicron(cstring);
            }

        }

        private double parseCalibMicron(string Cstring)
        {
            double val = 0d;
            string[] tmp;

            NumberFormatInfo NF = new NumberFormatInfo();
            NF.NumberDecimalSeparator = ".";
            NF.NumberGroupSeparator = "";

            tmp = Cstring.Trim().Split(' ');
            if (double.TryParse(tmp[0], NumberStyles.Any, NF, out val))
            {
                tmp[1] = tmp[1].Trim();

                switch (tmp[1])
                {
                    case "nm": val = val / 1000d;
                        break;
                    case "mm": val = val * 1000d;
                        break;
                    case "pm": val = val / 1000000d;
                        break;
                    default:
                        break;
                }
            }

            return val;
        }
    }

    [Serializable,DataContract]
    public class semImageData
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
        [DataMember(Name = "Mode")]
        private int _Mode = 1;
        [DataMember(Name = "TextBold")]
        private bool? _TextBold = false;
        [DataMember(Name = "BarMinWidth")]
        private float _BarMinWidth = 3f;
        [DataMember(Name = "BarMaxWidth")]
        private float _BarMaxWidth = 5f;

        public string ULtext { get { return _ULtext; } set { _ULtext = value; } }
        public string URtext { get { return _URtext; } set { _URtext = value; } }
        public string BLtext { get { return _BLtext; } set { _BLtext = value; } }
        public string BarText { get { return _BarText; } set { _BarText = value; } }
        public string filename { get { return _filename; } set { _filename = value; } }
        public float FontSize { get { return _FontSize; } set { _FontSize = value; } }
        public float BarWidth { get { return _BarWidth; } set { _BarWidth = value; } }
        public float BorderWidth { get { return _BorderWidth; } set { _BorderWidth = value; } }
        public float Width { get { return _Width; } set { _Width = value; } }
        public float Height { get { return _Height; } set { _Height = value; } }
        public double BarLength { get { return _BarLength; } set { _BarLength = value; } }
        public double Calibration { get { return _Calibration; } set { _Calibration = value; } }
        public int Mode { get { return _Mode; } set { _Mode = value; } }
        public bool? TextBold { get { return _TextBold; } set { _TextBold = value; } }
        public float BarMinWidth { get { return _BarMinWidth; } set { _BarMinWidth = value; } }
        public float BarMaxWidth { get { return _BarMaxWidth; } set { _BarMaxWidth = value; } }


        public override string ToString()
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(semImageData));
            Stream s = new MemoryStream();
            js.WriteObject(s, this);
            s.Position = 0;
            StreamReader sr = new StreamReader(s);
            return sr.ReadToEnd();
        }

        public static semImageData FromString(string JsonData)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(semImageData));
            byte[] bA = Encoding.UTF8.GetBytes(JsonData);
            Stream s = new MemoryStream(bA);
            s.Position = 0;
            semImageData tmp = (semImageData)js.ReadObject(s);
            return tmp;
        }

        public semImageData Clone()
        {
            return semImageData.FromString(this.ToString());
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
            double[] res = new double[2];
            double OffsetResolution = CDWin.ConvertUnits(
                imgShape.Bitmap.SizeWidth / imgShape.SizeWidth,
                cdrUnit.cdrCentimeter,
                CDWin.ActiveDocument.Unit);

            double minVal = imgData.BarMinWidth* OffsetResolution * imgData.Calibration;
            double pot = Math.Pow(10d, Math.Floor(Math.Log10(minVal)));

            for (int i = 1; i <= 19; i++)
            {
                double l, z;

                z = i * pot;
                l = z / (OffsetResolution * imgData.Calibration);

                if (l >= imgData.BarMinWidth && l <= imgData.BarMaxWidth)
                {
                    imgData.BarLength = l;
                    imgData.BarText = getScaleText( z);
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
            else if (scale <= 0.001d) res = (scale * 1000000d).ToString() + " pm";
            else if (scale <= 1d) res = (scale * 1000d).ToString() + " nm";
            else res = scale.ToString() + " µm";

            return res;
        }
    }

    enum PropID
    {
        calibration = 1,
        TextOL = 2,
        TextOR = 3,
        TextUL = 4,
        Length = 5,
        Text = 6,
        lineW = 7,
        lineBW = 8,
        TextBold = 9,
        TextSize = 10,
        width = 11,
        height = 12,
        Mode = 13,
        isBalkenGroup = 14,
        filename = 15,
    }
}
