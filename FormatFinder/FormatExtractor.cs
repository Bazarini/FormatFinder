using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace FormatFinderCore
{
    public class FormatExtractor
    {        
        static void FillFormats()
        {
            _paperFormats = new List<PaperFormat>();
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A0, minSize: new Size(8475, 11985), maxSize: new Size(int.MaxValue, int.MaxValue)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A1, minSize: new Size(5985, 8475), maxSize: new Size(8475, 11985)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A2, minSize: new Size(4230, 5985), maxSize: new Size(5985, 8475)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A3, minSize: new Size(3000, 4230), maxSize: new Size(4230, 5985)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A4, minSize: new Size(2115, 3000), maxSize: new Size(3000, 4230)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A5, minSize: new Size(1485, 2115), maxSize: new Size(2115, 3000)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A6, minSize: new Size(1050, 1485), maxSize: new Size(1485, 2115)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A7, minSize: new Size(735, 1050), maxSize: new Size(1050, 1485)));
            _paperFormats.Add(new PaperFormat(name: PaperFormatName.A8, minSize: new Size(525, 735), maxSize: new Size(735, 1050)));
        }
        static List<PaperFormat> _paperFormats;
        public static FormatInfo GetFormatName(Size input)
        {
            if (_paperFormats == null)
                FillFormats();
            foreach (var format in _paperFormats.OrderByDescending(s => s.MaxVal))
            {
                if (format.IsInRange(input))
                    return new FormatInfo() { Name = format.Name, IsStandard = true };
                if (format.IsInParticularRange(input))
                    return new FormatInfo() { Name = format.Name, IsStandard = false };
            }
            return new FormatInfo() { Name = PaperFormatName.Unknown, IsStandard = false };
        }
        public static FormatInfo GetFormatName(int width, int height)
        {
            Size size = new Size(width, height);
            return GetFormatName(size);
        }        
        public static FormatInfo GetFormatName(string file)
        {
            Bitmap bitmap = new Bitmap(file);
            var size = bitmap.Size;
            bitmap.Dispose();
            return GetFormatName(size);
        }
        public static Dictionary<string, FormatInfo> GetFormats(List<string> files)
        {
            Dictionary<string, FormatInfo> indexed = new Dictionary<string, FormatInfo>();
            foreach (var file in files)
            {
                var format = GetFormatName(file);
                indexed.Add(file, format);
                Console.WriteLine(file + " Format: " + format.Name.ToString(), " Standard: " + format.IsStandard);
            }
            return indexed;
        }
    }
}
