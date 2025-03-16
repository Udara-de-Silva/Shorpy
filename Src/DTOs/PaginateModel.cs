/*
File-Name: PaginateModel
File-Description: contains the paginate model which will be bound to the request from the client side.
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
namespace Shorpy.DTOs;
public class PaginateModel
{
  public int Limit { get; set; }
  public int Offset { get; set; }
  public IEnumerable<SearchModel>? Search { get; set; }
  public IEnumerable<SortModel>? Sort { get; set; }
  public IEnumerable<string>? Include { get; set; }
}