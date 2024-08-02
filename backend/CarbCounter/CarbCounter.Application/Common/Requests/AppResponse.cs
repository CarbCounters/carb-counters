using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CarbCounter.Application.Common.Requests;

public class AppResponse
{
    public HttpStatusCode StatusCode { get; init; }
    public string? Message { get; init; }

    public AppResponse(HttpStatusCode statusCode, string? message = default)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public ActionResult ToActionResult()
    {
        return StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(Message),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(Message),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(Message),
            HttpStatusCode.NotFound => new NotFoundObjectResult(Message),
            _ => new ObjectResult(Message)
            {
                StatusCode = (int)StatusCode
            }
        };
    }
}

public class AppResponse<TContent>
{
    public HttpStatusCode StatusCode { get; init; }
    public string? Message { get; init; }
    public TContent? Content { get; init; }

    public AppResponse(HttpStatusCode statusCode, TContent? content = default, string? message = default)
    {
        StatusCode = statusCode;
        Message = message;
        Content = content;
    }

    public ActionResult ToActionResult()
    {
        return StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(Content is null ? Message : Content),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(Content is null ? Message : Content),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(Content is null ? Message : Content),
            HttpStatusCode.NotFound => new NotFoundObjectResult(Content is null ? Message : Content),
            _ => new ObjectResult(Content is null ? Message : Content)
            {
                StatusCode = (int)StatusCode
            }
        };
    }
}