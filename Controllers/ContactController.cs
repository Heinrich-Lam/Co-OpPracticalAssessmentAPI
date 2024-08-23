using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PracticalAssessmentAPI.Classes;
using PracticalAssessmentAPI.Classes.Parameters;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PracticalAssessmentAPI.Controllers
{
    [Authorize] // Secure all endpoints in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        clsDatabase db = new clsDatabase();

        #region "Security"
        [AllowAnonymous] // Allow access without authentication
        [Route("Read/readSecurity")]
        [HttpPost]
        public DataTable readSecurity(ContactParameters param)
        {
            return db.readSecurity(param.Name, param.Password);
        }

        [AllowAnonymous] // Allow access without authentication
        [HttpPost("login")]
        public IActionResult Login([FromBody] ContactParameters loginRequest)
        {
            // Validate username and password with your database here
            var user = db.readSecurity(loginRequest.Name, loginRequest.Password);

            if (user == null || user.Rows.Count == 0)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("HeinrichLambrechtsCoOPAssessment247");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("NAME", user.Rows[0]["NAME"].ToString()) }), // Assuming the user has an Id column
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "https://localhost:7145",
                Audience = "https://localhost:44318",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
        #endregion

        [Route("Read/readContacts")]
        [HttpPost]
        public DataTable readContact(ContactParameters param)
        {
            return db.readContacts(param.History);
        }

        [Route("Read/readContactInfo")]
        [HttpPost]
        public DataTable readContactInfo(ContactParameters param)
        {
            return db.readContactInfo(param.EntryID);
        }

        [Route("Insert/insertContact")]
        [HttpPost]
        public DataTable insertContact(ContactParameters param)
        {
            return db.insertContact(param.Name, param.Email, param.Phone, param.Address, param.History);
        }

        [Route("Update/updateContact")]
        [HttpPost]
        public DataTable updateContact(ContactParameters param)
        {
            return db.updateContact(param.EntryID, param.Name, param.Email, param.Phone, param.Address);
        }

        [Route("Delete/deleteContact")]
        [HttpPost]
        public DataTable deleteContact(ContactParameters param)
        {
            return db.deleteContact(param.EntryID);
        }

        #region "Register User."
        [Route("Read/readUser")]
        [HttpPost]
        public DataTable readUser(ContactParameters param)
        {
            return db.readUser(param.History);
        }

        [Route("Read/readUserInfo")]
        [HttpPost]
        public DataTable readUserInfo(ContactParameters param)
        {
            return db.readUserInfo(param.EntryID);
        }

        [Route("Insert/insertUser")]
        [HttpPost]
        public DataTable insertUser(ContactParameters param)
        {
            return db.insertUser(param.Name, param.Surname, param.Email, param.Password, param.History);
        }

        [Route("Update/updateUser")]
        [HttpPost]
        public DataTable updateUser(ContactParameters param)
        {
            return db.updateUser(param.EntryID, param.Name, param.Surname, param.Email, param.Password);
        }

        [Route("Delete/deleteUser")]
        [HttpPost]
        public DataTable deleteUser(ContactParameters param)
        {
            return db.deleteUser(param.EntryID);
        }
        #endregion

        #region "Token Authentication for requested user."
        public class AuthMiddleware
        {
            private readonly RequestDelegate _next;

            public AuthMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext httpContext)
            {
                string authHeader = httpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    string token = authHeader.Substring("Bearer ".Length).Trim();
                    try
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes("HeinrichLambrechtsCoOPAssessment247");
                        var validationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero
                        };
                        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                        httpContext.User = (ClaimsPrincipal)principal.Identity;
                    }
                    catch
                    {
                        // Handle invalid token
                    }
                }
                await _next(httpContext);
            }
        }


        [Authorize] // Keeping it for clarity, although it's redundant if the whole controller is authorized
        [HttpGet("Read/readContact")]
        public IActionResult ReadContact([FromQuery] ContactParameters param)
        {
            // Set the Content-Type header
            HttpContext.Response.ContentType = "application/json";

            // Validate the parameters
            if (param == null || string.IsNullOrEmpty(param.Name) || string.IsNullOrEmpty(param.Password))
            {
                return BadRequest(new { message = "Invalid parameters." });
            }

            // Fetch contact info from the database
            var contactInfo = db.readContactSecurity(param.Name, param.Password);

            if (contactInfo == null)
            {
                return NotFound(new { message = "Contact not found." });
            }

            return Ok(contactInfo); // Assuming contactInfo is already in JSON-serializable format
        }
        #endregion

    }
}
