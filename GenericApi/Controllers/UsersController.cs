using Microsoft.AspNetCore.Mvc;
using GenericApi.Models;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenericApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        
        // GET: api/<CustomersController>
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            //return new string[] { "value1", "value2" };
            return Customer.Customers;
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public Customer? Get(int id)
        {
            return Customer.Customers.Find(c => c.Id == id);
            
        }

        // POST api/<CustomersController>
        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            customer.Id = Customer.Customers.Count +1;
            Customer.Customers.Add(customer);
         
        }

        // PUT api/<CustomersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Customer value)
        {
            var item = Customer.Customers.Find(c => c.Id == id);
            if(item != null)
            {
                item.Email = value.Email;
                item.Address = value.Address;   
                item.Phone = value.Phone;   
            }

        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           var item = Customer.Customers.Find(c => c.Id == id);
            Customer.Customers.Remove(item!);
        }
    }
}
