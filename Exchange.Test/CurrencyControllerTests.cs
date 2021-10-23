using Exchange.Controllers;
using Exchange.Core.Services;
using Exchange.Items.Exceptions;
using Exchange.Items.Models.Enum;
using Exchange.Items.Models.Response;
using Exchange.Items.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Exchange.Test
{
    public class CurrencyControllerTests
    {
        [Fact]
        public async Task CurrencyController_GetCurrencies_ShouldReturnOk_ExpectedValueAsync()
        {
            //Arrange
            var currencyServiceMock = new Mock<ICurrencyService>();

            currencyServiceMock
                .Setup(x => x.GetAllCurrenciesAsync(It.IsAny<GetAllCurrenciesQuery>()))
                .ReturnsAsync(() => new List<DailyRateDto>()
                {
                    new DailyRateDto()
                    {
                        Code = Codes.USD.ToString(),
                        Rate = 35
                    }
                });

            var currencyController = CreateInstance(currencyServiceMock);

            //Act
            var result = await currencyController.GetCurrencies(new GetAllCurrenciesQuery());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CurrencyController_GetCurrencies_ShouldThrowException()
        {
            try
            {
                //Arrange
                var currencyServiceMock = new Mock<ICurrencyService>();

                currencyServiceMock
                    .Setup(x => x.GetAllCurrenciesAsync(It.IsAny<GetAllCurrenciesQuery>()))
                    .ReturnsAsync(() => throw new BusinessException("Hata"));

                var currencyController = CreateInstance(currencyServiceMock);

                //Act
                var result = await currencyController.GetCurrencies(new GetAllCurrenciesQuery());
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsType<BusinessException>(ex);
            }
        }

        [Fact]
        public async Task CurrencyController_GetCurrencyRates_ShouldReturnOk_ExpectedValueAsync()
        {
            //Arrange
            var currencyServiceMock = new Mock<ICurrencyService>();

            currencyServiceMock
                .Setup(x => x.GetCurrencyRatesAsync(It.IsAny<GetCurrencyRatesQuery>()))
                .ReturnsAsync(() => new List<CurrencyRateDto>()
                {
                    new CurrencyRateDto()
                    {
                        Code = Codes.USD.ToString(),
                        Rate = 10,
                        Date = new DateTime(2021,10,20).ToString("dd.MM.yyyy")
                    },
                    new CurrencyRateDto()
                    {
                        Code = Codes.USD.ToString(),
                        Rate = 12,
                        Date = new DateTime(2021,10,21).ToString("dd.MM.yyyy")
                    }
                });

            var currencyController = CreateInstance(currencyServiceMock);

            //Act
            var result = await currencyController.GetCurrencyRates(Codes.USD.ToString());

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CurrencyController_GetCurrencyRates_ShouldReturnThrowException()
        {
            try
            {
                //Arrange
                var currencyServiceMock = new Mock<ICurrencyService>();

                currencyServiceMock
                    .Setup(x => x.GetCurrencyRatesAsync(It.IsAny<GetCurrencyRatesQuery>()))
                    .ReturnsAsync(() => throw new BusinessException("Hata"));

                var currencyController = CreateInstance(currencyServiceMock);

                //Act
                var result = await currencyController.GetCurrencyRates(Codes.USD.ToString());
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsType<BusinessException>(ex);
            }
        }

        private CurrencyController CreateInstance(Mock<ICurrencyService> currencyServiceMock)
        {
            var currencyController = new CurrencyController(currencyServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return currencyController;
        }
    }
}
