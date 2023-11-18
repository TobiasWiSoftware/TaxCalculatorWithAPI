using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Formats.Asn1;
using System;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class MainService : IMainService
    {
        public MainService() { }
        public BillingOutput Calculation(BillingInput billingInput)
        {
            BillingOutput billingOutput = new();

            billingOutput.Calculation(billingInput);


            return billingOutput;
        }
    }
}
