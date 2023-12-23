using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    public class Tracking
    {
        private static string _path = string.Empty;

        public static void DataPathInit(String s)
        {
            _path = Path.Combine(s, "Data", "Tracker.txt");
        }
        public static bool IncrementVisitCounter()
        {
            try
            {
                int i = 0;
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
