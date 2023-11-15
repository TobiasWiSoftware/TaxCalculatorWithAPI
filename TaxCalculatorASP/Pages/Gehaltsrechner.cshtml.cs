using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace TaxCalculatorASP.Pages
{
    [BindProperties]
    public class GehaltsrechnerModel : PageModel
    {
        public int Year { get; set; }
        public decimal GrossIncome { get; set; }
        public string? BillingPeriod { get; set; }
        public int TaxClass { get; set; }
        public decimal TaxFree { get; set; } = 0;
        public bool InChurch { get; set; }
        public string? FederalState { get; set; }
        public int? Age { get; set; }
        public bool HasChildren { get; set; }
        public decimal ChildTaxCredit { get; set; }
        public string? Insurance { get; set; }

        public decimal InsuranceAdditionTotal { get; set; }
        public string? Pension { get; set; }
        public string? Unimployment { get; set; }


        public SocialSecurityRates? SocialSecurityRatesOfYear { get; set; }
        public TaxInformation? TaxInformationOfYear { get; set; }







        public decimal InsuranceSum { get; set; }
        public decimal InsuranceCareSum { get; set; }
        public decimal PensionSum { get; set; }
        public decimal UnimploymentSum { get; set; }
        public decimal InsurancesTotal { get => InsuranceSum + InsuranceCareSum + PensionSum + UnimploymentSum; }
        public decimal TaxedIncome { get; set; }
        public decimal TaxSum { get; set; }
        public decimal BorderTaxSet { get; set; }
        public decimal AvgTaxSet { get; set; }







        public void OnGet()
        {
            Year = DateTime.Now.Year;

            if (TaxInformationOfYear == null || SocialSecurityRatesOfYear == null)
            {

                List<SocialSecurityRates>? lsr = SocialSecurityRates.GetList();
                List<TaxInformation>? lti = TaxInformation.GetList();
                if (lsr != null && lti != null)
                {
                    SocialSecurityRates? sr = lsr.Find(x => x.Year == Year);
                    TaxInformation? ti = lti.Find(x => x.Year == Year);

                    if (sr != null && ti != null)
                    {
                        TaxInformationOfYear = ti;
                        SocialSecurityRatesOfYear = sr;
                    }
                }

            }

            if (SocialSecurityRatesOfYear != null && InsuranceAdditionTotal == default)
            {
                InsuranceAdditionTotal = SocialSecurityRatesOfYear.EmployeeInsuranceBonusRate + SocialSecurityRatesOfYear.EmployerInsuranceBonusRate;
            }

        }

        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page(); // why fields are not correct??
            //}


            List<SocialSecurityRates>? lsr = SocialSecurityRates.GetList();
            List<TaxInformation>? lti = TaxInformation.GetList();
            if (lsr != null && lti != null)
            {
                SocialSecurityRates? sr = lsr.Find(x => x.Year == Year);
                TaxInformation? ti = lti.Find(x => x.Year == Year);

                if (sr != null && ti != null)
                {
                    TaxInformationOfYear = ti;
                    SocialSecurityRatesOfYear = sr;
                }
            }


            








            return RedirectToPage("Error");
        }
    }
}
