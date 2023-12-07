using Xunit;
using Moq;
using TaxCalculatorAPI.Controllers;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using TestTaxCalculatorAPI;

namespace TaxCalculatorAPI.Tests
{
    public class MainControllerTest2023
    {
        private readonly MainController _controller;
        private readonly MainService _mainService;
        private readonly List<TestData> _testData = TestData.GetTestCases();



        public MainControllerTest2023()
        {
            _mainService = new MainService();
            _controller = new MainController(_mainService);
            SocialSecurityRates.LoadDataFromJsonForTesting();
            TaxInformation.LoadDataFromJsonForTesting();

        }

        [Fact]
        public void TestControllerAndService()
        {
            if (_testData.Count < 1)
            {
                Assert.Fail("No test data");
            }

            _testData.ForEach(d =>
            {
                // Arrange
                var billingInput = d.BillingInput;
                BillingOutput expectedOutput = d.BillingOutput; // expected result
                                                                                                         // Act
                var result = _controller.TransferAmount(billingInput);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                Assert.NotNull(okResult);

                BillingOutput? actualResult = okResult.Value as BillingOutput;
                Assert.NotNull(actualResult);

                Assert.True(actualResult.CheckForTestingWithTolerance(expectedOutput).Item1);
            });

        }

        [Fact]
        public void TransferInput_ReturnsOkResult_WithTuple()
        {
            // Arrange
            int year = 2021;
            var tuple = Tuple.Create(new SocialSecurityRates(), new TaxInformation()); // Erwartetes Ergebnis
            //_mockService.Setup(s => s.FetchSocialAndTaxData(year)).Returns(tuple);

            // Act
            var result = _controller.TransferInput(year);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(tuple, okResult.Value);
        }
    }
}
