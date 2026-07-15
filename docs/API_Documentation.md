# BaoCaoChiPhi API — Tài liệu mô tả

**Base URL:** `https://<host>/api`  
**Phiên bản:** v1  
**Xác thực:** JWT Bearer Token (trừ endpoint đăng nhập)

---

## Mục lục

1. [Auth — Xác thực](#1-auth--xác-thực)
   - [POST /Auth/login](#11-post-authlogin)
2. [BienBanGiaoNhan — Biên bản giao nhận vật liệu](#2-bienbangiaoNhan--biên-bản-giao-nhận-vật-liệu)
   - [GET /BienBanGiaoNhan](#21-get-bienbangiaoNhan)

---

## 1. Auth — Xác thực

### 1.1 POST /Auth/login

Đăng nhập bằng tài khoản nội bộ để nhận JWT token. Token này được dùng để gọi tất cả các API yêu cầu xác thực.

**Không yêu cầu xác thực.**

#### Request

| Thuộc tính | Vị trí | Kiểu dữ liệu | Bắt buộc | Mô tả |
|---|---|---|---|---|
| `Content-Type` | Header | `string` | Có | `application/json` |

**Body (JSON):**

```json
{
  "username": "string",
  "password": "string"
}
```

| Trường | Kiểu | Bắt buộc | Mô tả |
|---|---|---|---|
| `username` | string | Có | Tên đăng nhập |
| `password` | string | Có | Mật khẩu |

#### Response

**200 OK — Đăng nhập thành công**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

| Trường | Kiểu | Mô tả |
|---|---|---|
| `token` | string | JWT Bearer Token, có thời hạn theo cấu hình `JwtSettings.ExpirationMinutes` (mặc định 60 phút) |

**401 Unauthorized — Sai thông tin đăng nhập**

```json
{
  "message": "Sai tên đăng nhập hoặc mật khẩu"
}
```

#### Cách dùng token sau khi đăng nhập

Đính kèm token vào header `Authorization` cho tất cả các request tiếp theo:

```
Authorization: Bearer <token>
```

#### Ví dụ cURL

```bash
curl -X POST https://<host>/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"username": "admin", "password": "123456"}'
```

---

## 2. BienBanGiaoNhan — Biên bản giao nhận vật liệu

### 2.1 GET /BienBanGiaoNhan

Lấy danh sách biên bản giao nhận vật liệu giữa các phân xưởng/phòng ban. Dữ liệu được **nhóm theo** ngày sản xuất, ca, vật tư, phân xưởng bên giao và bên nhận; **phân trang** theo `PageNumber` / `PageSize`.

**Yêu cầu xác thực:** Bearer Token (JWT)

#### Request

| Thuộc tính | Vị trí | Kiểu dữ liệu | Bắt buộc | Mô tả |
|---|---|---|---|---|
| `Authorization` | Header | `string` | Có | `Bearer <token>` |

**Query Parameters:**

| Tham số | Kiểu | Bắt buộc | Mặc định | Mô tả |
|---|---|---|---|---|
| `FromDate` | datetime | Không | — | Lọc từ ngày (tính theo ngày, bỏ qua giờ). Định dạng: `yyyy-MM-dd` hoặc ISO 8601 |
| `ToDate` | datetime | Không | — | Lọc đến ngày (tính theo ngày, bỏ qua giờ). Định dạng: `yyyy-MM-dd` hoặc ISO 8601 |
| `Shift` | integer | Không | — | Lọc theo ca: `1` = Ca ngày, `2` = Ca đêm |
| `MaterialCode` | string | Không | — | Mã vật tư SAP (khớp chính xác) |
| `WorkshopFrom` | string | Không | — | Tên xưởng bên **giao** (khớp chính xác) |
| `WorkshopTo` | string | Không | — | Tên xưởng bên **nhận** (khớp chính xác) |
| `PlantFrom` | string | Không | — | Tên phòng ban bên **giao** (khớp chính xác) |
| `PlantTo` | string | Không | — | Tên phòng ban bên **nhận** (khớp chính xác) |
| `PageNumber` | integer | Không | `1` | Số trang (bắt đầu từ 1) |
| `PageSize` | integer | Không | `20` | Số bản ghi mỗi trang |

#### Response

**200 OK**

```json
{
  "code": 200,
  "message": "Success",
  "data": [
    {
      "productionDate": "2026-06-30T00:00:00",
      "shift": 1,
      "shiftName": "Ngày",
      "materialName": "Thép cuộn CT3",
      "materialCode": "MAT-001",
      "workshopFrom": "Xưởng Cán",
      "workshopTo": "Xưởng Lắp Ráp",
      "plantFrom": "Phân xưởng A",
      "plantTo": "Phân xưởng B",
      "weight": 1250.50,
      "unit": "KG"
    }
  ],
  "totalRecords": 150,
  "pageNumber": 1,
  "pageSize": 20
}
```

**Cấu trúc Response:**

| Trường | Kiểu | Mô tả |
|---|---|---|
| `code` | integer | Mã trạng thái HTTP (`200` khi thành công) |
| `message` | string | Thông báo kết quả (`"Success"` khi thành công) |
| `data` | array | Danh sách biên bản giao nhận (xem bên dưới) |
| `totalRecords` | integer | Tổng số nhóm bản ghi thỏa điều kiện lọc (dùng để tính tổng trang) |
| `pageNumber` | integer | Trang hiện tại |
| `pageSize` | integer | Số bản ghi mỗi trang |

**Cấu trúc mỗi phần tử trong `data`:**

| Trường | Kiểu | Mô tả |
|---|---|---|
| `productionDate` | datetime | Ngày sản xuất (phần ngày của `NgayTao`) |
| `shift` | integer | Ca sản xuất (`1` hoặc `2`) |
| `shiftName` | string | Tên ca: `"Ngày"` (ca 1) hoặc `"Đêm"` (ca 2) |
| `materialName` | string | Tên vật tư (ưu tiên tên SAP, dự phòng tên nội bộ) |
| `materialCode` | string | Mã vật tư SAP |
| `workshopFrom` | string | Tên xưởng bên giao |
| `workshopTo` | string | Tên xưởng bên nhận |
| `plantFrom` | string | Tên phòng ban bên giao |
| `plantTo` | string | Tên phòng ban bên nhận |
| `weight` | decimal | Tổng khối lượng quy kho bên nhận (`KL_QuyKho_BN`) |
| `unit` | string | Đơn vị tính |

**401 Unauthorized** — Token không hợp lệ hoặc hết hạn.

#### Logic nhóm dữ liệu

Mỗi bản ghi trong `data` là kết quả **GROUP BY** theo:
- Ngày sản xuất (`NgayTao.Date`)
- Ca (`Ca`)
- Vật tư (`ID_VatTu`)
- Phòng ban bên giao (`ID_PhongBan_BG`) và xưởng bên giao (`ID_Xuong_BG`)
- Phòng ban bên nhận (`ID_PhongBan_BN`) và xưởng bên nhận (`ID_Xuong_BN`)

`weight` là **tổng** `KL_QuyKho_BN` của tất cả dòng trong nhóm.  
Kết quả sắp xếp: **ngày giảm dần**, sau đó **ca tăng dần**.

#### Ví dụ cURL

```bash
# Lấy trang đầu, lọc theo ngày và ca ngày
curl -X GET "https://<host>/api/BienBanGiaoNhan?FromDate=2026-06-01&ToDate=2026-06-30&Shift=1&PageNumber=1&PageSize=20" \
  -H "Authorization: Bearer <token>"
```

```bash
# Lọc theo mã vật tư và xưởng
curl -X GET "https://<host>/api/BienBanGiaoNhan?MaterialCode=MAT-001&WorkshopFrom=Xưởng+Cán&PageSize=50" \
  -H "Authorization: Bearer <token>"
```

---

## Luồng sử dụng điển hình

```
1. POST /api/Auth/login
      ↓ nhận được { "token": "..." }

2. GET /api/BienBanGiaoNhan?FromDate=...&ToDate=...
      Header: Authorization: Bearer <token>
      ↓ nhận danh sách biên bản giao nhận có phân trang
```

---

## Mã lỗi tham chiếu

| HTTP Status | Ý nghĩa |
|---|---|
| 200 | Thành công |
| 401 | Chưa xác thực hoặc token không hợp lệ / hết hạn |
