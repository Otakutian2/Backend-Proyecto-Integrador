using Mapster;
using Microsoft.AspNetCore.Mvc;
using project_backend.Enums;
using project_backend.Interfaces;
using project_backend.Models;
using project_backend.Schemas;
using project_backend.Utils;

namespace project_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommand _commandService;
        private readonly ITableRestaurant _tableService;
        private readonly IEmployee _employeeService;
        private readonly IDish _dishService;
        private readonly IAuth _authService;

        public CommandController(ICommand commandService, ITableRestaurant tableService, IDish dishService, IEmployee employeeService, IAuth authService)
        {
            _commandService = commandService;
            _employeeService = employeeService;
            _tableService = tableService;
            _dishService = dishService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandGet>>> GetCommand()
        {
            return Ok((await _commandService.GetAll()).Adapt<List<CommandGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommandGet>> GetCommand(int id)
        {
            var command = await _commandService.GetById(id);

            if (command == null)
            {
                return NotFound("Comanda no encontrada");
            }

            CommandGet commandGet = command.Adapt<CommandGet>();

            return Ok(commandGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCommand([FromBody] CommandCreate command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TableRestaurant table = null;

            if (command.TableRestaurantId != null)
            {
                table = await _tableService.GetById(command.TableRestaurantId.Value);

                if (table == null)
                {
                    return NotFound("Mesa no encontrada");
                }

                if (table.State == TableStateEnum.Occupied.GetEnumMemberValue())
                {
                    return BadRequest("Mesa ocupada, eliga otra");
                }

                if (table.SeatCount < command.SeatCount)
                {
                    return BadRequest("La cantidad de asientos en la comanda no puede exceder la cantidad permitida de la mesa");
                }
            }
            else
            {
                command.SeatCount = null;
            }

            var ids = command.CommandDetailsCollection.Select(cd => cd.DishId);
            var idsCount = await _dishService.Count(d => ids.Contains(d.Id));

            if (idsCount != ids.Count())
            {
                return BadRequest("No se encontró al menos un plato en la lista o hay elementos repetidos");
            }

            var newCommand = command.Adapt<Command>();

            newCommand.EmployeeId = _authService.GetCurrentUserId();

            await _commandService.CreateCommand(newCommand);

            if (table != null)
            {
                table.State = TableStateEnum.Occupied.GetEnumMemberValue();
                await _tableService.UpdateTable(table);
            }

            var getCommand = (await _commandService.GetById(newCommand.Id)).Adapt<CommandGet>();

            return CreatedAtAction(nameof(GetCommand), new { id = getCommand.Id }, getCommand);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCommand(int id, [FromBody] CommandUpdate command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commandUpdate = await _commandService.GetById(id);

            if (commandUpdate == null)
            {
                return NotFound("Comanda no encontrada");
            }

            if (commandUpdate.CommandStateId == (int)CommandStateEnum.Paid)
            {
                return BadRequest("No se puedes actualizar una comanda ya pagada");
            }
            if (commandUpdate.TableRestaurantId != null)
            {
                if (commandUpdate.TableRestaurant.SeatCount < command.SeatCount)
                {
                    return BadRequest("La cantidad de asientos en la comanda no puede exceder la cantidad permitida de la mesa");
                }
            }
            else
            {
                command.SeatCount = null;
            }

            var ids = command.CommandDetailsCollection.Select(cd => cd.DishId);
            var idsCount = await _dishService.Count(d => ids.Contains(d.Id));

            if (idsCount != ids.Count())
            {
                return BadRequest("No se encontró al menos un plato de la lista o hay platos repetidos");
            }

            if (command.SeatCount != null)
            {
                commandUpdate.SeatCount = command.SeatCount.Value;
            }

            commandUpdate.CommandDetailsCollection = command.CommandDetailsCollection.Adapt<List<CommandDetails>>();

            await _commandService.UpdateCommand(commandUpdate);

            var getCommand = (await _commandService.GetById(commandUpdate.Id)).Adapt<CommandGet>();

            return Ok(getCommand);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommand(int id)
        {
            var command = await _commandService.GetById(id);

            if (command == null)
            {
                return NotFound("Comanda no encontrada");
            }

            if (command.CommandStateId == (int)CommandStateEnum.Paid)
            {
                return BadRequest("No se puedes eliminar una comanda ya pagada");
            }

            await _commandService.DeleteCommand(command);

            TableRestaurant table = command.TableRestaurant;

            if (table != null)
            {
                table.State = TableStateEnum.Free.GetEnumMemberValue();
                await _tableService.UpdateTable(table);
            }

            return NoContent();
        }

        [HttpPut("prepare-command/{id}")]
        public async Task<ActionResult> PrepareCommand(int id)
        {
            var command = await _commandService.GetById(id);

            if (command == null)
            {
                return NotFound("Comanda no encontrada");
            }

            if (command.CommandStateId != (int)CommandStateEnum.Generated)
            {
                return BadRequest("No se puede preparar una comanda que no tenga un estado de 'Generado'");
            }

            await _commandService.PrepareCommand(command);

            return Ok("La comanda se está preparando");
        }
    }
}

/*
[HttpPut("update-state/{id}")]
public async Task<ActionResult<CommandGet>> updateState(int id)
{
    bool command = await _commandService.UpdateCommandState(id);

    if (!command)
    {
        return NotFound("Se ha enconctrado un error");
    }

    return Ok();
}


[HttpPut("{id}")]
public async Task<ActionResult<Command>> UpdateCommand(int id, [FromBody] CommandPrincipal value)
{
    if (!ModelState.IsValid) { return BadRequest(ModelState); }
    var updateCommand = await _commandService.GetById(id);
    if (updateCommand == null) { return NotFound("Comanda no encontrada"); }

    if (updateCommand.TableRestaurantId != value.TableRestaurantId)
    {
        //Validar mesa
        var newTable = await _tableService.GetById(value.TableRestaurantId);
        if (newTable == null)
        {
            return BadRequest("No existe la mesa");
        }

        if (newTable.State == "Ocupado")
        {
            return BadRequest("Mesa ocupada, eliga otra");
        }
        newTable.State = "Ocupado";
        await _tableService.UpdateTable(newTable);
        var tableOld = await _tableService.GetById(updateCommand.TableRestaurantId);
        tableOld.State = "Libre";
        await _tableService.UpdateTable(tableOld);

    }

    updateCommand.TableRestaurantId = value.TableRestaurantId;
    updateCommand.SeatCount = value.SeatCount;
    await _commandService.UpdateCommand(updateCommand);
    var getCommand = await _commandService.GetById(id);
    return Ok(getCommand);
}

[HttpPut("Prepare-Command/{id}")]
public async Task<ActionResult<Command>> PrepareCommand(int id)
{
    if (!ModelState.IsValid) { return BadRequest(ModelState); }

    var updateCommand = await _commandService.GetById(id);

    if (updateCommand == null) { return NotFound("Comanda no encontrada"); }

    if (updateCommand.CommandStateId == 3)
    {
        return BadRequest("La comanda ya ha sido facturada, eliga otra");
    }

    updateCommand.CommandStateId = 2;

    await _commandService.UpdateCommand(updateCommand);
    return Ok("Se ha cambiado el estado de comanda correctamente");
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteCommand(int id)
{
    await _commandService.DeleteCommand(id);
    return NoContent();
}


[HttpGet("GetCommandByTableId/{id}")]
public async Task<ActionResult<GetCommandWithTable>> GetCommandByTableId(int id)
{
    var command = await _commandService.GetCommandByTableId(id);

    if (command == null)
    {
        return NotFound("Comanda no encontrada");
    }

    return Ok(command);
}


*/
