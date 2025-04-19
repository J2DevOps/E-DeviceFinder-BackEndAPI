using CORE.ItemServices;
using Microsoft.AspNetCore.Mvc;

namespace EF_API.Controllers.ItemController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchReports([FromQuery] string query)
        {
            // Call a method in the service to perform the search, passing the query parameter
            var response = await _itemService.SearchItemsAsync(query);

            return StatusCode(response.StatusCode, response);
        }
    }
}
