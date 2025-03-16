/*
File-Name: PaginatedObject
File-Description: Depicts a paginated object; the return type of the paginate feature
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
namespace Shorpy.Paginate;
public class PaginatedObject<T>
{
  public int Total { get; set; }
  public IEnumerable<T>? Values { get; set; }
}