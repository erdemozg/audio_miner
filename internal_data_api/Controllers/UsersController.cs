using internal_data_api.Controllers.Base;
using internal_data_api.Model;
using Microsoft.AspNetCore.Mvc;

namespace internal_data_api.Controllers;

public class UsersController : BaseController
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResultModel<UserModel>), 200)]
    public IActionResult GetById(int id)
    {
        try
        {
            var data = UserQuery.GetUser(id);

            if (data != null)
            {
                return Success<UserModel>(string.Empty, data);    
            }
            else
            {
                return NotFound("Resource not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpGet("getbyusername/{username}")]
    [ProducesResponseType(typeof(ApiResultModel<UserModel>), 200)]
    public IActionResult GetByUsername(string username)
    {
        try
        {
            var data = UserQuery.GetUserByUsername(username);

            if (data != null)
            {
                return Success<UserModel>(string.Empty, data);
            }
            else
            {
                return NotFound("Resource not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(ApiResultModel), 200)]
    public IActionResult Post(UserModel model)
    {
        try
        {
            UserQuery.CreateUser(model);

            return Created(string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }
}
