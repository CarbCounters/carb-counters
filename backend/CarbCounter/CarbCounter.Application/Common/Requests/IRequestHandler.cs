namespace CarbCounter.Application.Common.Requests;

public interface IRequestHandler<in TRequest>
{
    public Task<AppResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest, TResponse>
{
    public Task<AppResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}