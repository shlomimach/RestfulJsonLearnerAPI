using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestfulJsonLearnerAPI.Data;
using RestfulJsonLearnerAPI.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RestfulJsonLearnerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonController : ControllerBase
    {

        private readonly JsonContext _context;

        public JsonController(JsonContext context)
        {
            _context = context;
        }
        /// <summary>
        /// אנחנו מקבלים את הנתונים כמודל JsonDataModel,
        /// ממירים אותם למחרוזת JSON,
        /// ואז שומרים את המחרוזת בעמודה JsonData בטבלה JsonTable 
        /// באמצעות המודל JsonEntity.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonDataModel jsonData)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // המרה ושמירה של הנתונים בפורמט JSON
                var jsonString = JsonSerializer.Serialize(jsonData);
                var jsonEntity = new JsonEntity { JsonData = jsonString };
                _context.JsonTable.Add(jsonEntity);
                await _context.SaveChangesAsync();

                // יצירת תגובה עם תאריך ותיאור
                var response = CreateSuccessResponse();
                return Ok(response);
            }
            catch (Exception ex)
            {
                // לוג או טיפול בשגיאה
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
        private object CreateSuccessResponse()
        {
            return new
            {
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Description = "Success"
            };
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // חיפוש הרשומה לפי ה-id שסופק
            var jsonEntity = await _context.JsonTable.FindAsync(id);
            if (jsonEntity == null)
            {
                // אם הרשומה לא נמצאת, החזר תגובת 404 Not Found
                return NotFound($"Record with ID {id} not found.");
            }

            try
            {
                // מחיקת הרשומה מה-DbContext
                _context.JsonTable.Remove(jsonEntity);
                // שמירת השינויים במסד הנתונים
                await _context.SaveChangesAsync();

                // החזרת תגובת 200 OK עם מסר על הצלחת המחיקה
                return Ok($"Record with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                // במקרה של שגיאה, החזר תגובת שגיאה פנימית של השרת
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // חיפוש הרשומה לפי ה-id שסופק
            var jsonEntity = await _context.JsonTable.FindAsync(id);
            if (jsonEntity == null)
            {
                // אם הרשומה לא נמצאת, החזר תגובת 404 Not Found
                return NotFound($"Record with ID {id} not found.");
            }

            try
            {
                // המרה של המחרוזת JSON השמורה בעמודה JsonData לאובייקט
                var jsonData = JsonSerializer.Deserialize<JsonDataModel>(jsonEntity.JsonData);

                // החזרת הנתונים ללקוח בפורמט JSON
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                // במקרה של שגיאה בהמרה או בשליפת הנתונים, החזר תגובת שגיאה פנימית של השרת
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

