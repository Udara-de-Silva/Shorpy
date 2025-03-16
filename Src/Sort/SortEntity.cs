/*
File-Name: SortEntity
File-Description: depicts the sort entity
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/

using System.Linq.Expressions;
namespace Shorpy.Sort;
public class SortEntity
{
  public Expression Field { get; set; } = null!;
  public bool IsDesc { get; set; }
  public object[]? CustomSortOrder {get;set;}
}