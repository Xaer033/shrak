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
            _logger.LogError(e, $"Error processing request: {request}");
            
            var backendException = e as BackendException;
            var error = backendException?.Error ?? new Error();

            if (response != null)
                response.Error = error;
            
            return BadRequest(response);
        }
        finally
        {
            if (response != null)
                response.TimeStamp = DateTime.UtcNow;
        }
    }
}