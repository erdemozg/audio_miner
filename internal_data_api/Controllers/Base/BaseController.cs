using internal_data_api.Model;
using Microsoft.AspNetCore.Mvc;

namespace internal_data_api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    [NonAction]
    protected IActionResult Success<T>(string message, T data)
    {
        return Ok(new ApiResultModel<T>
        {
            Success = true,
            Message = message,
            Data = data
        });
    }

    [NonAction]
    protected IActionResult Success(string message)
    {
        return Ok(new ApiResultModel
        {
            Success = true,
            Message = message
        });
    }

    [NonAction]
    protected IActionResult Created<T>(string message, T data)
    {
        return StatusCode(201, new ApiResultModel<T>
        {
            Success = true,
            Message = message,
            Data = data
        });
    }

    [NonAction]
    protected IActionResult Created(string message)
    {
        return StatusCode(201, new ApiResultModel
        {
            Success = true,
            Message = message
        });
    }

    [NonAction]
    protected IActionResult NotFound<T>(string message, T data)
    {
        return StatusCode(404, new ApiResultModel<T>
        {
            Success = false,
            Message = message,
            Data = data
        });
    }

    [NonAction]
    protected IActionResult NotFound(string message)
    {
        return StatusCode(404, new ApiResultModel
        {
            Success = false,
            Message = message
        });
    }

    [NonAction]
    protected IActionResult BadRequest<T>(string message, string internalMessage, T data)
    {
        return StatusCode(400, new ApiResultModel<T>
        {
            Success = false,
            Message = message,
            Data = data
        });
    }

    [NonAction]
    protected IActionResult BadRequest(string message, string internalMessage)
    {
        return StatusCode(400, new ApiResultModel
        {
            Success = false,
            Message = message
        });
    }

    [NonAction]
    protected IActionResult Error<T>(string message, T data)
    {
        return StatusCode(500, new ApiResultModel<T>
        {
            Success = false,
            Message = message,
            Data = data
        });
    }

    [NonAction]
    protected IActionResult Error(string message)
    {
        return StatusCode(500, new ApiResultModel
        {
            Success = false,
            Message = message
        });
    }
}
