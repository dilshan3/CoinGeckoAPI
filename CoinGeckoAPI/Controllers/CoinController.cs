using CoinGeckoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoinGeckoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinController : ControllerBase
    {
        private readonly CoinService _service;
        public CoinController(CoinService service)
        {
            _service = service;
        }

        //GenAI
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //GenAI
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var coin = await _service.GetCoinAsync(id);
                return coin != null ? Ok(coin) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}