using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    public class TaxInformationStep
    {
        public int Id { get; set; }
        public int TaxInformationId { get; set; }
        public decimal StepAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public TaxInformationStep()
        {
            
        }
    }
}
