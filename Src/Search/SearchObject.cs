/*
File-Name: SearchObject
File-Description: contains classes and enums which represent search criteria
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16,
Updated-On: 2025-03-21 udara@thesoftwarefirm.lk => added gte and lte Search Methods
Version: 1.0
*/
using System.Linq.Expressions;
namespace Shorpy.Search;

public enum SearchCondition
{
  and,
  or
}

public enum SearchMethods
{
  equal,
  gt,
  lt,
  gte,
  lte,
  notequal,
  like,
  wherein,
  wherepartiallyin
}

public class SearchEntity
{
  public Expression Field { get; set; } = null!;
  public SearchMethods Method { get; set; }
  public string Value { get; set; } = null!;
  public SearchEntity? And { get; set; }
  public SearchEntity? Or { get; set; }
  public SearchCondition Condition { get; set; }
}