using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CustomerRepo
    {
        FuminiHotelManagementContext _context;  
        public CustomerRepo(FuminiHotelManagementContext context)
        {
            _context = context;
        }
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
       
        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public class LoginRepository
        {
            private readonly FuminiHotelManagementContext _context;

            public LoginRepository(FuminiHotelManagementContext context)
            {
                _context = context;
            }

            public Customer? CheckLogin(string email, string password)
            {
                return _context.Customers
                    .FirstOrDefault(c => c.Email == email && c.Password == password);
            }
        }

    }
}
