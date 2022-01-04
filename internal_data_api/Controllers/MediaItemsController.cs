using internal_data_api.Controllers.Base;
using internal_data_api.Model;
using Microsoft.AspNetCore.Mvc;

namespace internal_data_api.Controllers;

public class MediaItemsController : BaseController
{
    private readonly ILogger<MediaItemsController> _logger;

    public MediaItemsController(ILogger<MediaItemsController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    [ProducesResponseType(typeof(ApiResultModel<List<MediaItemModel>>), 200)]
    public IActionResult Get()
    {
        try
        {
            var excludeDeletedItems = false;
            int? playlistId = null;
            var skip = 0;
            var take = Int32.MaxValue;
            var user = "";
            var ytTitle = "";
            
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["exclude_deleted_items"]))
            {
                excludeDeletedItems = bool.Parse(HttpContext.Request.Query["exclude_deleted_items"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["playlist_id"]))
            {
                playlistId = Int32.Parse(HttpContext.Request.Query["playlist_id"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["skip"]))
            {
                skip = Int32.Parse(HttpContext.Request.Query["skip"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["take"]))
            {
                take = Int32.Parse(HttpContext.Request.Query["take"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["user"]))
            {
                user = HttpContext.Request.Query["user"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["title"]))
            {
                ytTitle = HttpContext.Request.Query["title"].ToString();
            }

            var filters = new MediaItemQueryFilters {
                ExcludeDeletedItems = excludeDeletedItems,
                PlaylistId = playlistId,
                Skip = skip,
                Take = take,
                User = user,
                YtTitle = ytTitle
            };

            var data = MediaItemQuery.GetMediaItems(filters);

            return Success<List<MediaItemModel>>(string.Empty, data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }

        
    }


    [HttpGet("{guid}")]
    [ProducesResponseType(typeof(ApiResultModel<MediaItemModel>), 200)]
    public IActionResult GetByGuid(string guid)
    {
        try
        {
            var data = MediaItemQuery.GetMediaItemByItemGuid(guid);

            if (data != null)
            {
                return Success<MediaItemModel>(string.Empty, data);
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
    public IActionResult Post(MediaItemModel model)
    {
        try
        {
            MediaItemQuery.CreateMediaItem(model);
            
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
    public IActionResult Put(MediaItemModel model)
    {
        try
        {
            MediaItemQuery.UpdateMediaItem(model);
            
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
            MediaItemQuery.DeleteMediaItem(id);
            
            return Success(string.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return Error("Something went wrong!");
        }
    }
    
}
