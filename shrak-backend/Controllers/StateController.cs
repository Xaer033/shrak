using System.Security.Claims;
using Shrak.Errors;
using Shrak.Protocols;
using Shrak.DatabaseContexts;
using Microsoft.AspNetCore.Mvc;

namespace Shrak.Controllers;

public abstract class StateController : ControllerBase
{
    protected readonly IShipmentDbContext _dbContext;
    protected readonly ILogger<StateController> _logger;
    
    protected int GetUserId() => int.Parse(User.FindFirstValue("id") ?? string.Empty);
    protected string GetUserGuid() => User.FindFirstValue("guid") ?? string.Empty;
    
    public StateController(IShipmentDbContext dbContext, ILogger<StateController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    protected async Task<IActionResult> ProcessRequestAsync(
        Request request, 
        Response response, 
        Func<Task> requestProcess)
    {
        try
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await requestProcess();

            await transaction.CommitAsync();

            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error processing request: {request} | {e.Message}");

            if (response != null)
            {
                if (e is BackendException backendException)
                {
                    var error = backendException?.Error;
                    response.Error = error;
                }
                else
                {
                    response.Error = new Error
                    {
                        Message = e.Message
                    };
                }
            }
            
            return BadRequest(response);
        }
        finally
        {
            if (response != null)
                response.TimeStamp = DateTime.UtcNow;
        }
    }
}