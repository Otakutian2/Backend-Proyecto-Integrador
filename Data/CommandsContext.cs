
using Microsoft.EntityFrameworkCore;
using project_backend.Models;

namespace project_backend.Data
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasOne(e => e.Employee)
                .WithMany(e => e.ReceiptCollection)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLowerCaseNamingConvention();
        }

        public DbSet<Employee> Employee { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<Receipt> Receipt { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<TableRestaurant> TableRestaurant { get; set; } = default!;
        public DbSet<CommandState> CommandState { get; set; } = default!;
        public DbSet<Command> Command { get; set; } = default!;
        public DbSet<CommandDetails> CommandDetails { get; set; } = default!;
        public DbSet<Establishment> Establishment { get; set; } = default!;
        public DbSet<Cash> Cash { get; set; } = default!;
        public DbSet<PaymentMethod> PaymentMethod { get; set; } = default!;
        public DbSet<ReceiptType> ReceiptType { get; set; } = default!;
        public DbSet<ReceiptDetails> ReceiptDetails { get; set; } = default!;
        public DbSet<Customer> Customer { get; set; } = default!;
    }
}
