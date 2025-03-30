using CORE.UserServices;
using DATA.DTO;
using DATA.Models;
using Microsoft.AspNetCore.Mvc;

namespace EF_API.Controllers.Usercontroller
{
    [Route("api/[controller]")]
    [ApiController]

    // <summary>
    /// Constructor to inject the user service.
    /// </summary>
    /// <param name="userService">Instance of UserService.</param>
    public class Usercontroller : ControllerBase
    {


        public IApplicationuserServices _userService { get; }

        public Usercontroller(IApplicationuserServices services)
        {
            _userService = services;
        }


        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userDto">User details.</param>
        /// <returns>HTTP response with status and message.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userDto)
        {
            var response = await _userService.CreateUser(userDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="userName">Username of the user.</param>
        /// <returns>HTTP response with user details.</returns>
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUserByUsername(string userName)
        {
            var response = await _userService.GetUserByUsername(userName);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Edits an existing user.
        /// </summary>
        /// <param name="userDto">Updated user details.</param>
        /// <returns>HTTP response with status.</returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditUser([FromBody] ApplicationUser userDto)
        {
            var response = await _userService.EditUser(userDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Updates specific fields of a user.
        /// </summary>
        /// <param name="userDto">Updated user details.</param>
        /// <returns>HTTP response with status.</returns>
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUser userDto)
        {
            var response = await _userService.UpdateUser(userDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>HTTP response with status.</returns>
        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var response = await _userService.DeleteUser(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
