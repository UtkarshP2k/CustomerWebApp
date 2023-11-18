using AutoFixture;
using AutoMapper;
using Customer_API.Controllers;
using Customer_DataAccess.Repostitory.IRepository;
using Customer_Models.Dto;
using Customer_Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Customer_UnitTests.CustomerControllerTests
{
    [TestClass]
    public class UpdateCustomerTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly IFixture _fixture;
        private readonly Mock<IMapper> _mockMapper;

        public UpdateCustomerTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _controller = new CustomerController(_mockRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void UpdateCustomer_ReturnsOkResult_WhenValidInput()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var customersMock = _fixture.Create<Customer>();
            var customersDtoMock = _fixture.Create<CustomerDto>();
            var customersUpdateDtoMock = _fixture.Create<CustomerUpdateDto>();
            _mockRepo.Setup(x => x.UpdateCustomer(customersMock)).Returns(true);
            _mockRepo.Setup(x => x.Exists(id)).Returns(true);
            _mockMapper.Setup(x => x.Map<Customer>(customersUpdateDtoMock)).Returns(customersMock);
            _mockMapper.Setup(x => x.Map<CustomerDto>(customersMock)).Returns(customersDtoMock);

            //Act
            var result = _controller.UpdateCustomer(id,customersUpdateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value.As<APIResponse<CustomerDto>>().Result
                .Should()
                .NotBeNull()
                .And.BeOfType(customersDtoMock.GetType());
            _mockRepo.Verify(x => x.UpdateCustomer(customersMock), Times.Once);
        }

        [TestMethod]
        public void UpdateCustomer_ReturnsBadRequest_WhenInvalidInput()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var customersMock = _fixture.Create<Customer>();
            var customersUpdateDtoMock = _fixture.Create<CustomerUpdateDto>();
            _controller.ModelState.AddModelError("", "Please enter valid details!");

            //Act
            var result = _controller.UpdateCustomer(id, customersUpdateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [TestMethod]
        public void UpdateCustomer_ReturnsNotFound_WhenDataNotFound()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var customersMock = _fixture.Create<Customer>();
            var customersUpdateDtoMock = _fixture.Create<CustomerUpdateDto>();
            _mockRepo.Setup(x => x.Exists(id)).Returns(false);

            //Act
            var result = _controller.UpdateCustomer(id, customersUpdateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
        }

        [TestMethod]
        public void UpdateCustomer_ReturnsBadRequest_WhenErrorOccured()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var customersMock = _fixture.Create<Customer>();
            var customersUpdateDtoMock = _fixture.Create<CustomerUpdateDto>();
            _mockRepo.Setup(x => x.UpdateCustomer(customersMock)).Returns(false);
            _mockRepo.Setup(x => x.Exists(id)).Returns(true);
            _mockMapper.Setup(x => x.Map<Customer>(customersUpdateDtoMock)).Returns(customersMock);

            //Act
            var result = _controller.UpdateCustomer(id, customersUpdateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _mockRepo.Verify(x => x.UpdateCustomer(customersMock), Times.Once);
        }
    }
}
