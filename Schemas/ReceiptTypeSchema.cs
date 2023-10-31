namespace proyecto_backend.Schemas
{
    public class ReceiptTypePrincipal
    {
        public string Name { get; set; }
    }

    public class ReceiptTypeGet : ReceiptTypePrincipal
    {
        public int Id { get; set; }
    }
}
