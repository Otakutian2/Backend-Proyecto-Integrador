namespace project_backend.Schemas
{
    public class RolePrincipal
    {
        public string Name { get; set; }
    }

    public class RoleGet : RolePrincipal
    {
        public int Id { get; set; }
    }
}
