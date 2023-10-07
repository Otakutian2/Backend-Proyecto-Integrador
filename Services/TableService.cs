using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using project_backend.Data;
using project_backend.Dto;
using project_backend.Interfaces;
using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Services
{
    public class TableService : ITableRestaurant
    {
        private readonly CommandContext _context;

        public TableService(CommandContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTable(TableRestaurant table)
        {
            bool result = false;

            try
            {
                _context.TableRestaurant.Add(table);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> DeleteTable(TableRestaurant table)
        {
            bool result = false;

            try
            {
                _context.TableRestaurant.Remove(table);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<TableRestaurant> GetById(int id)
        {
            var table = await _context.TableRestaurant.FirstOrDefaultAsync(t => t.Id == id);

            return table;
        }

        public async Task<List<TableRestaurant>> GetAll()
        {
            var tables = await _context.TableRestaurant.ToListAsync();

            return tables;
        }

        public async Task<bool> UpdateTable(TableRestaurant tableUpdate)
        {
            bool result = false;

            try
            {
                _context.Entry(tableUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
        /*
        public async Task<int> GetNumberCommandInTable(int tableId)
        {
            var table = await _context.TableRestaurant
            .Include(c => c.CommandCollection)
            .Where(c => c.Id == tableId)
            .FirstOrDefaultAsync();

            return table.CommandCollection.Count;
        }
        */
        public async Task<int> Count(Expression<Func<TableRestaurant, bool>> predicate = null)
        {
            return await (predicate != null ? _context.TableRestaurant.CountAsync(predicate) : _context.TableRestaurant.CountAsync());
        }

        /*
        public async Task<List<TableComands>> GetTableCollectionWithCommand(string role)
        {
            List<TableComands> response = new List<TableComands>();

            List<TableRestaurant> tables = await _context.TableRestaurant.
            Include(c => c.CommandCollection).ThenInclude(c => c.Employee)
           .Include(c => c.CommandCollection).ThenInclude(c => c.CommandState)
            .ToListAsync();

            string[] roles = { "Cajero", "Cocinero" };

            foreach (var table in tables)
            {
                TableComands tableComands = new TableComands();

                tableComands.NumTable = table.Id;
                tableComands.NumSeats = table.SeatCount;
                tableComands.StateTable = table.State;

                if (table.CommandCollection.Any() && table.CommandCollection != null)
                {
                    tableComands.hasCommand = true;

                    List<CommandCustom> commandsCustoms = GetCommandCustoms(table.CommandCollection, ref tableComands, role);

                    if (commandsCustoms.Count == 0)
                    {
                        continue;
                    }

                    tableComands.Command = commandsCustoms;
                }
                else
                {
                    if (roles.Contains(role))
                    {
                        continue;
                    }

                    tableComands.hasCommand = false;

                    tableComands.Command = new List<CommandCustom> { };
                }

                response.Add(tableComands);
            }

            return response;
        }

        public async Task<TableComands> GetTableCollectionWithCommandByTableId(int id)
        {
            TableComands response = new TableComands();

            TableRestaurant table = await _context.TableRestaurant.
            Include(c => c.CommandCollection).ThenInclude(c => c.Employee)
           .Include(c => c.CommandCollection).ThenInclude(c => c.CommandState)
           .Where(c => c.Id == id).FirstOrDefaultAsync();

            response.NumTable = table.Id;
            response.NumSeats = table.SeatCount;
            response.StateTable = table.State;

            return response;
        }

        public List<CommandCustom> GetCommandCustoms(List<Command> listCommand, ref TableComands tableComands, string role)
        {
            List<CommandCustom> commands = new List<CommandCustom>();

            foreach (var command in listCommand)
            {
                if (role == "Cocinero" && command.CommandStateId == 3)
                {
                    continue;
                }

                Console.WriteLine("HOLAAAAAAAAAAAAAA" + command);

                if (role == "Cajero" && command.CommandStateId != 2)
                {
                    continue;
                }

                CommandCustom commandsCustom = new CommandCustom();

                commandsCustom.Id = command.Id;
                commandsCustom.CantSeats = command.SeatCount;
                commandsCustom.PrecTotOrder = command.TotalOrderPrice;
                commandsCustom.CreatedAt = command.CreatedAt.ToString("dd/MM/yyyy");
                commandsCustom.EmployeeId = command.EmployeeId;
                commandsCustom.EmployeeName = command.Employee.FirstName + " " + command.Employee.LastName;
                commandsCustom.StatescommandId = command.CommandStateId;
                commandsCustom.StatesCommandName = command.CommandState.Name;

                List<CommandDetails> detailsComands = _context.CommandDetails.
                Include(c => c.Dish).ThenInclude(c => c.Category).
                Where(c => c.CommandId == command.Id).ToList();

                List<DetailCommandCustom> details = new List<DetailCommandCustom>();
                if (detailsComands.Any() && detailsComands != null)
                {
                    foreach (var detail in detailsComands)
                    {
                        DetailCommandCustom detailsComandCustom = new DetailCommandCustom();
                        detailsComandCustom.Id = detail.Id;
                        detailsComandCustom.CantDish = detail.DishQuantity;
                        detailsComandCustom.PrecDish = detail.DishPrice;
                        detailsComandCustom.Dish = new DishCustom()
                        {
                            Id = detail.Dish.Id,
                            CategoryId = detail.Dish.CategoryId,
                            CategoryName = detail.Dish.Category.Name,
                            ImgDish = detail.Dish.Image,
                            NameDish = detail.Dish.Name,
                            PriceDish = detail.Dish.Price
                        };

                        detailsComandCustom.Observation = detail.Observation;
                        detailsComandCustom.PrecOrder = detail.OrderPrice;

                        details.Add(detailsComandCustom);
                    }

                    commandsCustom.DetailsComand = details;
                }
                else
                {
                    commandsCustom.DetailsComand = new List<DetailCommandCustom> { };
                }

                if (command.CommandState.Id.Equals(1) || command.CommandState.Id.Equals(2))
                {
                    tableComands.commandActive = commandsCustom;
                }

                commands.Add(commandsCustom);
            }

            return commands;
        }*/
    }
}
