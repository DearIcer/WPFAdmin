using System.Security.Claims;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Backed.Infrastructure.Services;

namespace Backed.Services;

public class JwtInterceptor : Interceptor
{
    private readonly JwtService _jwtService;

    public JwtInterceptor(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (context.Method.EndsWith("/Login") || context.Method.EndsWith("/AuthenticateUser"))
        {
            return await continuation(request, context);
        }

        // 从 metadata 中获取 token
        var authToken = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization")?.Value;

        if (string.IsNullOrEmpty(authToken))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "缺少访问令牌"));
        }
       
        var principal = _jwtService.ValidateToken(authToken);
        if (principal == null)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "无效的访问令牌"));
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = principal.FindFirst(ClaimTypes.Name)?.Value;
        context.UserState["userId"] = userId;
        context.UserState["username"] = username;

        return await continuation(request, context);
    }
}