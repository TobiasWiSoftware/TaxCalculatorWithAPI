using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    public class Tracking
    {
        public int Id { get; set; }
        public int VisitCounter { get; set; }

        private static string _path = string.Empty;
        public static void DataPathInit(String s)
        {
            _path = Path.Combine(s, "Data", "Tracker.txt");
        }
        public static int IncrementVisitCounter()
        {
            int i = -1;
            try
            {
                using (StreamReader r = new(_path))
                {
                    string? str = r.ReadLine();
                    if (str != null)
                    {
                        i = int.Parse(str);
                    }
                }
            }
            catch (Exception)
            {
                i = 0;
            }
            using (StreamWriter w = new(_path, false))
            {
                w.WriteLine(++i);
            }


            return i;
        }
        public static int GetVisitCounter()
        {
            int i = -1;

            try
            {
                using (StreamReader r = new(_path))
                {
                    string? str = r.ReadLine();
                    if (str != null)
                    {
                        i = int.Parse(str);
                    }
                }
            }
            catch (Exception)
            {
            }

            return i;
        }
    }
}
