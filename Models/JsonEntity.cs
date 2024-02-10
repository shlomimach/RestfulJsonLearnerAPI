using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestfulJsonLearnerAPI.Models
{
    public class JsonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CeshbonID { get; set; }

        public string JsonData { get; set; }
    }
}
