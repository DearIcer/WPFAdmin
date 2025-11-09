<img width="1920" height="1039" alt="image" src="https://github.com/user-attachments/assets/b541bef6-f8b0-4a81-81e5-366e4ddc9620" />

# WPFAdmin 管理系统

一个基于 WPF 和 gRPC 的后台管理系统，包含用户权限管理、菜单管理等功能。

## 项目概述

WPFAdmin 是一个现代化的后台管理系统，采用前后端分离的架构设计。后端使用 ASP.NET Core 提供 gRPC 服务，前端使用 WPF 构建桌面应用程序。系统实现了基于角色的访问控制（RBAC）模型，支持用户、角色、权限和菜单管理。

## 技术栈

### 后端 (Backed)
- ASP.NET Core 8.0
- gRPC 服务
- Entity Framework Core (Code First)
- SQL Server 数据库
- 领域驱动设计 (DDD)

### 前端 (Client)
- WPF (.NET 8.0)
- MVVM 设计模式
- gRPC 客户端

## 功能特性

1. **用户管理**
    - 用户登录/登出
    - 用户信息管理
    - 用户状态管理

2. **角色管理**
    - 角色创建和编辑
    - 角色分配给用户
    - 角色权限管理

3. **菜单管理**
    - 多级菜单结构
    - 菜单权限控制
    - 菜单排序和状态管理

4. **权限控制**
    - 基于角色的访问控制（RBAC）
    - 动态菜单加载
    - 页面访问权限控制

## 项目结构

```
WPFAdmin/
├── Backed/                 # 后端项目
│   ├── Domain/             # 领域层
│   │   ├── Entities/       # 实体类
│   │   └── Interfaces/     # 接口定义
│   ├── Infrastructure/     # 基础设施层
│   │   ├── Data/           # 数据访问
│   │   └── Services/       # 服务实现
│   ├── Services/           # gRPC 服务
│   └── Migrations/         # 数据库迁移文件
└── Client/                 # 前端项目 (WPF)
    ├── ViewModels/         # 视图模型
    ├── Views/              # 视图
    ├── Models/             # 数据模型
    └── Services/           # 客户端服务
```

## 数据库配置

项目使用 SQL Server 数据库，连接字符串配置在 [appsettings.json](Backed/appsettings.json) 文件中：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=192.168.31.153;Database=WPFAdmin;User Id=sa;Password=123456abc-+;TrustServerCertificate=True;"
  }
}
```

请根据实际环境修改数据库连接信息。

## 快速开始

### 环境要求
- .NET 8.0 SDK
- SQL Server 数据库
- Visual Studio 或 JetBrains Rider

### 运行步骤

1. 克隆项目到本地
2. 修改数据库连接字符串
3. 运行数据库迁移：
   ```bash
   cd Backed
   dotnet ef database update
   ```
4. 启动后端服务：
   ```bash
   cd Backed
   dotnet run
   ```
5. 启动前端客户端：
   ```bash
   cd Client
   dotnet run
   ```

### 默认账户
系统包含默认管理员账户：
- 用户名：admin
- 密码：admin

## 主要实体

### 用户 (User)
- Username: 用户名
- Email: 邮箱
- PasswordHash: 密码哈希
- IsActive: 是否激活

### 角色 (Role)
- Name: 角色名
- Description: 描述

### 菜单 (Menu)
- Name: 菜单名称
- Code: 菜单代码（用于导航）
- Path: 路径
- Icon: 图标
- ParentId: 父级菜单ID
- SortOrder: 排序
- IsActive: 是否激活

## gRPC 服务

后端通过 gRPC 提供以下服务：

1. **GreeterService**: 示例服务
2. **RBACService**: 权限管理服务，包括用户认证、菜单获取、角色管理等

