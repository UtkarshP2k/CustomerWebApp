using AutoMapper;
using Customer_DataAccess.Repostitory.IRepository;
using Customer_Models;
using Customer_Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Customer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;
        public CustomerController(ICustomerRepository repository, IMapper mapper)
        {
            _customerRepo = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse<List<CustomerDto>>> GetAllCustomers()
        {
            var response = new APIResponse<List<CustomerDto>>();
            var customers = _customerRepo.GetAllCustomers();

            if (customers == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessage = "No customers found!";
                return NotFound(response);
            }


            response.StatusCode = HttpStatusCode.OK;
            response.Result = _mapper.Map<List<CustomerDto>>(customers);
            return Ok(response);
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse<CustomerDto>> GetCustomer(int id)
        {
            var response = new APIResponse<CustomerDto>();

            if (id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Please enter valid Id!";
                return BadRequest(response);
            }
                

            var customer = _customerRepo.GetCustomer(id);

            if (customer == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessage = "No customer found with this id!";
                return NotFound(response);

            }

            response.StatusCode = HttpStatusCode.OK;
            response.Result = _mapper.Map<CustomerDto>(customer);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<APIResponse<CustomerDto>> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            var response = new APIResponse<CustomerDto>();

            if (!ModelState.IsValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Please enter valid details!";
                return BadRequest(response);
            }
                

            var customer = _mapper.Map<Customer>(customerDto);
            var result = _customerRepo.CreateCustomer(customer);

            if (result)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Result = _mapper.Map<CustomerDto>(customer);
                return Created("api/Customer/" + customer.Id, response);

            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Error while creating customer!";
                return BadRequest(response);
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse<CustomerDto>> UpdateCustomer(int id, [FromBody] CustomerUpdateDto customerDto)
        {
            var response = new APIResponse<CustomerDto>();

            if (!ModelState.IsValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Please enter valid details!";
                return BadRequest(response);
            }

            if (!_customerRepo.Exists(id))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessage = "No customer found with this id!";
                return NotFound(response);
            }

            var customer = _mapper.Map<Customer>(customerDto);
            var result = _customerRepo.UpdateCustomer(customer);

            if (result)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Result = _mapper.Map<CustomerDto>(customer);
                return Ok(response);

            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Error while updating customer!";
                return BadRequest(response);
            }

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse<CustomerDto>> DeleteCustomer(int id)
        {
            var response = new APIResponse<CustomerDto>();

            if (id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Please enter valid Id!";
                return BadRequest(response);
            }

            var customer = _customerRepo.GetCustomer(id);

            if (customer == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessage = "No customer found with this id!";
                return NotFound(response);

            }

            var result = _customerRepo.DeleteCustomer(customer);

            if (result)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Result = _mapper.Map<CustomerDto>(customer);
                return Ok(response);

            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = "Error while deleting customer!";
                return BadRequest(response);
            }

        }


    }
}
