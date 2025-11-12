using Backed.Domain.Interfaces;
using Backed.Grpc;
using Grpc.Core;
using Menu = Backed.Domain.Entities.Menu;
using Permission = Backed.Domain.Entities.Permission;
using Role = Backed.Domain.Entities.Role;
using User = Backed.Domain.Entities.User;
using Backed.Infrastructure.Services;

namespace Backed.Services;

public class RBACService : Grpc.RBACService.RBACServiceBase
{
    private readonly ILogger<RBACService> _logger;
    private readonly IPermissionService _permissionService;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;
    private readonly IMenuService _menuService;
    private readonly JwtService _jwtService;

    public RBACService(
        IUserService userService,
        IRoleService roleService,
        IPermissionService permissionService,
        IMenuService menuService,
        JwtService jwtService,
        ILogger<RBACService> logger)
    {
        _userService = userService;
        _roleService = roleService;
        _permissionService = permissionService;
        _menuService = menuService;
        _jwtService = jwtService;
        _logger = logger;
    }

    // User operations
    public override async Task<AuthenticateUserResponse> AuthenticateUser(
        AuthenticateUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);

            if (user == null)
                return new AuthenticateUserResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            
            var roles = await _userService.GetUserRolesAsync(user.Id);
            var roleNames = roles.Select(r => r.Name).ToList();
            
            var token = _jwtService.GenerateToken(user.Id, user.Username, roleNames);

