using Grpc.Net.Client;
using Backed.Grpc;
using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Client.Services;

public class GrpcClientService
{
    private static readonly Lazy<GrpcClientService> _instance = new(() => new GrpcClientService());
    private readonly GrpcChannel _channel;
    private readonly RBACService.RBACServiceClient _rbacClient;
    private string _authToken;

    private GrpcClientService()
    {
        _channel = GrpcChannel.ForAddress("http://localhost:5101");
        _rbacClient = new RBACService.RBACServiceClient(_channel.Intercept(new AuthInterceptor(GetAuthToken)));
    }

    public static GrpcClientService Instance => _instance.Value;

    public RBACService.RBACServiceClient RbacClient => _rbacClient;

    public void SetAuthToken(string token)
    {
        _authToken = token;
    }

    private string GetAuthToken()
    {
        return _authToken;
    }
}

public class AuthInterceptor : Interceptor
{
    private readonly Func<string> _getToken;

    public AuthInterceptor(Func<string> getToken)
    {
        _getToken = getToken;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var token = _getToken();
        if (!string.IsNullOrEmpty(token))
        {
            var headers = context.Options.Headers ?? new Metadata();
            headers.Add("authorization", token);
            
            var options = context.Options.WithHeaders(headers);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
        }

        return base.AsyncUnaryCall(request, context, continuation);
    }
}