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
    public class CreateCustomerTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly IFixture _fixture;
        private readonly Mock<IMapper> _mockMapper;

        public CreateCustomerTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _controller = new CustomerController(_mockRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void CreateCustomer_ReturnsOkResult_WhenValidInput()
        {
            //Arrange
            var customersMock = _fixture.Create<Customer>();
            var customersDtoMock = _fixture.Create<CustomerDto>();
            var customersCreateDtoMock = _fixture.Create<CustomerCreateDto>();
            _mockRepo.Setup(x => x.CreateCustomer(customersMock)).Returns(true);
            _mockMapper.Setup(x => x.Map<Customer>(customersCreateDtoMock)).Returns(customersMock);
            _mockMapper.Setup(x => x.Map<CustomerDto>(customersMock)).Returns(customersDtoMock);

            //Act
            var result = _controller.CreateCustomer(customersCreateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<CreatedResult>();
            result.Result.As<CreatedResult>().Value.As<APIResponse<CustomerDto>>().Result
                .Should()
                .NotBeNull()
                .And.BeOfType(customersDtoMock.GetType());
            _mockRepo.Verify(x => x.CreateCustomer(customersMock), Times.Once);
        }

        [TestMethod]
        public void CreateCustomer_ReturnsBadRequest_WhenInvalidInput()
        {
            //Arrange
            var customersCreateDtoMock = _fixture.Create<CustomerCreateDto>();
            _controller.ModelState.AddModelError("", "Please enter valid details!");

            //Act
            var result = _controller.CreateCustomer(customersCreateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [TestMethod]
        public void CreateCustomer_ReturnsBadRequest_WhenErrorOccured()
        {
            //Arrange
            var customersMock = _fixture.Create<Customer>();
            var customersCreateDtoMock = _fixture.Create<CustomerCreateDto>();
            _mockRepo.Setup(x => x.CreateCustomer(customersMock)).Returns(false);
            _mockMapper.Setup(x => x.Map<Customer>(customersCreateDtoMock)).Returns(customersMock);

            //Act
            var result = _controller.CreateCustomer(customersCreateDtoMock);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _mockRepo.Verify(x => x.CreateCustomer(customersMock), Times.Once);
        }
    }
}
