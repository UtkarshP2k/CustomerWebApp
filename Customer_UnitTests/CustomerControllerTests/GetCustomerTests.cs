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
    public class GetCustomerTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly IFixture _fixture;
        private readonly Mock<IMapper> _mockMapper;

        public GetCustomerTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _controller = new CustomerController(_mockRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void GetCustomer_ReturnsOkResult_WhenValidId()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var customerMock = _fixture.Create<Customer>();
            var customerDtoMock = _fixture.Create<CustomerDto>();
            _mockRepo.Setup(x => x.GetCustomer(id)).Returns(customerMock);
            _mockMapper.Setup(x => x.Map<CustomerDto>(customerMock)).Returns(customerDtoMock);

            //Act
            var result = _controller.GetCustomer(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value.As<APIResponse<CustomerDto>>().Result
                .Should()
                .NotBeNull()
                .And.BeOfType(customerDtoMock.GetType());
            _mockRepo.Verify(x => x.GetCustomer(id), Times.Once);
        }

        [TestMethod]
        public void GetCustomer_ReturnsBadRequest_WhenInvalidId()
        {
            //Arrange
            var id = 0;

            //Act
            var result = _controller.GetCustomer(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [TestMethod]
        public void GetCustomer_ReturnsNotFound_WhenDataNotFound()
        {
            //Arrange
            var id = _fixture.Create<int>();
            Customer customerMock = null;
            _mockRepo.Setup(x => x.GetCustomer(id)).Returns(customerMock);

            //Act
            var result = _controller.GetCustomer(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<CustomerDto>>>();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _mockRepo.Verify(x => x.GetCustomer(id), Times.Once);
        }

    }
}
