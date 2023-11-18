using Customer_DataAccess.Data;
using Customer_DataAccess.Repostitory.IRepository;
using Customer_Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_DataAccess.Repostitory
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(ApplicationDbContext context,ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public bool CreateCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                int result = _context.SaveChanges();

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }
        }

        public bool DeleteCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Remove(customer);
                int result = _context.SaveChanges();

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _context.Customers.Any(x => x.Id == id);
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Update(customer);
                int result = _context.SaveChanges();

                return result == 1;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return false;
            }
        }
    }
}
