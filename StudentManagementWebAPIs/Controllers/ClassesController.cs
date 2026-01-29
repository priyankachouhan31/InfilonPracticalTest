using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementWebAPIs.Models;
using System.Security.Claims;

namespace StudentManagementWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ClassesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpPost("bulkUpload")]
        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File size should not exceed 5 MB.");

            if (!file.FileName.EndsWith(".csv"))
                return BadRequest("Only CSV files are allowed.");

            List<Classes> classes = new List<Classes>();
            var errors = new List<string>();

            using var reader = new StreamReader(file.OpenReadStream());
            int rowNumber = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                rowNumber++;

                if (rowNumber == 1)
                    continue;

                var values = line.Split(',');

                if (values.Length < 2)
                {
                    errors.Add($"Invalid column count at row {rowNumber}");
                    continue;
                }

                var name = values[0]?.Trim();
                var description = values.Length > 1 ? values[1]?.Trim() : null;
                var studentIds = values.Length > 2 ? string.Join(",", values.Skip(2)) : null;

                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add($"Class Name is required at row {rowNumber}");
                    continue;
                }

                classes.Add(new Classes
                {
                    Name = name,
                    Description = description,
                    StudentIds = studentIds
                });
            }

            if (errors.Any())
            {
                return BadRequest(new
                {
                    Message = "CSV contains invalid data",
                    Errors = errors
                });
            }

            await _context.Classes.AddRangeAsync(classes);
            await _context.SaveChangesAsync();

            return StatusCode(200, new
            {
                Message = "Classes imported successfully",
                TotalInserted = classes.Count
            });
        }
    }
}
