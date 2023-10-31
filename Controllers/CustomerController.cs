using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerService;

        public CustomerController(ICustomer customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerGet>>> GetCustomer()
        {
            return Ok((await _customerService.GetAll()).Adapt<List<CustomerGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGet>> GetCustomer(int id)
        {
            var customer = await _customerService.GetById(id);

            if (customer == null)
            {
                return NotFound("Cliente no encontrado");
            }

            var customerGet = customer.Adapt<CustomerGet>();

            return Ok(customerGet);
        }

        [HttpGet("first")]
        public async Task<ActionResult<CustomerGet>> GetFirstCustomer()
        {
            var customer = (await _customerService.GetFirstOrDefault()).Adapt<CustomerGet>();
            return Ok(customer);
        }

        [HttpGet("dni/{id}")]
        public async Task<ActionResult<CustomerGet>> GetCustomerFindByDni(string id)
        {
            var customer = (await _customerService.FindCustomerByDni(id)).Adapt<CustomerGet>();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerGet>> CreateCustomer([FromBody] CustomerPrincipal customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isNotDniUnique = !await _customerService.IsDniUnique(customer.Dni);

            if (isNotDniUnique)
            {
                return Conflict("El DNI ya está en uso");
            }

            var newCustomer = customer.Adapt<Customer>();
            await _customerService.CreateCustomer(newCustomer);

            var getCustomer = (await _customerService.GetById(newCustomer.Id)).Adapt<CustomerGet>();

            return CreatedAtAction(nameof(GetCustomer), new { id = getCustomer.Id }, getCustomer);
        }
    }
}
