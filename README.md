# BaoCaoChiPhi API

.NET 9 Web API theo chuẩn **Clean Architecture** + **CQRS** (MediatR).

---

## Cấu trúc solution

```
BaoCaoChiPhi.sln
├── BaoCaoChiPhi.Domain          # Entity, Interface, Enum — không phụ thuộc gì
├── BaoCaoChiPhi.Application     # CQRS Command/Query, DTO, Validator
├── BaoCaoChiPhi.Infrastructure  # EF Core, Repository, JWT, Settings
└── BaoCaoChiPhi.API             # Controller, Program.cs, Swagger
```

**Luồng phụ thuộc (chỉ một chiều):**

```
API → Application → Domain
       ↑
Infrastructure (implement interface của Application/Domain)
```

---

## Luồng tạo một API mới

Ví dụ: tạo API **Lấy danh sách nhà máy**.

### Bước 1 — Domain: Định nghĩa Entity & Interface (nếu chưa có)

`BaoCaoChiPhi.Domain/Entities/NhaMay.cs`

```csharp
public class NhaMay : BaseEntity
{
    public string Ten { get; private set; } = string.Empty;
    public string Ma  { get; private set; } = string.Empty;
}
```

`BaoCaoChiPhi.Domain/Interfaces/INhaMayRepository.cs`

```csharp
public interface INhaMayRepository : IRepository<NhaMay>
{
    Task<IReadOnlyList<NhaMay>> GetActiveAsync(CancellationToken ct = default);
}
```

> Nếu query từ **DB ngoài** (như PRODUCTDATA1) thì đặt interface trong
> `BaoCaoChiPhi.Application/Interfaces/` thay vì Domain.

---

### Bước 2 — Application: Tạo Query + DTO + Handler

**2a. DTO** — `Application/DTOs/NhaMayDto.cs`

```csharp
public record NhaMayDto(Guid Id, string Ten, string Ma);
```

**2b. Query** — `Application/Features/NhaMays/Queries/GetNhaMayListQuery.cs`

```csharp
public record GetNhaMayListQuery(int PageNumber = 1, int PageSize = 20)
    : IRequest<PagedApiResponse<NhaMayDto>>;
```

**2c. Handler** — `Application/Features/NhaMays/Queries/GetNhaMayListQueryHandler.cs`

```csharp
public class GetNhaMayListQueryHandler(INhaMayRepository repo)
    : IRequestHandler<GetNhaMayListQuery, PagedApiResponse<NhaMayDto>>
{
    public async Task<PagedApiResponse<NhaMayDto>> Handle(
        GetNhaMayListQuery request, CancellationToken ct)
    {
        var (data, total) = await repo.GetPagedAsync(request.PageNumber, request.PageSize, ct);
        var dtos = data.Select(x => new NhaMayDto(x.Id, x.Ten, x.Ma)).ToList();
        return PagedApiResponse<NhaMayDto>.Success(dtos, total, request.PageNumber, request.PageSize);
    }
}
```

**2d. Validator (tuỳ chọn)** — `Application/Features/NhaMays/Queries/GetNhaMayListQueryValidator.cs`

```csharp
public class GetNhaMayListQueryValidator : AbstractValidator<GetNhaMayListQuery>
{
    public GetNhaMayListQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
```

---

### Bước 3 — Infrastructure: Implement Repository

**3a. Entity DB (nếu từ DB ngoài)**
`Infrastructure/Persistence/Entities/TblNhaMay.cs`

```csharp
[Table("Tbl_NhaMay", Schema = "dbo")]
public class TblNhaMay
{
    [Key] public long ID { get; set; }
    public string? TenNhaMay { get; set; }
    public string? MaNhaMay  { get; set; }
}
```

Thêm vào `ProductDataDbContext`:

```csharp
public DbSet<TblNhaMay> TblNhaMay => Set<TblNhaMay>();
```

**3b. Repository**
`Infrastructure/Repositories/NhaMayRepository.cs`

