/*
File-Name: Paginate
File-Description: Everything comes together here!
Author: Udara de Silva <udara@thesoftwarefirm.lk>
Created-On: 2025-03-16
Version: 1.0
*/

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shorpy.DTOs;
using Shorpy.Helpers;
using Shorpy.Interfaces;
using Shorpy.Paginate.Converters;
using Shorpy.Search;
using Shorpy.Sort;

namespace Shorpy.Paginate;
public class Paginate<TEntity, TSnSAble, TIncludable> where TEntity : class where TSnSAble : ISnSAble? where TIncludable : IIncludable?
{

    public static async Task<PaginatedObject<TEntity>> PaginateWithTracking(DbContext context, PaginateModel pm)
    {
        return await ExecutePaginate(context, true, pm);
    }

    public static async Task<PaginatedObject<TEntity>> PaginateWithTracking(DbContext context, int offset = 0, int limit = 0, IEnumerable<SearchEntity>? SeO = null, IEnumerable<SortEntity>? SrO = null, params LambdaExpression[] includes)
    {
        return await ExecutePaginate(context, true, offset, limit, SeO, SrO, includes);
    }

    public static async Task<PaginatedObject<TEntity>> PaginateWithoutTracking(DbContext context, PaginateModel pm)
    {
        return await ExecutePaginate(context, false, pm);
    }

    public static async Task<PaginatedObject<TEntity>> PaginateWithoutTracking(DbContext context, int offset = 0, int limit = 0, IEnumerable<SearchEntity>? SeO = null, IEnumerable<SortEntity>? SrO = null, params LambdaExpression[] includes)
    {
        return await ExecutePaginate(context, false, offset, limit, SeO, SrO, includes);
    }

    private static async Task<PaginatedObject<TEntity>> ExecutePaginate(DbContext Context, bool track, PaginateModel pm)
    {
        var SnSAble = typeof(TSnSAble);
        var Includable = typeof(TIncludable);

        // convert search entity
        IEnumerable<SearchEntity>? search = SnSAble is not null ? SearchModelConverter.Convert(pm.Search, SnSAble) : null;
        // convert sort entity
        IEnumerable<SortEntity>? sort = SnSAble is not null ? SortModelConverter.Convert(pm.Sort, SnSAble) : null;
        // convert includables
        IEnumerable<LambdaExpression>? includes = Includable is not null ? IncludesConverter.Convert(pm.Include, Includable) : null;

        return await ExecutePaginate(Context, track, pm.Offset, pm.Limit, search, sort, includes?.ToArray() ?? Array.Empty<LambdaExpression>());
    }
    private static async Task<PaginatedObject<TEntity>> ExecutePaginate(DbContext Context, bool track, int offset = 0, int limit = 0, IEnumerable<SearchEntity>? SeO = null, IEnumerable<SortEntity>? SrO = null, params LambdaExpression[] includes)
    {
        var po = new PaginatedObject<TEntity>();
        // make the set queryable
        IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();

        // set search predicate
        if (SeO is not null)
            query = query.Where(SeO);
        // set total => only if the offset is set to 0 and the limit is not 0 || null. This way, the total count is returned only during the initial pagination
        if (offset == 0 && limit != 0)
            po.Total = await query.AsNoTracking().CountAsync();

        // add first level includes
        if (includes is not null)
            foreach (var include in includes)
            {
                var i = include as Expression<Func<TEntity, Object>>;
                if (i != null)
                    query = query.Include(i);
            }

        // set sort predicate
        if (SrO is not null)
            query = query.OrderBy(SrO);
        // set offset
        if (offset != 0)
            query = query.Skip(offset);
        // set limit
        if (limit != 0)
            query = query.Take(limit);
        // set values
        if (track)
            po.Values = await query.ToListAsync();
        else
            po.Values = await query.AsNoTracking().ToListAsync();

        return po;
    }

}