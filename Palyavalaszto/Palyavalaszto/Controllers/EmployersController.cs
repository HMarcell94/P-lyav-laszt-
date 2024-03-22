using DB_Module.SQL_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        SQL _sql;

        public EmployersController(SQL sql)
        {
            this._sql = sql;
        }

        [HttpPut("Add")]
        public async Task<IActionResult> Add([FromBody] Employer employer)
        {
            try
            {
                if (!this._sql.Employers.Any(a => a.EmployerID == employer.EmployerID))
                {
                    var e = this._sql.Employers.Add(employer);
                    await this._sql.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("A megadott munkaadó ID már használatban van!");
                }
            }
            catch (OverflowException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Modify([FromBody] Employer employer)
        {
            try
            {
                if (this._sql.Employers.Any(a => a.EmployerID == employer.EmployerID))
                {
                    this._sql.Employers.Update(employer);
                    await this._sql.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("A megadott munkaadó ID nem létezik!");
                }
            }
            catch (OverflowException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (this._sql.Employers.Any(a => a.EmployerID == id))
                {
                    var e = this._sql.Employers.Single(a => a.EmployerID == id);
                    this._sql.Employers.Remove(e);
                    await this._sql.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("A megadott munkaadó ID nem létezik!");
                }
            }
            catch (OverflowException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Get")]
        public IActionResult Get(int id)
        {
            if (this._sql.Employers.Any(a => a.EmployerID == id))
            {
                return Ok(this._sql.Employers.Single(a => a.EmployerID == id));
            }
            return BadRequest("Nincs ilyen munkaadó az adatbázisban!");
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(this._sql.Employers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }
        }
    }
}

