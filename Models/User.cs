namespace project_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public static string GeneratePassword(Employee employee)
        {
            var rand = new Random();

            int charactersToExtract = 2;
            int randomNum = rand.Next(1, employee.LastName.Length - charactersToExtract + 1);
            string extractedCharacters = employee.LastName.Substring(randomNum, charactersToExtract);
            string capitalizedExtractedCharacters = string.Concat(extractedCharacters[..1].ToUpper(), extractedCharacters.AsSpan(1));

            return capitalizedExtractedCharacters + "$" + rand.Next(1000, 5001);
        }
    }
}
