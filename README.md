# FU News Management System

## Thông tin dự án
- **Sinh viên**: Đỗ Hữu Hòa (HE186716)
- **Mã lớp**: SE1884-NET
- **Môn học**: PRN232 - Building Cross-Platform Back-End Application With .NET
- **Assignment**: 01

## Công nghệ sử dụng
- **.NET 8.0**
- **ASP.NET Core Web API** (Backend)
- **ASP.NET Core Razor Pages** (Frontend)
- **Entity Framework Core** (ORM)
- **SQL Server** (Database)
- **Bootstrap 5** (UI Framework)

## Cấu trúc Project

```
HE186716_DoHuuHoa_A01/
├── HE186716_DoHuuHoa_SE1884-NET_A01_BE/  # Backend API
│   ├── Controllers/                       # API Controllers
│   ├── DTOs/                              # Data Transfer Objects
│   ├── Models/                            # Entity Models
│   ├── Repositories/                      # Data Access Layer
│   └── Services/                          # Business Logic Layer
│
└── HE186716_DoHuuHoa_SE1884-NET_A01_FE/  # Frontend Web App
    ├── Pages/                             # Razor Pages
    │   ├── Admin/                         # Admin pages
    │   ├── Auth/                          # Login/Logout
    │   ├── News/                          # News views
    │   ├── Staff/                         # Staff pages
    │   └── Shared/                        # Layout, partials
    ├── Models/                            # Frontend DTOs
    └── Services/                          # API Service calls
```

## Hướng dẫn cài đặt

### 1. Yêu cầu hệ thống
- Visual Studio 2026
- .NET 8.0 SDK
- SQL Server 2025

### 2. Cấu hình Connection String
**Backend** (`appsettings.json`):
```json
{
  "ConnectionStrings": {
    "MyCnn": "Server=YOUR_SERVER;Database=FUNewsManagement;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Chạy ứng dụng

**Backend:**
```bash
cd HE186716_DoHuuHoa_SE1884-NET_A01_BE
dotnet run
# API: http://localhost:5184
```

**Frontend:**
```bash
cd HE186716_DoHuuHoa_SE1884-NET_A01_FE
dotnet run
# Web: http://localhost:5000
```

## API Endpoints

### Authentication
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| POST | `/api/auth/login` | Đăng nhập |

### Account (Admin)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/account` | Danh sách |
| POST | `/api/account` | Tạo mới |
| PUT | `/api/account/{id}` | Cập nhật |
| DELETE | `/api/account/{id}` | Xóa |
| PUT | `/api/account/{id}/change-password` | Đổi mật khẩu |

### Category (Staff)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/category` | Danh sách |
| POST | `/api/category` | Tạo mới |
| PUT | `/api/category/{id}` | Cập nhật |
| DELETE | `/api/category/{id}` | Xóa |

### News Article
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/news` | Tin active (public) |
| GET | `/api/news/all` | Tất cả |
| POST | `/api/news` | Tạo mới |
| PUT | `/api/news/{id}` | Cập nhật |
| DELETE | `/api/news/{id}` | Xóa |
| POST | `/api/news/{id}/duplicate` | Duplicate |

### Tag
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/tag` | Danh sách |
| POST | `/api/tag` | Tạo mới |
| PUT | `/api/tag/{id}` | Cập nhật |
| DELETE | `/api/tag/{id}` | Xóa |

### Report (Admin)
| Method | Endpoint | Mô tả |
|--------|----------|-------|
| GET | `/api/report/statistics` | Thống kê |
| GET | `/api/report/export` | Export CSV |

## Tài khoản Test

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@FUNewsManagementSystem.org` | `@@abc123@@` |
| Staff | *(xem database)* | *(xem database)* |

## Phân quyền

| Role | Quyền hạn |
|------|-----------|
| **Admin** | Quản lý Accounts, Báo cáo |
| **Staff** | Quản lý Categories, Articles, Tags |
| **Lecturer** | Xem tin tức |
| **Guest** | Xem tin tức active |

## Patterns
- 3-Layer Architecture
- Repository Pattern
- DTO Pattern
- Singleton Pattern (DbContext)
