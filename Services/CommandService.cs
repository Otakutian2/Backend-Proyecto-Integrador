using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using project_backend.Enums;
using project_backend.Interfaces;
using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Services
{
    public class CommandService : ICommand
    {
        private readonly CommandContext _context;

        public CommandService(CommandContext context)
        {
            _context = context;
        }

        public async Task<List<Command>> GetAll()
        {
            List<Command> command = await _context.Command
                .Include(c => c.TableRestaurant)
                .Include(c => c.Employee.User)
                .Include(c => c.Employee.Role)
                .Include(c => c.CommandState)
                .Include(c => c.CommandDetailsCollection).ThenInclude(d => d.Dish).ThenInclude(ca => ca.Category)
                .ToListAsync();

            return command;
        }

        public async Task<Command> GetById(int id)
        {
            var command = await _context.Command
                .Include(c => c.TableRestaurant)
                .Include(c => c.Employee.User)
                .Include(c => c.Employee.Role)
                .Include(c => c.CommandState)
                .Include(c => c.CommandDetailsCollection).ThenInclude(d => d.Dish).ThenInclude(ca => ca.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            return command;
        }

        public async Task<bool> CreateCommand(Command command)
        {
            using var transaction = _context.Database.BeginTransaction();
            bool result = false;

            try
            {
                command.CommandStateId = (int)CommandStateEnum.Generated;

                decimal totalOrderPrice = 0;

                var dishIds = command.CommandDetailsCollection.Select(c => c.DishId).ToList();
                var dishPrices = await _context.Dish.Where(d => dishIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, d => d.Price);

                foreach (var c in command.CommandDetailsCollection)
                {
                    c.DishPrice = dishPrices[c.DishId];
                    c.OrderPrice = c.DishPrice * c.DishQuantity;
                    totalOrderPrice += c.OrderPrice;
                }

                command.TotalOrderPrice = totalOrderPrice;

                _context.Command.Add(command);
                await _context.SaveChangesAsync();

                transaction.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
            }

            return result;
        }

        public async Task<bool> UpdateCommand(Command command)
        {
            using var transaction = _context.Database.BeginTransaction();
            bool result = false;

            try
            {
                decimal totalOrderPrice = 0;

                var dishIds = command.CommandDetailsCollection.Select(c => c.DishId).ToList();
                var dishPrices = await _context.Dish.Where(d => dishIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, d => d.Price);

                foreach (var c in command.CommandDetailsCollection)
                {
                    c.DishPrice = dishPrices[c.DishId];
                    c.OrderPrice = c.DishPrice * c.DishQuantity;
                    totalOrderPrice += c.OrderPrice;
                }

                command.TotalOrderPrice = totalOrderPrice;

                _context.Entry(command).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                transaction.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
            }

            return result;
        }

        public async Task<bool> DeleteCommand(Command command)
        {
            bool result = false;

            try
            {
                _context.Command.Remove(command);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> PrepareCommand(Command command)
        {
            bool result = false;

            try
            {
                command.CommandStateId = ((int)CommandStateEnum.Prepared);
                _context.Entry(command).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public async Task<bool> PayCommand(Command command)
        {
            bool result = false;

            try
            {
                command.CommandStateId = ((int)CommandStateEnum.Paid);
                _context.Entry(command).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
        public async Task<int> Count(Expression<Func<Command, bool>> predicate = null)
        {
            return await (predicate != null ? _context.Command.CountAsync(predicate) : _context.Command.CountAsync());
        }

        public async Task<int> CommandDetailsCount(Expression<Func<CommandDetails, bool>> predicate = null)
        {
            return await (predicate != null ? _context.CommandDetails.CountAsync(predicate) : _context.CommandDetails.CountAsync());
        }

        /*
     

        public async Task<GetCommandWithTable> GetCommandByTableId(int id)
        {
            GetCommandWithTable command = new GetCommandWithTable();

            Command commandS = await _context.Command
           .Include(c => c.TableRestaurant)
           .Include(c => c.Employee)
           .Include(c => c.CommandState)
           .Include(c => c.CommandDetailsCollection).ThenInclude(d => d.Dish).ThenInclude(ca => ca.Category)
           .FirstOrDefaultAsync(c => c.TableRestaurant.Id == id && c.CommandState.Id != 3 && c.TableRestaurant.State.Equals("Ocupado"));

            if (commandS is null)
            {
                TableRestaurant table = await _context.TableRestaurant.FirstOrDefaultAsync(t => t.Id == id && t.State.Equals("Libre"));

                int idd = _context.Command.Any() ? _context.Command.Max(c => c.Id) + 1 : 1;
                if (table != null)
                {
                    command = new GetCommandWithTable()
                    {
                        Id = 0,
                        CantSeats = 0,
                        CreatedAt = "",
                        EmployeeId = 0,
                        EmployeeName = "",
                        NumSeats = table.SeatCount,
                        NumTable = table.Id,
                        PrecTotOrder = 0,
                        StateTable = table.State,
                        StatescommandId = 0,
                        StatesCommandName = CommandStateEnum.Generated.ToString(),
                        isCommandActive = false,
                        DetailsComand = new List<DetailCommandCustom>()
                    };

                    return command;
                }
                else
                {
                    return null;
                }



            };

            command.Id = commandS.Id;
            command.NumTable = commandS.TableRestaurant.Id;
            command.NumSeats = commandS.TableRestaurant.SeatCount;
            command.CreatedAt = commandS.CreatedAt.ToString("dd/MM/yyyy");
            command.StatescommandId = commandS.CommandState.Id;
            command.StatesCommandName = commandS.CommandState.Name;
            command.CantSeats = commandS.SeatCount;
            command.EmployeeId = commandS.Employee.Id;
            command.EmployeeName = commandS.Employee.FirstName + " " + commandS.Employee.LastName;
            command.PrecTotOrder = commandS.TotalOrderPrice;
            command.isCommandActive = true;
            command.StateTable = commandS.TableRestaurant.State;


            if (commandS.CommandDetailsCollection.Any())
            {
                command.DetailsComand = commandS.CommandDetailsCollection.Select(d => new DetailCommandCustom
                {
                    Id = d.Id,
                    CantDish = d.DishQuantity,
                    PrecDish = d.DishPrice,
                    Observation = d.Observation,
                    Dish = new DishCustom
                    {
                        Id = d.Dish.Id,
                        ImgDish = d.Dish.Image,
                        PriceDish = d.Dish.Price,
                        CategoryId = d.Dish.Category.Id,
                        CategoryName = d.Dish.Category.Name,
                        NameDish = d.Dish.Name
                    },
                    PrecOrder = d.OrderPrice,

                }).ToList();
            }
            else
            {
                command.DetailsComand = new List<DetailCommandCustom>();
            }

            return command;

        }
    }*/
    }
}
