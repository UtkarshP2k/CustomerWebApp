using AutoFixture;
using AutoMapper;
using Customer_API.Controllers;
using Customer_DataAccess.Repostitory.IRepository;
using Customer_Models;
using Customer_Models.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_UnitTests.CustomerControllerTests
{
    [TestClass]
    public class GetAllCustomersTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerRepository> _mockRepo;
        private readonly IFixture _fixture;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllCustomersTests()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _controller = new CustomerController(_mockRepo.Object, _mockMapper.Object);
        }

        [TestMethod]
        public void GetAllCustomers_ReturnsOkResult_WhenDataFound()
        {
            //Arrange
            var customersMock = _fixture.Create<List<Customer>>();
            var customersDtoMock = _fixture.Create<List<CustomerDto>>();
            _mockRepo.Setup(x => x.GetAllCustomers()).Returns(customersMock);
            _mockMapper.Setup(x => x.Map<List<CustomerDto>>(customersMock)).Returns(customersDtoMock);

            //Act
            var result = _controller.GetAllCustomers();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<List<CustomerDto>>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value.As<APIResponse<List<CustomerDto>>>().Result
                .Should()
                .NotBeNull()
                .And.BeOfType(customersDtoMock.GetType());
            _mockRepo.Verify(x => x.GetAllCustomers(), Times.Once);
        }

        [TestMethod]
        public void GetAllCustomers_ReturnsNotFound_WhenDataNotFound()
        {
            //Arrange
            List<Customer> customersMock = null;
            _mockRepo.Setup(x => x.GetAllCustomers()).Returns(customersMock);

            //Act
            var result = _controller.GetAllCustomers();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<APIResponse<List<CustomerDto>>>>();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _mockRepo.Verify(x => x.GetAllCustomers(), Times.Once);
        }
    }
}
