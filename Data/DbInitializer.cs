using project_backend.Models;
using project_backend.Utils;

namespace project_backend.Data
{
    public class DbInitializer
    {
        public async static void Initialize(CommandContext context)
        {
            if (!context.Category.Any())
            {
                await context.Category.AddRangeAsync(new List<Category>
                {
                    new Category() { Id = "C-001", Name = "Bebidas" },
                    new Category() { Id = "C-002", Name = "Hamburguesas" },
                    new Category() { Id = "C-003", Name = "Postres" },
                    new Category() { Id = "C-004", Name = "Sopas" }
                });

                context.SaveChanges();
            }

            if (!context.Establishment.Any())
            {
                await context.Establishment.AddRangeAsync(new List<Establishment>
                {
                    new Establishment() {
                        Name = "Sangunchería Wong",
                        Address = "Av.Izaguirre",
                        Phone = "942850902",
                        Ruc = "20509311412"
                    }
                });

                context.SaveChanges();
            }
            if (!context.CommandState.Any())
            {
                await context.CommandState.AddRangeAsync(new List<CommandState>
                {
                    new CommandState() {
                        Name = "Generado"
                    },
                    new CommandState() {
                        Name = "Preparado"
                    },
                     new CommandState() {
                        Name = "Pagado"
                    },
                });

                context.SaveChanges();
            }
            if (!context.PaymentMethod.Any())
            {
                await context.PaymentMethod.AddRangeAsync(new List<PaymentMethod>
                {
                    new PaymentMethod() { Name = "En efectivo" },
                    new PaymentMethod() { Name = "BCP" },
                    new PaymentMethod() { Name = "BBVA" },
                    new PaymentMethod() { Name = "Scotiabank" },
                    new PaymentMethod() { Name = "Interbank" },
                });

                context.SaveChanges();
            }

            if (!context.Customer.Any())
            {
                await context.Customer.AddAsync(new Customer()
                {
                    FirstName = "Cliente",
                    LastName = string.Empty,
                    Dni = string.Empty
                }); ;
            }

            if (!context.Role.Any())
            {
                await context.Role.AddRangeAsync(new List<Role>
                {
                    new Role() {Name = "Administrador"},
                    new Role() {Name = "Mesero"},
                    new Role() {Name = "Cajero"},
                    new Role() {Name = "Cocinero"},
                    new Role() {Name = "Gerente"},
                });

                context.SaveChanges();
            }

            if (!context.ReceiptType.Any())
            {
                await context.ReceiptType.AddRangeAsync(new List<ReceiptType>
                {
                    new ReceiptType() {Name = "Boleta"},
                    new ReceiptType() {Name = "Factura"}
                });

                context.SaveChanges();
            }

            if (!context.Employee.Any())
            {
                await context.Employee.AddRangeAsync(new List<Employee>
                {
                    new Employee()
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Phone = "999999999",
                        Dni = "75123512",
                        RoleId = 1,
                        User = new User()
                        {
                            Email = "admin@admin.com",
                            Password = SecurityUtils.HashPassword("admin")
                        }
                    },
                    new Employee()
                    {
                        FirstName = "Mesero",
                        LastName = "Mesero",
                        Phone = "999999998",
                        Dni = "72341264",
                        RoleId = 2,
                        User = new User()
                        {
                            Email = "mesero@mesero.com",
                            Password = SecurityUtils.HashPassword("mesero")
                        }
                    },
                    new Employee()
                    {
                        FirstName = "Cajero",
                        LastName = "Cajero",
                        Phone = "999999997",
                        Dni = "71235912",
                        RoleId = 3,
                        User = new User()
                        {
                            Email = "cajero@cajero.com",
                            Password = SecurityUtils.HashPassword("cajero")
                        }
                    },
                    new Employee()
                    {
                        FirstName = "Cocinero",
                        LastName = "Cocinero",
                        Phone = "999999996",
                        Dni = "64231231",
                        RoleId = 4,
                        User = new User()
                        {
                            Email = "cocinero@cocinero.com",
                            Password = SecurityUtils.HashPassword("cocinero")
                        }
                    },
                    new Employee()
                    {
                        FirstName = "Gerente",
                        LastName = "Gerente",
                        Phone = "999999995",
                        Dni = "85158921",
                        RoleId = 5,
                        User = new User()
                        {
                            Email = "gerente@gerente.com",
                            Password = SecurityUtils.HashPassword("gerente")
                        }
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
