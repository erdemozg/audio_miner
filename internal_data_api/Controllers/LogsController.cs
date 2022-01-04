using internal_data_api.Controllers.Base;
using internal_data_api.Model;
using Microsoft.AspNetCore.Mvc;

namespace internal_data_api.Controllers;

public class LogsController : BaseController
{
    private readonly ILogger<LogsController> _logger;

    public LogsController(ILogger<LogsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResultModel), 201)]
    public IActionResult Post(LogModel model)
    {
        try
        {
            LogQuery.CreateLog(model);

            return Created(string.Empty);
        }
        catch (Exception)
        {
            return Error("Something went wrong!");
        }
    }
}
