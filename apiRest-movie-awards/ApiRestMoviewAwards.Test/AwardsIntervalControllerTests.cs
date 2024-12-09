using ApiRestMovieAwards.BFF.Controllers.v1;
using Application.Features.AwardsIntervalFeatures.Queries;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApiRestMoviewAwards.Test
{
    public class AwardsIntervalControllerTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnOkResultWithData()
        {
            // Arrange
            var expectedResult = new AwardsIntervalDTO
            {
                Min = new List<AwardsIntervalItemDTO>
            {
                new AwardsIntervalItemDTO { Producer = "Producer 1", PreviousWin = 2000, FollowingWin = 2005 },
            },
                Max = new List<AwardsIntervalItemDTO>
            {
                new AwardsIntervalItemDTO { Producer = "Producer 2", PreviousWin = 1995, FollowingWin = 2020 },
            }
            };

            var mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(m => m.Send(It.IsAny<AwardsIntervalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var controller = new AwardsIntervalController();

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualData = Assert.IsType<AwardsIntervalDTO>(okResult.Value); 
            Assert.Equal(expectedResult.Min.Count, actualData.Min.Count);
            Assert.Equal(expectedResult.Max.Count, actualData.Max.Count);
        }
    }
}
