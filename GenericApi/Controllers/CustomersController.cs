#nullable disable
using GenericApi.Data;
using GenericApi.Services;
using Microsoft.AspNetCore.Mvc;
using GenericApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GenericApi.Controllers
{
    // example implementing generic controller and generic repository
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : DefaultGenericController<ApplicationDbContext, Customer, int>
    {
        public CustomersController(IGenericRepository<ApplicationDbContext, Customer> repo) : base(repo) {}
        // using unitOfWork
        // public CustomersController(IUnitOfWork<ApplicationDbContext> repo) : base(repo) {}

    }
}
