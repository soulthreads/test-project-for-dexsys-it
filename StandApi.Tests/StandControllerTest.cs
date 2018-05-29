using System;
using Xunit;
using Moq;
using StandApi.Models;
using StandApi.Controllers;
using StandApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StandApi.Tests
{
    public class StandControllerTest
    {
        private Mock<IStandRepository> standRepoMock;
        private StandController controller;
 
        public StandControllerTest()
        {
            standRepoMock = new Mock<IStandRepository>();
            controller = new StandController(standRepoMock.Object);
        }

        [Fact]
        public async void GetTest_ReturnsEntriesList()
        {
            var mockEntriesList = new List<StandEntry>
            {
                new StandEntry { Url = "localhost", DateTime = DateTime.Now, Status = StandStatus.StartingUp, Error = "" },
                new StandEntry { Url = "localhost", DateTime = DateTime.Now, Status = StandStatus.Working, Error = "" },
            };
            standRepoMock
                .Setup(repo => repo.GetAllEntries())
                .Returns(Task.FromResult(mockEntriesList));

            var result = await controller.GetAll();

            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(mockEntriesList, actionResult.Value);
        }

        [Fact]
        public async void GetTest_ReturnsNotFound_WhenStandDoesNotExist()
        {
            var url = "localhost";
            standRepoMock
                .Setup(repo => repo.GetLastEntryForStand(url))
                .Returns(Task.FromResult<StandEntry>(null));

            var result = await controller.GetLastByUrl(url);
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void AddEntryTest_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var mockEntry = new StandEntry { Url = "localhost" };
            controller.ModelState.AddModelError("Description", "This field is required");

            var result = await controller.AddEntry(mockEntry);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(new SerializableError(controller.ModelState), actionResult.Value);
        }
    }
}
