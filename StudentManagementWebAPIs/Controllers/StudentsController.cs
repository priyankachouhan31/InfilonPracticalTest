using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementWebAPIs.Models;

namespace StudentManagementWebAPIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public StudentsController(AppDBContext context)
        {
            _context = context;
        }

        #region WebAPI
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddStudent([FromBody] Students model)
        {
            try
            {
                if (CheckAccessToken(HttpContext) == false)
                {
                    var response = new
                    {
                        name = "no_token",
                        message = "No token information.",
                        statusCode = 401
                    };
                    return StatusCode(401, response);
                }
                if (model == null)
                {
                    return StatusCode(400, "Bad request");
                }
                Students student = new Students
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    EmailId = model.EmailId,
                    ClassIds = model.ClassIds
                };
                _context.Students.Add(student);
                int primaryKey = await _context.SaveChangesAsync();
                if (primaryKey > 0)
                {
                    return StatusCode(201, primaryKey);
                }
                else
                {
                    return StatusCode(400, "Failed to add");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return StatusCode(500, "exception " + ex.Message);
            }
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateStudent([FromBody] Students model)
        {
            try
            {
                if (CheckAccessToken(HttpContext) == false)
                {
                    var response = new
                    {
                        name = "no_token",
                        message = "No token information.",
                        statusCode = 401
                    };
                    return StatusCode(401, response);
                }
                if (model == null)
                {
                    return StatusCode(400, "Bad request");
                }
                Students student = await _context.Students
            .FirstOrDefaultAsync(x => x.StudentId == model.StudentId);

                if (student == null)
                {
                    return StatusCode(404, "Student not found");
                }

                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.PhoneNumber = model.PhoneNumber;
                student.EmailId = model.EmailId;

                if (!string.IsNullOrWhiteSpace(model.ClassIds))
                {
                    if (string.IsNullOrWhiteSpace(student.ClassIds))
                    {
                        student.ClassIds = model.ClassIds;
                    }
                    else
                    {
                        student.ClassIds = student.ClassIds + "," + model.ClassIds;
                    }
                }

                await _context.SaveChangesAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return StatusCode(500, "exception " + ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (CheckAccessToken(HttpContext) == false)
                {
                    var response = new
                    {
                        name = "no_token",
                        message = "No token information.",
                        statusCode = 401
                    };
                    return StatusCode(401, response);
                }
                List<Students> students = await _context.Students.ToListAsync();
                return StatusCode(200, students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            try
            {
                if (CheckAccessToken(HttpContext) == false)
                {
                    var response = new
                    {
                        name = "no_token",
                        message = "No token information.",
                        statusCode = 401
                    };
                    return StatusCode(401, response);
                }
                Students student = await _context.Students.Where(t => t.StudentId == Id).FirstOrDefaultAsync();
                return StatusCode(200, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            try
            {
                if (!CheckAccessToken(HttpContext))
                {
                    return StatusCode(401, new
                    {
                        name = "no_token",
                        message = "No token information.",
                        statusCode = 401
                    });
                }

                Students student = await _context.Students
                    .FirstOrDefaultAsync(x => x.StudentId == Id);

                if (student == null)
                {
                    return StatusCode(404, "Student not found");
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return StatusCode(200, "Student deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return StatusCode(500, "Exception: " + ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private bool CheckAccessToken(HttpContext httpContext)
        {
            if (httpContext != null &&
                httpContext.Request != null &&
                httpContext.Request.Headers != null &&
                httpContext.Request.Headers.ContainsKey("Authorization") == true &&
                string.IsNullOrWhiteSpace(httpContext.Request.Headers["Authorization"]) == false)
            {
                string accessToken = httpContext.Request.Headers["Authorization"];
                string bearerPart = "Bearer ";

                if (accessToken.ToLower().StartsWith(bearerPart.ToLower()) == false)
                {
                    return false;
                }
                accessToken = accessToken.Remove(0, bearerPart.Length);
                if (string.IsNullOrWhiteSpace(accessToken) == true)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
