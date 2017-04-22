using ByggKostnad.Forms.Data;
using Microsoft.AspNetCore.Mvc;
using ByggKostnad.Forms.Emails;
using System.Threading.Tasks;
using Byggkostnad.Forms.Models;
using GlobalPhoneContext = GlobalPhone.Context;
using PhoneNumber = GlobalPhone.Number;

namespace ByggKostnad.Forms.Controllers
{
    [Route("api/[controller]")]
    public class FormsController : Controller
    {
        private readonly GlobalPhoneContext _globalPhone;
        private readonly FormsDbContext _dbContext;

        public FormsController(GlobalPhoneContext globalPhone, FormsDbContext dbContext)
        {
            _globalPhone = globalPhone;
            _dbContext = dbContext;
        }
        // GET api/values

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ResponseInputModel model)
        {
            if (!Email.TryParse(model.Email, out Email parsedEmail))
            {
                return BadRequest(nameof(model.Email));
            }

            if (!_globalPhone.TryParse(model.Phone, out PhoneNumber number))
            {
                return BadRequest(nameof(model.Phone));
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(nameof(model.Name));
            }

            await _dbContext.Responses.AddAsync(new Response()
            {
                Email = model.Email,
                Name = model.Name,
                Phone = model.Phone
            });
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
