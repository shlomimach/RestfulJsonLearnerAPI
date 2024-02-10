using Microsoft.EntityFrameworkCore;
using RestfulJsonLearnerAPI.Models;

namespace RestfulJsonLearnerAPI.Data
{
    public class JsonContext :DbContext
    {
        public JsonContext(DbContextOptions<JsonContext> options) 
            : base(options) 
        {

        }


        public DbSet<JsonEntity> JsonTable { get; set; }

    }
}
