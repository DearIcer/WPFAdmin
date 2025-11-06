using Backed.Domain.Entities;
using Backed.Domain.Interfaces;
using Grpc.Core;

namespace Backed.Services;

public class RBACService : Backed.Grpc.RBACService.RBACServiceBase
{
    private readonly ILogger<RBACService> _logger;
    private readonly IPermissionService _permissionService;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public RBACService(
        IUserService userService,
        IRoleService roleService,
        IPermissionService permissionService,
        ILogger<RBACService> logger)
    {
        _userService = userService;
        _roleService = roleService;
        _permissionService = permissionService;
        _logger = logger;
    }

    // User operations
    public override async Task<Backed.Grpc.AuthenticateUserResponse> AuthenticateUser(
        Backed.Grpc.AuthenticateUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                return new Backed.Grpc.AuthenticateUserResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };

            return new Backed.Grpc.AuthenticateUserResponse
            {
                Success = true,
                Message = "Authentication successful",
                User = MapToGrpcUser(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user {Username}", request.Username);
            return new Backed.Grpc.AuthenticateUserResponse
            {
                Success = false,
                Message = "An error occurred during authentication"
            };
        }
    }

    public override async Task<Backed.Grpc.GetUserResponse> GetUser(Backed.Grpc.GetUserRequest request,
        ServerCallContext context)
    {
        try
        {
            var user = await _userService.GetByIdAsync(request.Id);

            if (user == null)
                return new Backed.Grpc.GetUserResponse
                {
                    Success = false,
                    Message = "User not found"
                };

            return new Backed.Grpc.GetUserResponse
            {
                Success = true,
                Message = "User retrieved successfully",
                User = MapToGrpcUser(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", request.Id);
            return new Backed.Grpc.GetUserResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the user"
            };
        }
    }

    public override async Task<Backed.Grpc.GetAllUsersResponse> GetAllUsers(
        Backed.Grpc.GetAllUsersRequest request, ServerCallContext context)
    {
        try
        {
            var users = await _userService.GetAllAsync();

            return new Backed.Grpc.GetAllUsersResponse
            {
                Success = true,
                Message = "Users retrieved successfully",
                Users = { users.Select(MapToGrpcUser) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return new Backed.Grpc.GetAllUsersResponse
            {
                Success = false,
                Message = "An error occurred while retrieving users"
            };
        }
    }

    public override async Task<Backed.Grpc.CreateUserResponse> CreateUser(
        Backed.Grpc.CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            // Check if user already exists
            if (await _userService.UserExistsAsync(request.Username))
                return new Backed.Grpc.CreateUserResponse
                {
                    Success = false,
                    Message = "User with this username already exists"
                };

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = "" // 临时设置空字符串，实际会在UserService中处理
            };

            var createdUser = await _userService.CreateAsync(user, request.Password);

            return new Backed.Grpc.CreateUserResponse
            {
                Success = true,
                Message = "User created successfully",
                User = MapToGrpcUser(createdUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {Username}", request.Username);
            return new Backed.Grpc.CreateUserResponse
            {
                Success = false,
                Message = "An error occurred while creating the user"
            };
        }
    }

    public override async Task<Backed.Grpc.UpdateUserResponse> UpdateUser(
        Backed.Grpc.UpdateUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = MapToDomainUser(request.User);
            await _userService.UpdateAsync(user);

            return new Backed.Grpc.UpdateUserResponse
            {
                Success = true,
                Message = "User updated successfully",
                User = MapToGrpcUser(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", request.User.Id);
            return new Backed.Grpc.UpdateUserResponse
            {
                Success = false,
                Message = "An error occurred while updating the user"
            };
        }
    }

    public override async Task<Backed.Grpc.DeleteUserResponse> DeleteUser(
        Backed.Grpc.DeleteUserRequest request, ServerCallContext context)
    {
        try
        {
            await _userService.DeleteAsync(request.Id);

            return new Backed.Grpc.DeleteUserResponse
            {
                Success = true,
                Message = "User deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", request.Id);
            return new Backed.Grpc.DeleteUserResponse
            {
                Success = false,
                Message = "An error occurred while deleting the user"
            };
        }
    }

    // Role operations
    public override async Task<Backed.Grpc.GetRoleResponse> GetRole(Backed.Grpc.GetRoleRequest request,
        ServerCallContext context)
    {
        try
        {
            var role = await _roleService.GetByIdAsync(request.Id);

            if (role == null)
                return new Backed.Grpc.GetRoleResponse
                {
                    Success = false,
                    Message = "Role not found"
                };

            return new Backed.Grpc.GetRoleResponse
            {
                Success = true,
                Message = "Role retrieved successfully",
                Role = MapToGrpcRole(role)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role {RoleId}", request.Id);
            return new Backed.Grpc.GetRoleResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the role"
            };
        }
    }

    public override async Task<Backed.Grpc.GetAllRolesResponse> GetAllRoles(
        Backed.Grpc.GetAllRolesRequest request, ServerCallContext context)
    {
        try
        {
            var roles = await _roleService.GetAllAsync();

            return new Backed.Grpc.GetAllRolesResponse
            {
                Success = true,
                Message = "Roles retrieved successfully",
                Roles = { roles.Select(MapToGrpcRole) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all roles");
            return new Backed.Grpc.GetAllRolesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving roles"
            };
        }
    }

    public override async Task<Backed.Grpc.CreateRoleResponse> CreateRole(
        Backed.Grpc.CreateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdRole = await _roleService.CreateAsync(role);

            return new Backed.Grpc.CreateRoleResponse
            {
                Success = true,
                Message = "Role created successfully",
                Role = MapToGrpcRole(createdRole)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role {RoleName}", request.Name);
            return new Backed.Grpc.CreateRoleResponse
            {
                Success = false,
                Message = "An error occurred while creating the role"
            };
        }
    }

    public override async Task<Backed.Grpc.UpdateRoleResponse> UpdateRole(
        Backed.Grpc.UpdateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var role = MapToDomainRole(request.Role);
            await _roleService.UpdateAsync(role);

            return new Backed.Grpc.UpdateRoleResponse
            {
                Success = true,
                Message = "Role updated successfully",
                Role = MapToGrpcRole(role)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role {RoleId}", request.Role.Id);
            return new Backed.Grpc.UpdateRoleResponse
            {
                Success = false,
                Message = "An error occurred while updating the role"
            };
        }
    }

    public override async Task<Backed.Grpc.DeleteRoleResponse> DeleteRole(
        Backed.Grpc.DeleteRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.DeleteAsync(request.Id);

            return new Backed.Grpc.DeleteRoleResponse
            {
                Success = true,
                Message = "Role deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role {RoleId}", request.Id);
            return new Backed.Grpc.DeleteRoleResponse
            {
                Success = false,
                Message = "An error occurred while deleting the role"
            };
        }
    }

    public override async Task<Backed.Grpc.GetRolePermissionsResponse> GetRolePermissions(
        Backed.Grpc.GetRolePermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _roleService.GetRolePermissionsAsync(request.RoleId);

            return new Backed.Grpc.GetRolePermissionsResponse
            {
                Success = true,
                Message = "Role permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for role {RoleId}", request.RoleId);
            return new Backed.Grpc.GetRolePermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving role permissions"
            };
        }
    }

    public override async Task<Backed.Grpc.AssignPermissionToRoleResponse> AssignPermissionToRole(
        Backed.Grpc.AssignPermissionToRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.AssignPermissionAsync(request.RoleId, request.PermissionId);

            return new Backed.Grpc.AssignPermissionToRoleResponse
            {
                Success = true,
                Message = "Permission assigned to role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission {PermissionId} to role {RoleId}",
                request.PermissionId, request.RoleId);
            return new Backed.Grpc.AssignPermissionToRoleResponse
            {
                Success = false,
                Message = "An error occurred while assigning permission to role"
            };
        }
    }

    public override async Task<Backed.Grpc.RemovePermissionFromRoleResponse> RemovePermissionFromRole(
        Backed.Grpc.RemovePermissionFromRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.RemovePermissionAsync(request.RoleId, request.PermissionId);

            return new Backed.Grpc.RemovePermissionFromRoleResponse
            {
                Success = true,
                Message = "Permission removed from role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission {PermissionId} from role {RoleId}",
                request.PermissionId, request.RoleId);
            return new Backed.Grpc.RemovePermissionFromRoleResponse
            {
                Success = false,
                Message = "An error occurred while removing permission from role"
            };
        }
    }

