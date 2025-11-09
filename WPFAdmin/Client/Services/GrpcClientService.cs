using Grpc.Net.Client;
using Backed.Grpc;

namespace Client.Services;

public class GrpcClientService
{
    private static readonly Lazy<GrpcClientService> _instance = new(() => new GrpcClientService());
    private readonly GrpcChannel _channel;
    private readonly RBACService.RBACServiceClient _rbacClient;

    private GrpcClientService()
    {
        _channel = GrpcChannel.ForAddress("http://localhost:5101");
        _rbacClient = new RBACService.RBACServiceClient(_channel);
    }

    public static GrpcClientService Instance => _instance.Value;

    public RBACService.RBACServiceClient RbacClient => _rbacClient;
}