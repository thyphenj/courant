using System;
using System.Linq;
using System.Collections.Generic;

namespace SlipStream
{
    class Line
    {
        public string Day;
        public string Month;
        public List<string> Desc = new List<string>();
        public string MoneyOut;
        public string MoneyIn;
        public string Balance;

        public Line()
        {

        }
    }
    class Program
    {
        static void Main()
        {
            string path = "../../../datafiles/20200828.pdf";

            using (var pdf = UglyToad.PdfPig.PdfDocument.Open(path))
            {
                List<double> linePositions = new List<double>();
                List<AWord> chunks = new List<AWord>();

                foreach (var page in pdf.GetPages())
                {
                    var words = page.GetWords();
                    foreach (var word in words)
                    {
                        // This is the "Description" field from the table
                        if (isAbout(word.BoundingBox.Left, 237.6))
                        {
                            double B = word.BoundingBox.Bottom;
                            if (!linePositions.Contains(B))
                                linePositions.Add(B);
                        }
                    }
                    foreach (var word in words)
                    {
                        if (linePositions.Contains(word.BoundingBox.Bottom))
                        {
                            chunks.Add(new AWord(word.BoundingBox.Left
                                , word.BoundingBox.Bottom
                                , word.BoundingBox.Right
                                , word.Text));
                        }
                    }
                    foreach (double y in linePositions)
                    {
                        foreach (var z in chunks.Where(a => a.Y == y))
                        {
                            Console.Write($"{z.Mout} {z.Min} {z.Bal}");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }

        private static bool isAbout(double a, double b)
        {
            return (Math.Abs(a - b) < 0.05);
        }
    }

    class AWord
    {
        public double X;
        public double Y;
        public double R;
        public string Text;
        public string Min;
        public string Mout;
        public string Bal;

        public AWord(double x, double y, double r, string text)
        {
            X = x;
            Y = y;
            R = r;
            Text = text;
            if (Math.Abs(R - 409.70) < 0.1) Mout = text;
            if (Math.Abs(R - 475.87) < 0.1) Min = text;
            if (Math.Abs(R - 560.08) < 0.1) Bal = text;
        }
        public override string ToString()
        {
            return $"{X:F3},{Y:F3},{R:F3} - {Text}";
        }
    }

    class Tran
    {
        public DateTime When;
        public string Description;
        public float Value;
        public bool Debit;
        public Tran()
        {

        }
    }
}