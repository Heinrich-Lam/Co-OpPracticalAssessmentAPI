using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticalAssessmentAPI.Classes;
using PracticalAssessmentAPI.Classes.Parameters;
using System.Data;

namespace PracticalAssessmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        clsDatabase db = new clsDatabase();

        #region "Security"
        [Route("Read/readSecurity")]
        [HttpPost]
        public DataTable readSecurity(ContactParameters param)
        {
            return db.readSecurity(param.Name, param.Password, param.Access);
        }
        #endregion

        [Route("Read/readContact")]
        [HttpPost]
        public DataTable readContact(ContactParameters param)
        {
            return db.readContact(param.Name);
        }

        [Route("Insert/insertContact")]
        [HttpPost]
        public DataTable insertContact(ContactParameters param)
        {
            return db.insertContact(param.Name, param.Email, param.Phone, param.Address);
        }

        [Route("Update/updateContact")]
        [HttpPost]
        public DataTable updateContact(ContactParameters param)
        {
            return db.updateContact(param.Name, param.Email, param.Phone, param.Address);
        }

        [Route("Delete/deleteContact")]
        [HttpPost]
        public DataTable deleteContact(ContactParameters param)
        {
            return db.deleteContact(param.EntryID);
        }
    }
}
