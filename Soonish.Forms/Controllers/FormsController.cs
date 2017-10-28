using Soonish.Forms.Data;
using Microsoft.AspNetCore.Mvc;
using Soonish.Forms.Emails;
using System.Threading.Tasks;
using Soonish.Forms.Models;
using GlobalPhoneContext = GlobalPhone.Context;
using PhoneNumber = GlobalPhone.Number;

namespace Soonish.Forms.Controllers
{
    [Route("api/[controller]")]
    public class FormsController : Controller
    {
        private readonly GlobalPhoneContext _globalPhone;
        private readonly FormsDbContext _dbContext;
        private readonly ISoonishTableStorage _soonishTable;

        public FormsController(GlobalPhoneContext globalPhone, FormsDbContext dbContext, ISoonishTableStorage soonishTable)
        {
            _globalPhone = globalPhone;
            _dbContext = dbContext;
            _soonishTable = soonishTable;
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
            var response = new Response()
            {
                Email = model.Email,
                Name = model.Name,
                Phone = model.Phone
            };
            await _soonishTable.Insert(response);
            await _dbContext.Responses.AddAsync(response);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