    // Permission operations
    public override async Task<Backed.Grpc.GetPermissionResponse> GetPermission(
        Backed.Grpc.GetPermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = await _permissionService.GetByIdAsync(request.Id);

            if (permission == null)
                return new Backed.Grpc.GetPermissionResponse
                {
                    Success = false,
                    Message = "Permission not found"
                };

            return new Backed.Grpc.GetPermissionResponse
            {
                Success = true,
                Message = "Permission retrieved successfully",
                Permission = MapToGrpcPermission(permission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permission {PermissionId}", request.Id);
            return new Backed.Grpc.GetPermissionResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the permission"
            };
        }
    }

    public override async Task<Backed.Grpc.GetAllPermissionsResponse> GetAllPermissions(
        Backed.Grpc.GetAllPermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _permissionService.GetAllAsync();

            return new Backed.Grpc.GetAllPermissionsResponse
            {
                Success = true,
                Message = "Permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all permissions");
            return new Backed.Grpc.GetAllPermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving permissions"
            };
        }
    }

    public override async Task<Backed.Grpc.CreatePermissionResponse> CreatePermission(
        Backed.Grpc.CreatePermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = new Permission
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdPermission = await _permissionService.CreateAsync(permission);

            return new Backed.Grpc.CreatePermissionResponse
            {
                Success = true,
                Message = "Permission created successfully",
                Permission = MapToGrpcPermission(createdPermission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating permission {PermissionName}", request.Name);
            return new Backed.Grpc.CreatePermissionResponse
            {
                Success = false,
                Message = "An error occurred while creating the permission"
            };
        }
    }

    public override async Task<Backed.Grpc.UpdatePermissionResponse> UpdatePermission(
        Backed.Grpc.UpdatePermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = MapToDomainPermission(request.Permission);
            await _permissionService.UpdateAsync(permission);

            return new Backed.Grpc.UpdatePermissionResponse
            {
                Success = true,
                Message = "Permission updated successfully",
                Permission = MapToGrpcPermission(permission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating permission {PermissionId}", request.Permission.Id);
            return new Backed.Grpc.UpdatePermissionResponse
            {
                Success = false,
                Message = "An error occurred while updating the permission"
            };
        }
    }

    public override async Task<Backed.Grpc.DeletePermissionResponse> DeletePermission(
        Backed.Grpc.DeletePermissionRequest request, ServerCallContext context)
    {
        try
        {
            await _permissionService.DeleteAsync(request.Id);

            return new Backed.Grpc.DeletePermissionResponse
            {
                Success = true,
                Message = "Permission deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting permission {PermissionId}", request.Id);
            return new Backed.Grpc.DeletePermissionResponse
            {
                Success = false,
                Message = "An error occurred while deleting the permission"
            };
        }
    }

    // User-role operations
    public override async Task<Backed.Grpc.GetUserRolesResponse> GetUserRoles(
        Backed.Grpc.GetUserRolesRequest request, ServerCallContext context)
    {
        try
        {
            var roles = await _userService.GetUserRolesAsync(request.UserId);

            return new Backed.Grpc.GetUserRolesResponse
            {
                Success = true,
                Message = "User roles retrieved successfully",
                Roles = { roles.Select(MapToGrpcRole) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles for user {UserId}", request.UserId);
            return new Backed.Grpc.GetUserRolesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user roles"
            };
        }
    }

    public override async Task<Backed.Grpc.GetUserPermissionsResponse> GetUserPermissions(
        Backed.Grpc.GetUserPermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _userService.GetUserPermissionsAsync(request.UserId);

            return new Backed.Grpc.GetUserPermissionsResponse
            {
                Success = true,
                Message = "User permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for user {UserId}", request.UserId);
            return new Backed.Grpc.GetUserPermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user permissions"
            };
        }
    }

    // Mapping methods
    private Backed.Grpc.User MapToGrpcUser(User user)
    {
        return new Backed.Grpc.User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = ((DateTimeOffset)user.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)user.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private User MapToDomainUser(Backed.Grpc.User user)
    {
        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(user.CreatedAt).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(user.UpdatedAt).UtcDateTime,
            PasswordHash = "" // PasswordHash is not transferred for security reasons
        };
    }

    private Backed.Grpc.Role MapToGrpcRole(Role role)
    {
        return new Backed.Grpc.Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description ?? "",
            CreatedAt = ((DateTimeOffset)role.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)role.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private Role MapToDomainRole(Backed.Grpc.Role role)
    {
        return new Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(role.CreatedAt).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(role.UpdatedAt).UtcDateTime
        };
    }

    private Backed.Grpc.Permission MapToGrpcPermission(Permission permission)
    {
        return new Backed.Grpc.Permission
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description ?? "",
            CreatedAt = ((DateTimeOffset)permission.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)permission.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private Permission MapToDomainPermission(Backed.Grpc.Permission permission)
    {
        return new Permission
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(permission.CreatedAt).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(permission.UpdatedAt).UtcDateTime
        };
    }
}