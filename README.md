# Shorpy

Shorpy is a powerful and easy-to-use .NET library that provides seamless dynamic Pagination, Searching, and Sorting for applications using **Entity Framework Core**, **ASP.NET Core WebAPI**, and written in **C#**. It aims to simplify data handling and improve performance by offering efficient querying mechanisms.

---

## 📦 Installation

Install via NuGet Package Manager:

```bash
Install-Package Shorpy
```

Or via .NET CLI:

```bash
dotnet add package Shorpy
```

---

## 🔧 Features

- ✅ **Dynamic Pagination:** Effortlessly handle large datasets with automatic page splitting.
- 🔍 **Search Filtering:** Apply search criteria dynamically across multiple columns.
- 🔄 **Sorting:** Enable sorting by various properties in ascending or descending order.
- 📚 **Entity Framework Core Integration:** Designed to work seamlessly with EF Core queries.
- 🌐 **ASP.NET Core WebAPI Support:** Simple integration with WebAPI controllers.

---

## 🚀 Getting Started

### 1. Configure Your SnSAbles and Includables

In your application, create SnSAble (Search and Sortables) and Includables for the Entities you want to paginate.

Eg: Assume you have a two entities in your Context. Namely, `Employee` and `Department`. With each `Employee` belonging to a single `Department`

Your SnSAble for the `Employee` entity will look like this.

```csharp
using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Employee, object>>;
public class SnSEmployee: ISnSAble
{
    public static readonly type Name = e=>e.Name;
}
```

Your Includable for the `Employee` entity will look like this.

```csharp
using System.Linq.Expressions;
using Shorpy.Interfaces;
using Shorpy.Tests.Entities;

namespace Shorpy.Tests.SnSAbles;

using type = Expression<Func<Employee, object>>;
public class IncEmployee: IIncludable
{
    public static readonly type Department = e=>e.DepartmentNavigation;
}
```

---

### 2. Handle a paginate request from your controller and let the `front-end` go nuts!

```csharp
using Shorpy.DTOs;
using Shorpy.Paginate;
.....
[HttpPost]
public virtual async Task<IActionResult> Paginate(PaginateModel pm)
{
    // you must pass your DBContext to the paginate function
    var paginatedObj = await Paginate<Employee, SnSEmployee, IncEmployee>.PaginateWithTracking(_dbcontext, pm);

    return Ok(paginatedObj);
}
```

---

## 📌 Usage

### Request Example

```bash
curl -X POST /api/employees/paginate \
-H "Content-Type: application/json" \
-d '{
    "search": [
        {
            "field": "name",
            "value": "Tim",
            "method": "equal",
        }
    ],
    "include": ['Department'],
    "limit": 10,
    "offset":0
}'
```
Supported search methods:

- equal
- gt
- lt
- notequal
- like
- wherein
- wherepartiallyin

### Response Example

```json
{
  "values": [
    {
      "id": 1,
      "name": "Tim",
      "department": 1,
      "telephone": "000-0000-0000"
    }
  ],
  "total": 1
}
```
---

## 🔨 Contributing

Contributions are welcome! Please submit a pull request or open an issue for discussion.

---

## 📃 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 💬 Support

If you have any questions or need help, feel free to open an issue or reach out.