            return new AuthenticateUserResponse
            {
                Success = true,
                Message = "Authentication successful",
                User = MapToGrpcUser(user),
                Token = token
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user {Username}", request.Username);
            return new AuthenticateUserResponse
            {
                Success = false,
                Message = "An error occurred during authentication"
            };
        }
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request,
        ServerCallContext context)
    {
        try
        {
            var user = await _userService.GetByIdAsync(request.Id);

            if (user == null)
                return new GetUserResponse
                {
                    Success = false,
                    Message = "User not found"
                };

            return new GetUserResponse
            {
                Success = true,
                Message = "User retrieved successfully",
                User = MapToGrpcUser(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", request.Id);
            return new GetUserResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the user"
            };
        }
    }

    public override async Task<GetAllUsersResponse> GetAllUsers(
        GetAllUsersRequest request, ServerCallContext context)
    {
        try
        {
            var users = await _userService.GetAllAsync();

            return new GetAllUsersResponse
            {
                Success = true,
                Message = "Users retrieved successfully",
                Users = { users.Select(MapToGrpcUser) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return new GetAllUsersResponse
            {
                Success = false,
                Message = "An error occurred while retrieving users"
            };
        }
    }

    public override async Task<CreateUserResponse> CreateUser(
        CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            // Check if user already exists
            if (await _userService.UserExistsAsync(request.Username))
                return new CreateUserResponse
                {
                    Success = false,
                    Message = "User with this username already exists"
                };

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                IsActive = true // 默认激活新用户
            };

            var createdUser = await _userService.CreateAsync(user, request.Password);

            return new CreateUserResponse
            {
                Success = true,
                Message = "User created successfully",
                User = MapToGrpcUser(createdUser)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {Username}", request.Username);
            return new CreateUserResponse
            {
                Success = false,
                Message = "An error occurred while creating the user"
            };
        }
    }

    public override async Task<UpdateUserResponse> UpdateUser(
        UpdateUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = MapToDomainUser(request.User);
            await _userService.UpdateAsync(user);

            return new UpdateUserResponse
            {
                Success = true,
                Message = "User updated successfully",
                User = MapToGrpcUser(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", request.User.Id);
            return new UpdateUserResponse
            {
                Success = false,
                Message = "An error occurred while updating the user"
            };
        }
    }

    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request, ServerCallContext context)
    {
        try
        {
            await _userService.DeleteAsync(request.Id);

            return new DeleteUserResponse
            {
                Success = true,
                Message = "User deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", request.Id);
            return new DeleteUserResponse
            {
                Success = false,
                Message = "An error occurred while deleting the user"
            };
        }
    }

    // Role operations
    public override async Task<GetRoleResponse> GetRole(GetRoleRequest request,
        ServerCallContext context)
    {
        try
        {
            var role = await _roleService.GetByIdAsync(request.Id);

            if (role == null)
                return new GetRoleResponse
                {
                    Success = false,
                    Message = "Role not found"
                };

            return new GetRoleResponse
            {
                Success = true,
                Message = "Role retrieved successfully",
                Role = MapToGrpcRole(role)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role {RoleId}", request.Id);
            return new GetRoleResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the role"
            };
        }
    }

    public override async Task<GetAllRolesResponse> GetAllRoles(
        GetAllRolesRequest request, ServerCallContext context)
    {
        try
        {
            var roles = await _roleService.GetAllAsync();

            return new GetAllRolesResponse
            {
                Success = true,
                Message = "Roles retrieved successfully",
                Roles = { roles.Select(MapToGrpcRole) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all roles");
            return new GetAllRolesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving roles"
            };
        }
    }

    public override async Task<CreateRoleResponse> CreateRole(
        CreateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdRole = await _roleService.CreateAsync(role);

            return new CreateRoleResponse
            {
                Success = true,
                Message = "Role created successfully",
                Role = MapToGrpcRole(createdRole)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role {RoleName}", request.Name);
            return new CreateRoleResponse
            {
                Success = false,
                Message = "An error occurred while creating the role"
            };
        }
    }

    public override async Task<UpdateRoleResponse> UpdateRole(
        UpdateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var role = MapToDomainRole(request.Role);
            await _roleService.UpdateAsync(role);

            return new UpdateRoleResponse
            {
                Success = true,
                Message = "Role updated successfully",
                Role = MapToGrpcRole(role)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role {RoleId}", request.Role.Id);
            return new UpdateRoleResponse
            {
                Success = false,
                Message = "An error occurred while updating the role"
            };
        }
    }

    public override async Task<DeleteRoleResponse> DeleteRole(
        DeleteRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.DeleteAsync(request.Id);

            return new DeleteRoleResponse
            {
                Success = true,
                Message = "Role deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role {RoleId}", request.Id);
            return new DeleteRoleResponse
            {
                Success = false,
                Message = "An error occurred while deleting the role"
            };
        }
    }

    public override async Task<GetRolePermissionsResponse> GetRolePermissions(
        GetRolePermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _roleService.GetRolePermissionsAsync(request.RoleId);

            return new GetRolePermissionsResponse
            {
                Success = true,
                Message = "Role permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for role {RoleId}", request.RoleId);
            return new GetRolePermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving role permissions"
            };
        }
    }

    public override async Task<AssignPermissionToRoleResponse> AssignPermissionToRole(
        AssignPermissionToRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.AssignPermissionAsync(request.RoleId, request.PermissionId);

            return new AssignPermissionToRoleResponse
            {
                Success = true,
                Message = "Permission assigned to role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission {PermissionId} to role {RoleId}",
                request.PermissionId, request.RoleId);
            return new AssignPermissionToRoleResponse
            {
                Success = false,
                Message = "An error occurred while assigning permission to role"
            };
        }
    }

    public override async Task<RemovePermissionFromRoleResponse> RemovePermissionFromRole(
        RemovePermissionFromRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.RemovePermissionAsync(request.RoleId, request.PermissionId);

            return new RemovePermissionFromRoleResponse
            {
                Success = true,
                Message = "Permission removed from role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission {PermissionId} from role {RoleId}",
                request.PermissionId, request.RoleId);
            return new RemovePermissionFromRoleResponse
            {
                Success = false,
                Message = "An error occurred while removing permission from role"
            };
        }
    }

    // Permission operations
    public override async Task<GetPermissionResponse> GetPermission(
        GetPermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = await _permissionService.GetByIdAsync(request.Id);

            if (permission == null)
                return new GetPermissionResponse
                {
                    Success = false,
                    Message = "Permission not found"
                };

            return new GetPermissionResponse
            {
                Success = true,
                Message = "Permission retrieved successfully",
                Permission = MapToGrpcPermission(permission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permission {PermissionId}", request.Id);
            return new GetPermissionResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the permission"
            };
        }
    }

    public override async Task<GetAllPermissionsResponse> GetAllPermissions(
        GetAllPermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _permissionService.GetAllAsync();

            return new GetAllPermissionsResponse
            {
                Success = true,
                Message = "Permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all permissions");
            return new GetAllPermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving permissions"
            };
        }
    }

    public override async Task<CreatePermissionResponse> CreatePermission(
        CreatePermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = new Permission
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdPermission = await _permissionService.CreateAsync(permission);

            return new CreatePermissionResponse
            {
                Success = true,
                Message = "Permission created successfully",
                Permission = MapToGrpcPermission(createdPermission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating permission {PermissionName}", request.Name);
            return new CreatePermissionResponse
            {
                Success = false,
                Message = "An error occurred while creating the permission"
            };
        }
    }

    public override async Task<UpdatePermissionResponse> UpdatePermission(
        UpdatePermissionRequest request, ServerCallContext context)
    {
        try
        {
            var permission = MapToDomainPermission(request.Permission);
            await _permissionService.UpdateAsync(permission);

            return new UpdatePermissionResponse
            {
                Success = true,
                Message = "Permission updated successfully",
                Permission = MapToGrpcPermission(permission)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating permission {PermissionId}", request.Permission.Id);
            return new UpdatePermissionResponse
            {
                Success = false,
                Message = "An error occurred while updating the permission"
            };
        }
    }

    public override async Task<DeletePermissionResponse> DeletePermission(
        DeletePermissionRequest request, ServerCallContext context)
    {
        try
        {
            await _permissionService.DeleteAsync(request.Id);

            return new DeletePermissionResponse
            {
                Success = true,
                Message = "Permission deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting permission {PermissionId}", request.Id);
            return new DeletePermissionResponse
            {
                Success = false,
                Message = "An error occurred while deleting the permission"
            };
        }
    }

    // User-role operations
    public override async Task<GetUserRolesResponse> GetUserRoles(
        GetUserRolesRequest request, ServerCallContext context)
    {
        try
        {
            var roles = await _userService.GetUserRolesAsync(request.UserId);

            return new GetUserRolesResponse
            {
                Success = true,
                Message = "User roles retrieved successfully",
                Roles = { roles.Select(MapToGrpcRole) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles for user {UserId}", request.UserId);
            return new GetUserRolesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user roles"
            };
        }
    }

    public override async Task<GetUserPermissionsResponse> GetUserPermissions(
        GetUserPermissionsRequest request, ServerCallContext context)
    {
        try
        {
            var permissions = await _userService.GetUserPermissionsAsync(request.UserId);

            return new GetUserPermissionsResponse
            {
                Success = true,
                Message = "User permissions retrieved successfully",
                Permissions = { permissions.Select(MapToGrpcPermission) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving permissions for user {UserId}", request.UserId);
            return new GetUserPermissionsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user permissions"
            };
        }
    }

    public override async Task<AssignRoleToUserResponse> AssignRoleToUser(
        AssignRoleToUserRequest request, ServerCallContext context)
    {
        try
        {
            await _userService.AssignRoleAsync(request.UserId, request.RoleId);

            return new AssignRoleToUserResponse
            {
                Success = true,
                Message = "Role assigned to user successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning role {RoleId} to user {UserId}",
                request.RoleId, request.UserId);
            return new AssignRoleToUserResponse
            {
                Success = false,
                Message = "An error occurred while assigning role to user"
            };
        }
    }

    public override async Task<RemoveRoleFromUserResponse> RemoveRoleFromUser(
        RemoveRoleFromUserRequest request, ServerCallContext context)
    {
        try
        {
            await _userService.RemoveRoleAsync(request.UserId, request.RoleId);

            return new RemoveRoleFromUserResponse
            {
                Success = true,
                Message = "Role removed from user successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing role {RoleId} from user {UserId}",
                request.RoleId, request.UserId);
            return new RemoveRoleFromUserResponse
            {
                Success = false,
                Message = "An error occurred while removing role from user"
            };
        }
    }

    // Role-menu operations
    public override async Task<GetRoleMenusResponse> GetRoleMenus(
        GetRoleMenusRequest request, ServerCallContext context)
    {
        try
        {
            var menus = await _roleService.GetRoleMenusAsync(request.RoleId);

            return new GetRoleMenusResponse
            {
                Success = true,
                Message = "Role menus retrieved successfully",
                Menus = { menus.Select(MapToGrpcMenu) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving menus for role {RoleId}", request.RoleId);
            return new GetRoleMenusResponse
            {
                Success = false,
                Message = "An error occurred while retrieving role menus"
            };
        }
    }

    public override async Task<AssignMenuToRoleResponse> AssignMenuToRole(
        AssignMenuToRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.AssignMenuAsync(request.RoleId, request.MenuId);

            return new AssignMenuToRoleResponse
            {
                Success = true,
                Message = "Menu assigned to role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning menu {MenuId} to role {RoleId}",
                request.MenuId, request.RoleId);
            return new AssignMenuToRoleResponse
            {
                Success = false,
                Message = "An error occurred while assigning menu to role"
            };
        }
    }

    public override async Task<RemoveMenuFromRoleResponse> RemoveMenuFromRole(
        RemoveMenuFromRoleRequest request, ServerCallContext context)
    {
        try
        {
            await _roleService.RemoveMenuAsync(request.RoleId, request.MenuId);

            return new RemoveMenuFromRoleResponse
            {
                Success = true,
                Message = "Menu removed from role successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing menu {MenuId} from role {RoleId}",
                request.MenuId, request.RoleId);
            return new RemoveMenuFromRoleResponse
            {
                Success = false,
                Message = "An error occurred while removing menu from role"
            };
        }
    }

    // Menu mapping operations
    public override async Task<GetMenuCodeToIdMapResponse> GetMenuCodeToIdMap(
        GetMenuCodeToIdMapRequest request, ServerCallContext context)
    {
        try
        {
            var menuMap = await _menuService.GetMenuCodeToIdMapAsync();

            return new GetMenuCodeToIdMapResponse
            {
                Success = true,
                Message = "Menu code to ID map retrieved successfully",
                MenuMap = { menuMap }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving menu code to ID map");
            return new GetMenuCodeToIdMapResponse
            {
                Success = false,
                Message = "An error occurred while retrieving menu code to ID map"
            };
        }
    }

    // Menu operations
    public override async Task<GetAllMenusResponse> GetAllMenus(
        GetAllMenusRequest request, ServerCallContext context)
    {
        try
        {
            var menus = await _menuService.GetAllAsync();

            return new GetAllMenusResponse
            {
                Success = true,
                Message = "Menus retrieved successfully",
                Menus = { menus.Select(MapToGrpcMenu) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all menus");
            return new GetAllMenusResponse
            {
                Success = false,
                Message = "An error occurred while retrieving menus"
            };
        }
    }

    public override async Task<GetUserMenuTreeResponse> GetUserMenuTree(
        GetUserMenuTreeRequest request, ServerCallContext context)
    {
        try
        {
            var menus = await _userService.GetUserMenusAsync(request.UserId);

            return new GetUserMenuTreeResponse
            {
                Success = true,
                Message = "User menu tree retrieved successfully",
                Menus = { menus.Select(MapToGrpcMenu) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving menu tree for user {UserId}", request.UserId);
            return new GetUserMenuTreeResponse
            {
                Success = false,
                Message = "An error occurred while retrieving user menu tree"
            };
        }
    }

    public override async Task<GetMenuResponse> GetMenu(
        GetMenuRequest request, ServerCallContext context)
    {
        try
        {
            var menu = await _menuService.GetByIdAsync(request.Id);

            if (menu == null)
                return new GetMenuResponse
                {
                    Success = false,
                    Message = "Menu not found"
                };

            return new GetMenuResponse
            {
                Success = true,
                Message = "Menu retrieved successfully",
                Menu = MapToGrpcMenu(menu)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving menu {MenuId}", request.Id);
            return new GetMenuResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the menu"
            };
        }
    }

    public override async Task<CreateMenuResponse> CreateMenu(
        CreateMenuRequest request, ServerCallContext context)
    {
        try
        {
            var menu = new Menu
            {
                Name = request.Name,
                Code = request.Code,
                Path = request.Path,
                Icon = request.Icon,
                ParentId = request.ParentId,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            var createdMenu = await _menuService.CreateAsync(menu);

            return new CreateMenuResponse
            {
                Success = true,
                Message = "Menu created successfully",
                Menu = MapToGrpcMenu(createdMenu)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating menu {MenuName}", request.Name);
            return new CreateMenuResponse
            {
                Success = false,
                Message = "An error occurred while creating the menu"
            };
        }
    }

    public override async Task<UpdateMenuResponse> UpdateMenu(
        UpdateMenuRequest request, ServerCallContext context)
    {
        try
        {
            var menu = MapToDomainMenu(request.Menu);
            await _menuService.UpdateAsync(menu);

            return new UpdateMenuResponse
            {
                Success = true,
                Message = "Menu updated successfully",
                Menu = MapToGrpcMenu(menu)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating menu {MenuId}", request.Menu.Id);
            return new UpdateMenuResponse
            {
                Success = false,
                Message = "An error occurred while updating the menu"
            };
        }
    }

    public override async Task<DeleteMenuResponse> DeleteMenu(
        DeleteMenuRequest request, ServerCallContext context)
    {
        try
        {
            await _menuService.DeleteAsync(request.Id);

            return new DeleteMenuResponse
            {
                Success = true,
                Message = "Menu deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting menu {MenuId}", request.Id);
            return new DeleteMenuResponse
            {
                Success = false,
                Message = "An error occurred while deleting the menu"
            };
        }
    }

    public override async Task<GetMenuTreeResponse> GetMenuTree(
        GetMenuTreeRequest request, ServerCallContext context)
    {
        try
        {
            var menus = await _menuService.GetMenuTreeAsync();

            return new GetMenuTreeResponse
            {
                Success = true,
                Message = "Menu tree retrieved successfully",
                Menus = { menus.Select(MapToGrpcMenu) }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving menu tree");
            return new GetMenuTreeResponse
            {
                Success = false,
                Message = "An error occurred while retrieving menu tree"
            };
        }
    }

    // Mapping methods
    private Grpc.User MapToGrpcUser(User user)
    {
        return new Grpc.User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = ((DateTimeOffset)user.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)user.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private User MapToDomainUser(Grpc.User user)
    {
        // 首先从数据库获取现有用户以保留密码
        var existingUser = _userService.GetByIdAsync(user.Id).Result;
        
        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(user.CreatedAt).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(user.UpdatedAt).UtcDateTime,
            PasswordHash = existingUser?.PasswordHash ?? "" // 保留现有密码哈希
        };
    }

    private Grpc.Role MapToGrpcRole(Role role)
    {
        return new Grpc.Role
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description ?? "",
            CreatedAt = ((DateTimeOffset)role.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)role.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private Role MapToDomainRole(Grpc.Role role)
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

    private Grpc.Permission MapToGrpcPermission(Permission permission)
    {
        return new Grpc.Permission
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description ?? "",
            CreatedAt = ((DateTimeOffset)permission.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)permission.UpdatedAt).ToUnixTimeSeconds()
        };
    }

    private Permission MapToDomainPermission(Grpc.Permission permission)
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

    private Grpc.Menu MapToGrpcMenu(Menu menu)
    {
        var grpcMenu = new Grpc.Menu
        {
            Id = menu.Id,
            Name = menu.Name,
            Code = menu.Code,
            Path = menu.Path ?? "",
            Icon = menu.Icon ?? "",
            ParentId = menu.ParentId ?? 0,
            SortOrder = menu.SortOrder,
            IsActive = menu.IsActive,
            CreatedAt = ((DateTimeOffset)menu.CreatedAt).ToUnixTimeSeconds(),
            UpdatedAt = ((DateTimeOffset)menu.UpdatedAt).ToUnixTimeSeconds()
        };

        // Map children recursively
        foreach (var child in menu.Children)
        {
            grpcMenu.Children.Add(MapToGrpcMenu(child));
        }

        return grpcMenu;
    }

    private Menu MapToDomainMenu(Backed.Grpc.Menu menu)
    {
        var domainMenu = new Menu
        {
            Id = menu.Id,
            Name = menu.Name,
            Code = menu.Code,
            Path = string.IsNullOrEmpty(menu.Path) ? null : menu.Path,
            Icon = string.IsNullOrEmpty(menu.Icon) ? null : menu.Icon,
            ParentId = menu.ParentId == 0 ? (int?)null : menu.ParentId, // 修复：当ParentId为0时才设为null
            SortOrder = menu.SortOrder,
            IsActive = menu.IsActive,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(menu.CreatedAt).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(menu.UpdatedAt).UtcDateTime
        };

        // Map children recursively
        foreach (var child in menu.Children)
        {
            domainMenu.Children.Add(MapToDomainMenu(child));
        }

        return domainMenu;
    }
}