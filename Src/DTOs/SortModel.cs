/*
File-Name: SortModel
File-Description: contains the sort model which will be bound to the user request from the client side
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/
namespace Shorpy.DTOs;
public class SortModel
{
  public string field { get; set; } = null!;
  public bool? isDesc { get; set; }
}