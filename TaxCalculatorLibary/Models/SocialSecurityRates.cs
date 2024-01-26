using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace TaxCalculatorLibary.Models
{
    public class SocialSecurityRates
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public decimal EmployeeInsuranceRate { get; set; }
        public decimal EmployerInsuranceRate { get; set; }
        public decimal EmployeeInsuranceBonusRate { get; set; }
        public decimal EmployerInsuranceBonusRate { get; set; }
        public decimal EmployeeInsuranceCareRate { get; set; }
        public decimal EmployeeInsuranceCareChildFreeRate { get; set; }
        public decimal EmployeeInsuranceCareChildDiscountRate { get; set; }
        public int EmployeeMinChildrenDiscount { get; set; }
        public int EmployeeMaxChildrenDiscount { get; set; }
        public decimal EmployerInsuranceCareRate { get; set; }
        public decimal EmployeePensionRate { get; set; }
        public decimal EmployerPensionRate { get; set; }
        public decimal EmployeeUnemploymentRate { get; set; }
        public decimal EmployerUnemploymentRate { get; set; }
        public decimal InsuranceMaxGross { get; set; }
        public decimal PensionAndUnimploymentMaxGross { get; set; }

        public SocialSecurityRates()
        {

        }


        public SocialSecurityRates(int year, decimal employeeInsuranceRate, decimal employerInsuranceRate,
                                   decimal employeeInsuranceBonusRate, decimal employerInsuranceBonusRate,
                                   decimal employeeInsuranceCareRate, decimal employeeInsuranceCareChildFreeRate,
                                   decimal employeeInsuranceCareChildDiscountRate, int employeeMinChildrenDiscount,
                                   int employeeMaxChildrenDiscount, decimal employerInsuranceCareRate,
                                   decimal employeePensionRate, decimal employerPensionRate,
                                   decimal employeeUnemploymentRate, decimal employerUnemploymentRate,
                                   decimal insuranceMaxGross, decimal pensionAndUnimploymentMaxGross
                                   )
        {
            Year = year;
            EmployeeInsuranceRate = employeeInsuranceRate;
            EmployerInsuranceRate = employerInsuranceRate;
            EmployeeInsuranceBonusRate = employeeInsuranceBonusRate;
            EmployerInsuranceBonusRate = employerInsuranceBonusRate;
            EmployeeInsuranceCareRate = employeeInsuranceCareRate;
            EmployeeInsuranceCareChildFreeRate = employeeInsuranceCareChildFreeRate;
            EmployeeInsuranceCareChildDiscountRate = employeeInsuranceCareChildDiscountRate;
            EmployeeMinChildrenDiscount = employeeMinChildrenDiscount;
            EmployeeMaxChildrenDiscount = employeeMaxChildrenDiscount;
            EmployerInsuranceCareRate = employerInsuranceCareRate;
            EmployeePensionRate = employeePensionRate;
            EmployerPensionRate = employerPensionRate;
            EmployeeUnemploymentRate = employeeUnemploymentRate;
            EmployerUnemploymentRate = employerUnemploymentRate;
            InsuranceMaxGross = insuranceMaxGross;
            PensionAndUnimploymentMaxGross = pensionAndUnimploymentMaxGross;

        }


        public static void LoadDataFromJsonForTesting()
        {
        //    string path = Path.Combine("../../../../", "TaxCalculatorAPI", "Data", "SocialSecurityRates.json");

        //    using (StreamReader r = new(path))
        //    {
        //        string s = r.ReadToEnd();
        //        List<SocialSecurityRates>? list = JsonConvert.DeserializeObject<List<SocialSecurityRates>>(s);

        //        if (list != null)
        //        {
        //            LSocialSecurityRates ??= list;
        //        }
         //   }
        }

        public static SocialSecurityRates? GetDataFromYear(int year)
        {
        //    return LSocialSecurityRates != null ? LSocialSecurityRates.Find(x => x.Year == year) : null;
        return null;
        }


    }

}
