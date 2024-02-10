using System.ComponentModel.DataAnnotations;

namespace RestfulJsonLearnerAPI.Models
{
    public class JsonDataModel
    {
        [Key]
        public int CeshbonID { get; set; }
        public string MisparZihuyMishtamesh { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain 10 digits.")]
        public string Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
    }
}
