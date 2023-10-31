namespace proyecto_backend.Schemas
{
    public class CommandStatePrincipal
    {
        public string Name { get; set; }
    }

    public class CommandStateGet : CommandStatePrincipal
    {
        public int Id { get; set; }
    }
}
