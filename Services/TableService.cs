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

        public async Task<int> GetNumberCommandInTable(int tableId)
        {
            var table = await _context.TableRestaurant
            .Include(c => c.CommandCollection)
            .Where(c => c.Id == tableId)
            .FirstOrDefaultAsync();

            return table.CommandCollection.Count;
        }

        public async Task<int> Count(Expression<Func<TableRestaurant, bool>> predicate = null)
        {
            return await (predicate != null ? _context.TableRestaurant.CountAsync(predicate) : _context.TableRestaurant.CountAsync());
        }

        public async Task<List<TableRestaurantWithCommand>> GetTableCollectionWithCommand(string role)
        {
            List<TableRestaurantWithCommand> collection = new();
            List<TableRestaurant> tables = await _context.TableRestaurant.ToListAsync();
            string[] roles = { "Cajero", "Cocinero" };

            // Mesas con comandas
            foreach (var table in tables)
            {
                TableRestaurantWithCommand tableWithCommand = new();
                tableWithCommand.Table = table.Adapt<TableRestaurantGet>();

                var command = await _context.Command.Where(c => c.TableRestaurantId == table.Id && c.CommandStateId != (int)CommandStateEnum.Paid)
                    .Include(c => c.Employee.User)
                    .Include(c => c.Employee.Role)
                    .Include(c => c.CommandState)
                    .Include(c => c.CommandDetailsCollection).FirstOrDefaultAsync();

                if (command != null)
                {
                    if (roles.Contains(role) && command.CommandStateId != (int)CommandStateEnum.Prepared)
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
