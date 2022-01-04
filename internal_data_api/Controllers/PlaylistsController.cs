using internal_data_api.Controllers.Base;
using internal_data_api.Model;
using Microsoft.AspNetCore.Mvc;

namespace internal_data_api.Controllers;

public class PlaylistsController : BaseController
{
    private readonly ILogger<PlaylistsController> _logger;

    public PlaylistsController(ILogger<PlaylistsController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    [ProducesResponseType(typeof(ApiResultModel<List<PlaylistModel>>), 200)]
    public IActionResult Get()
    {
        try
        {
            var data = PlaylistQuery.GetPlaylists();
            
            return Success<List<PlaylistModel>>(string.Empty, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResultModel<PlaylistModel>), 200)]
    public IActionResult GetById(int id)
    {
        try
        {
            var data = PlaylistQuery.GetPlaylist(id);

            if (data != null)
            {
                return Success<PlaylistModel>(string.Empty, data);
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


    [HttpGet("GetByUserId/{userId}")]
    [ProducesResponseType(typeof(ApiResultModel<List<PlaylistModel>>), 200)]
    public IActionResult GetByUserId(int userId)
    {
        try
        {
            var data = PlaylistQuery.GetPlaylistsByUserId(userId);
            
            return Success<List<PlaylistModel>>(string.Empty, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpGet("GetByYtPlaylistId/{ytPlaylistId}")]
    [ProducesResponseType(typeof(ApiResultModel<PlaylistModel>), 200)]
    public IActionResult GetByYtPlaylistId(string ytPlaylistId)
    {
        try
        {
            var data = PlaylistQuery.GetByYtPlaylistId(ytPlaylistId);
            
            return Success<PlaylistModel>(string.Empty, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(ApiResultModel), 200)]
    public IActionResult Post(PlaylistModel model)
    {
        try
        {
            PlaylistQuery.AddPlaylist(model);
            
            return Created(string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpPut]
    [ProducesResponseType(typeof(ApiResultModel), 200)]
    public IActionResult Put(PlaylistModel model)
    {
        try
        {
            PlaylistQuery.UpdatePlaylist(model);
            
            return Success(string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResultModel), 200)]
    public IActionResult Delete(int id)
    {
        try
        {
            PlaylistQuery.DeletePlaylist(id);
            
            return Success(string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }

}
