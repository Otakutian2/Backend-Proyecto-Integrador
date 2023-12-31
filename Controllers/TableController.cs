﻿using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using proyecto_backend.Interfaces;
using proyecto_backend.Utils;
using proyecto_backend.Models;
using proyecto_backend.Enums;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TableController : ControllerBase
    {
        private readonly ITableRestaurant _tableService;
        private readonly ICommand _commandService;
        private readonly IAuth _authService;

        public TableController(ITableRestaurant tableService, ICommand commandService, IAuth authService)
        {
            _tableService = tableService;
            _commandService = commandService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableRestaurantGet>>> GetTable()
        {
            return Ok((await _tableService.GetAll()).Adapt<List<TableRestaurantGet>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TableRestaurantGet>> GetTable(int id)
        {
            var table = await _tableService.GetById(id);

            if (table == null)
            {
                return NotFound("Mesa no encontrada");
            }

            var TableRestaurantGet = table.Adapt<TableRestaurantGet>();

            return Ok(TableRestaurantGet);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<TableRestaurantGet>> CreateTable([FromBody] TableRestaurantPrincipal table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTable = table.Adapt<TableRestaurant>();
            newTable.State = TableStateEnum.Free.GetEnumMemberValue();

            await _tableService.CreateTable(newTable);

            var getTable = (await _tableService.GetById(newTable.Id)).Adapt<TableRestaurantGet>();

            return CreatedAtAction(nameof(GetTable), new { id = getTable.Id }, getTable);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TableRestaurantGet>> UpdateTable(int id, [FromBody] TableRestaurantUpdate table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateTable = await _tableService.GetById(id);

            if (updateTable == null)
            {
                return NotFound("Mesa no encontrada");
            }

            updateTable.SeatCount = table.SeatCount;
            updateTable.State = table.State.GetEnumMemberValue();

            await _tableService.UpdateTable(updateTable);

            var getTable = (await _tableService.GetById(id)).Adapt<TableRestaurantGet>();

            return Ok(getTable);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _tableService.GetById(id);

            if (table == null)
            {
                return NotFound("Mesa no encontrada");
            }

            if (table.State == TableStateEnum.Occupied.GetEnumMemberValue())
            {
                return BadRequest("No se puede eliminar una mesa ocupada");

            }

            await _tableService.DeleteTable(table);

            return NoContent();
        }

        [HttpGet("{id}/number-commands")]
        public async Task<ActionResult<int>> GetNumberCommandInTable(int id)
        {
            var tableCount = await _tableService.Count(t => t.Id == id);

            if (tableCount == 0)
            {
                return NotFound("Mesa no encontrada");
            }

            var count = await _commandService.Count(c => c.TableRestaurantId == id);

            return Ok(count);
        }

        [HttpGet("commands")]
        public async Task<ActionResult<List<TableRestaurantWithCommand>>> GetCommandCollection()
        {
            var tableCollectionWithCommand = (await _tableService.GetTableCollectionWithCommand(_authService.GetCurrentUserRole())).Adapt<List<TableRestaurantWithCommand>>();
            var commandCollectionWithoutCommand = (await _commandService.GetCommandCollectionWithoutTable(_authService.GetCurrentUserRole())).Adapt<List<TableRestaurantWithCommand>>();
            tableCollectionWithCommand.AddRange(commandCollectionWithoutCommand);

            return Ok(tableCollectionWithCommand);
        }
    }
}
