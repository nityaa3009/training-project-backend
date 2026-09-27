using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Timers;
using training_project_backend.Providers;
using training_project_backend.Requests;

namespace training_project_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtController : ControllerBase
    {
        private readonly IJwtProvider _provider;

        public JwtController(
            IJwtProvider provider)
        {
            _provider = provider;
        }


        //GET api/<JwtController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _provider.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var token = await _provider.GetByIdAsync(id);

            if (token == null) {
                return NotFound();
            }
            return Ok(token);
        }

        //POST api/<JwtController>/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadJwtRequest request)
        {
            var result = await _provider.UploadAsync(request);

            if (!result.Success) { 
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate(
    IFormFile tokenFile)
        {
            var result =
                await _provider.ValidateAsync(
                    tokenFile);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted =
                await _provider
                    .DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
