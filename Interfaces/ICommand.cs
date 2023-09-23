using project_backend.Models;
using System.Linq.Expressions;

namespace project_backend.Interfaces
{
    public interface ICommand
    {
        public Task<List<Command>> GetAll();
        public Task<Command> GetById(int id);
        public Task<bool> CreateCommand(Command command);
        public Task<bool> UpdateCommand(Command command);
        public Task<bool> DeleteCommand(Command command);
        public Task<bool> PrepareCommand(Command command);
        public Task<bool> PayCommand(Command command);
        public Task<int> Count(Expression<Func<Command, bool>> predicate = null);
        public Task<int> CommandDetailsCount(Expression<Func<CommandDetails, bool>> predicate = null);
        /*
  
       
        //public Task<GetCommandWithTable> GetCommandByTableId(int id);
        public Task<bool> UpdateCommandState(int id);*/
    }
}
