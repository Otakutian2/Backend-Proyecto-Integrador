using Mapster;
using Microsoft.EntityFrameworkCore;
using project_backend.Data;
using project_backend.Enums;
using project_backend.Interfaces;
using project_backend.Models;
using project_backend.Schemas;
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
                command.CommandStateId = (int)CommandStateEnum.Prepared;
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
                command.CommandStateId = (int)CommandStateEnum.Paid;
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

        public async Task<List<TableRestaurantWithCommand>> GetCommandCollectionWithoutTable(string role)
        {
            List<TableRestaurantWithCommand> collection = new();
            var commands = await _context.Command.Where(c => c.TableRestaurantId == null && c.CommandStateId != (int)CommandStateEnum.Paid)
                .Include(c => c.Employee.User)
                .Include(c => c.Employee.Role)
                .Include(c => c.CommandState)
                .Include(c => c.CommandDetailsCollection).ToListAsync();
            string[] roles = { "Cajero", "Cocinero" };

            foreach (var command in commands)
            {
                TableRestaurantWithCommand tableWithCommand = new();
                tableWithCommand.Table = null;

                if (command != null)
                {
                    if (role == "Cajero" && command.CommandStateId != (int)CommandStateEnum.Prepared)
                    {
                        continue;
                    }

                    tableWithCommand.Command = command.Adapt<CommandForTable>();
                    tableWithCommand.Command.QuantityOfDish = command.CommandDetailsCollection.Sum(cd => cd.DishQuantity);
                }
                else
                {
                    if (roles.Contains(role))
                    {
                        continue;
                    }
                }

                collection.Add(tableWithCommand);
            }

            return collection;
        }
    }
}
