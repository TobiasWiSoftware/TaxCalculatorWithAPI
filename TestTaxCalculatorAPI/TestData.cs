using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TaxCalculatorAPI.Controllers;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TestTaxCalculatorAPI
{
    class TestData
    {
        private static List<TestData> LData = FillLTestData();
        public BillingInput BillingInput { get; set; }
        public BillingOutput BillingOutput { get; set; }
        private TestData(BillingInput ip, BillingOutput op)
        {
            BillingInput = ip;
            BillingOutput = op;
        }

        public static List<TestData> GetTestCases()
        {
            return LData;
        }

        private static List<TestData> FillLTestData()
        {
            LData = new List<TestData>();

            BillingInput billingInput = new BillingInput(2023, 0, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            BillingOutput expectedOutput = new BillingOutput(billingInput,0,0,0,0,0,0,0); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 1000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput,81, 23, 93, 13, 0, 0, 0); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 1500, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 121.50m, 34.50m, 139.5m, 19.50m, 30.33m, 30.33m, 30.33m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 2000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 162, 46, 186, 26, 122.75m, 122.75m, 122.75m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 3000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 243, 69, 279, 39, 337, 337, 337    ); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 4000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 324, 92, 372, 52, 582.08m, 582.08m, 582.08m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 6000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 403.99m, 114.71m, 558, 78, 1202.33m, 1202.33m, 1202.33m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 8000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 403.99m, 114.71m, 678.90m, 94.90m, 1987.83m, 1987.83m, 1987.83m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));
            
            billingInput = new BillingInput(2023, 10000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 403.99m, 114.71m, 678.90m, 94.90m, 2827.83m, 2827.83m, 2827.83m); // expected result

            LData.Add(new TestData(billingInput, expectedOutput)); 
            
            billingInput = new BillingInput(2023, 30000, true, 1, 30, false, 0, true.ToString(), 0, 1.6m, "true", true.ToString());
            expectedOutput = new BillingOutput(billingInput, 403.99m, 114.71m, 678.90m, 94.90m, 11394.66m, 626.70m, 0); // expected result

            LData.Add(new TestData(billingInput, expectedOutput));



            return LData;
        }

    }
}
