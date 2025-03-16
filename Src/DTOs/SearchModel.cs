/*
File-Name: SearchModel
File-Description: contains the search model which will be bound to the user request from the client side
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
namespace Shorpy.DTOs;
public class SearchModel
{
  public string method { get; set; } = "equal";
  public string value { get; set; } = string.Empty;
  public string field { get; set; } = string.Empty;
  public bool isAnd { get; set; } = true;

  public SearchModel? and { get; set; } 
  public SearchModel? or { get; set; } 
}