```csharp
public class NhaMayRepository(ProductDataDbContext context) : INhaMayRepository
{
    public async Task<(IReadOnlyList<NhaMayDto> Data, int Total)> GetPagedAsync(
        int pageNumber, int pageSize, CancellationToken ct)
    {
        var query = context.TblNhaMay.AsQueryable();

        var total = await query.CountAsync(ct);

        var data = await query
            .OrderBy(x => x.TenNhaMay)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new NhaMayDto(x.ID, x.TenNhaMay ?? "", x.MaNhaMay ?? ""))
            .ToListAsync(ct);

        return (data, total);
    }
}
```

**3c. Đăng ký DI** — `Infrastructure/DependencyInjection.cs`

```csharp
services.AddScoped<INhaMayRepository, NhaMayRepository>();
```

---

### Bước 4 — API: Tạo Controller

`API/Controllers/NhaMayController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NhaMayController(IMediator mediator) : ControllerBase
{
    /// <summary>Lấy danh sách nhà máy</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedApiResponse<NhaMayDto>), 200)]
    public async Task<IActionResult> GetList(
        [FromQuery] GetNhaMayListQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
```

---

## Checklist tạo API mới

```
[ ] 1. Domain   — Entity + Interface repository (nếu domain entity)
[ ] 2. App/DTOs — DTO response
[ ] 3. App/Features/<Tên>/Queries (hoặc Commands)
        [ ] XxxQuery.cs    (record implements IRequest<>)
        [ ] XxxHandler.cs  (class implements IRequestHandler<>)
        [ ] XxxValidator.cs (AbstractValidator — tuỳ chọn)
[ ] 4. Infra/Persistence/Entities — entity EF Core (nếu DB ngoài)
[ ] 5. Infra/Persistence/ProductDataDbContext — thêm DbSet<>
[ ] 6. Infra/Repositories — implement repository
[ ] 7. Infra/DependencyInjection.cs — đăng ký services.AddScoped<>
[ ] 8. API/Controllers — controller + action + [Authorize]
```

---

## Quy ước đặt tên

| Loại                 | Convention               | Ví dụ                        |
| -------------------- | ------------------------ | ---------------------------- |
| Query                | `Get{Tên}Query`          | `GetBienBanListQuery`        |
| Command              | `{Động từ}{Tên}Command`  | `TaoBaoCaoCommand`           |
| Handler              | `{Query/Command}Handler` | `GetBienBanListQueryHandler` |
| DTO                  | `{Tên}Dto`               | `BienBanGiaoNhanDto`         |
| Repository interface | `I{Tên}Repository`       | `IBienBanGiaoNhanRepository` |
| Controller           | `{Tên}Controller`        | `BienBanGiaoNhanController`  |

---

## Authentication

Tất cả endpoint cần `[Authorize]`. Lấy token qua:

```
POST /api/auth/login
{ "username": "admin", "password": "Admin@1234" }
```

Trả về `{ "token": "eyJ..." }` — dán vào Swagger UI nút **Authorize → Bearer {token}**.

---

## Cấu hình (`appsettings.json`)

| Key                                   | Mô tả                                    |
| ------------------------------------- | ---------------------------------------- |
| `ConnectionStrings:DefaultConnection` | DB chính (EF Core migrations)            |
| `ConnectionStrings:DbConnection`      | DB ngoài PRODUCTDATA1 (read-only)        |
| `JwtSettings:Secret`                  | Khóa ký JWT (≥ 32 ký tự)                 |
| `JwtSettings:ExpirationMinutes`       | Thời gian sống token (mặc định 480 phút) |
| `DefaultUser:Username / Password`     | Tài khoản đăng nhập mặc định             |

API
│
▼
Controller
│
▼
mediator.Send()
│
┌──────────┴──────────┐
▼ ▼
Query Handler Command Handler
│ │
▼ ▼
Repository Repository
│ │
└──────────┬──────────┘
▼
Database
