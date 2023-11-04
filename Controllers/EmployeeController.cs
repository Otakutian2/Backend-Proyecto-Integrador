using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employeeService;
        private readonly IRole _roleService;
        private readonly ICommand _commandService;
        private readonly IReceipt _receiptService;

        public EmployeeController(IEmployee employeeService, IRole roleService, ICommand commandService, IReceipt receiptService)
        {
            _employeeService = employeeService;
            _roleService = roleService;
            _commandService = commandService;
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeGet>>> GetEmployee()
        {
            return Ok((await _employeeService.GetAll()).Adapt<List<EmployeeGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeGet>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound("Empleado no encontrado");
            }

            var employeeGet = employee.Adapt<EmployeeGet>();

            return Ok(employeeGet);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<EmployeeGet>> UpdateEmployee(int id, [FromBody] EmployeeCreate employeeUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound("Empleado no encontrado");
            }

            employeeUpdate.User.Email = employeeUpdate.User.Email.ToLower();

            var isNotDniUnique = !await _employeeService.IsDniUnique(employeeUpdate.Dni, employee.Id);
            var isNotPhoneUnique = !await _employeeService.IsPhoneUnique(employeeUpdate.Phone, employee.Id);
            var isNotEmailUnique = !await _employeeService.IsEmailUnique(employeeUpdate.User.Email, employee.Id);

            if (isNotDniUnique && isNotPhoneUnique && isNotEmailUnique)
            {
                return Conflict("El DNI, teléfono y correo ya está en uso");
            }

            if (isNotDniUnique)
            {
                return Conflict("El DNI ya está en uso");
            }

            if (isNotPhoneUnique)
            {
                return Conflict("El teléfono ya está en uso");
            }

            if (isNotEmailUnique)
            {
                return Conflict("El email ya está en uso");
            }

            if (employee.RoleId != employeeUpdate.RoleId)
            {
                var role = await _roleService.GetById(employeeUpdate.RoleId);

                if (role == null)
                {
                    return NotFound("Rol no encontrado");
                }

                employee.RoleId = employeeUpdate.RoleId;
            }

            employee.FirstName = employeeUpdate.FirstName;
            employee.LastName = employeeUpdate.LastName;
            employee.Phone = employeeUpdate.Phone;
            employee.Dni = employeeUpdate.Dni;
            employee.User.Email = employeeUpdate.User.Email;

            await _employeeService.UpdateEmployee(employee);

            var getEmployee = (await _employeeService.GetById(id)).Adapt<EmployeeGet>();

            return Ok(getEmployee);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<EmployeeGet>> CreateEmployee([FromBody] EmployeeCreate employee)
        {
            if (!ModelState.IsValid) // Validar si el modelo es válido
            {
                return BadRequest(ModelState); // Devolver un BadRequest con los errores de validación
            }

            employee.User.Email = employee.User.Email.ToLower();

            var isNotDniUnique = !await _employeeService.IsDniUnique(employee.Dni);
            var isNotPhoneUnique = !await _employeeService.IsPhoneUnique(employee.Phone);
            var isNotEmailUnique = !await _employeeService.IsEmailUnique(employee.User.Email);

            if (isNotDniUnique && isNotPhoneUnique && isNotEmailUnique)
            {
                return Conflict("El DNI, teléfono y correo ya está en uso");
            }

            if (isNotDniUnique)
            {
                return Conflict("El DNI ya está en uso");
            }

            if (isNotPhoneUnique)
            {
                return Conflict("El teléfono ya está en uso");
            }

            if (isNotEmailUnique)
            {
                return Conflict("El email ya está en uso");
            }

            var role = await _roleService.GetById(employee.RoleId);

            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            var newEmployee = employee.Adapt<Employee>();

            await _employeeService.CreateEmployee(newEmployee);

            var getEmployee = (await _employeeService.GetById(newEmployee.Id)).Adapt<EmployeeGet>();

            return CreatedAtAction(nameof(GetEmployee), new { id = getEmployee.Id }, getEmployee);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound("Empleado no encontrado");
            }

            await _employeeService.DeleteEmployee(employee);

            return NoContent();
        }

        [HttpGet("{id}/number-commands")]
        public async Task<ActionResult<int>> GetNumberCommandInEmployee(int id)
        {
            var employeeCount = await _employeeService.Count(e => e.Id == id);

            if (employeeCount == 0)
            {
                return NotFound("Empleado no encontrado");
            }

            var count = await _commandService.Count(c => c.EmployeeId == id);

            return Ok(count);
        }

        [HttpGet("{id}/number-receipts")]
        public async Task<ActionResult<int>> GetNumberReceiptInEmployee(int id)
        {
            var employeeCount = await _employeeService.Count(e => e.Id == id);

            if (employeeCount == 0)
            {
                return NotFound("Empleado no encontrado");
            }

            var count = await _receiptService.Count(c => c.EmployeeId == id);

            return Ok(count);
        }
    }
}
