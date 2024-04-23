using System.Collections;
using System.Linq.Expressions;
using MenCore.Persistence.Dynamic;
using MenCore.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MenCore.Persistence.Repositories;

// Bu sınıf, genel Entity Framework temelli veritabanı işlemlerini gerçekleştirir.
// TEntity: İşlem yapılan varlık türü.
// TEntityId: İşlem yapılan varlığın benzersiz kimliği.
// TContext: Entity Framework veritabanı bağlamı türü.
public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>,
    IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TContext : DbContext
{
    protected readonly TContext _context;

    // Dependency Injection ile veritabanı bağlamını alır.
    public EfRepositoryBase(TContext context)
    {
        _context = context;
    }

    // Veritabanı sorgusunu temsil eden IQueryable nesnesini döndürür.
    public IQueryable<TEntity> Query()
    {
        return _context.Set<TEntity>();
    }

    #region ------------- Async Operations -------------

    // Asenkron olarak filtrelenmiş, sıralanmış, ilişkilendirilmiş ve sayfalı bir liste döndürür.
    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 10,
        bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    // Dinamik sorgu kullanarak asenkron olarak filtrelenmiş, ilişkilendirilmiş ve sayfalı bir liste döndürür.
    public async Task<Paginate<TEntity>> GetListByDynamicAsync(DynamicQuery dynamic,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 10,
        bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    // Asenkron olarak belirli bir ölçüt ile varlık alır.
    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool withDeleted = false,
        bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    // Asenkron olarak belirli bir ölçüte göre varlık var mı kontrol eder.
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null, bool withDeleted = false,
        bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }

    // Asenkron olarak bir varlık ekler.
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Asenkron olarak bir koleksiyon varlık ekler.
    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedDate = DateTime.UtcNow;

        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    // Asenkron olarak bir varlık günceller.
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Asenkron olarak bir koleksiyon varlık günceller.
    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.UpdatedDate = DateTime.UtcNow;
        _context.UpdateRange(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    // Asenkron olarak bir varlık siler.
    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Asenkron olarak bir koleksiyon varlık siler.
    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
        await _context.SaveChangesAsync();
        return entities;
    }

    #endregion

    #region ------------- Sync Operations -------------

    // Senkron olarak filtrelenmiş, sıralanmış, ilişkilendirilmiş ve sayfalı bir liste döndürür.
    public Paginate<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 10,
        bool withDeleted = false, bool enableTracking = true)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable).ToPaginate(index, size);
        return queryable.ToPaginate(index, size);
    }

    // Senkron olarak dinamik sorgu kullanarak filtrelenmiş, ilişkilendirilmiş ve sayfalı bir liste döndürür.
    public Paginate<TEntity> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 10,
        bool withDeleted = false, bool enableTracking = true)
    {
        var queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.ToPaginate(index, size);
    }

    // Senkron olarak belirli bir ölçüt ile varlık alır.
    public TEntity Get(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool withDeleted = false,
        bool enableTracking = true)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return queryable.FirstOrDefault(predicate);
    }

    // Senkron olarak belirli bir ölçüte göre varlık var mı kontrol eder.
    public bool Any(Expression<Func<TEntity, bool>> predicate = null, bool withDeleted = false,
        bool enableTracking = true)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.Any();
    }

    // Senkron olarak bir varlık ekler.
    public TEntity Add(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        _context.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    // Senkron olarak bir koleksiyon varlık ekler.
    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.CreatedDate = DateTime.UtcNow;
        _context.AddRange(entities);
        _context.SaveChanges();
        return entities;
    }

    // Senkron olarak bir varlık günceller.
    public TEntity Update(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    // Senkron olarak bir koleksiyon varlık günceller.
    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.UpdatedDate = DateTime.UtcNow;
        _context.UpdateRange(entities);
        _context.SaveChanges();
        return entities;
    }

    // Senkron olarak bir varlık siler.
    public TEntity Delete(TEntity entity, bool permanent = false)
    {
        SetEntityAsDeleted(entity, permanent);
        _context.SaveChanges();
        return entity;
    }

    // Senkron olarak bir koleksiyon varlık siler.
    public ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false)
    {
        SetEntityAsDeleted(entities, permanent);
        _context.SaveChanges();
        return entities;
    }

    #endregion

    #region ------------- Helper Methods -------------

    // Varlığı silmek için bir yöntem çağrısında çağrılmak üzere ayarlanmış, bu işlem veritabanı bağlamı tarafından yapılır.
    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await setEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            _context.Remove(entity);
        }
    }

    // Bir varlık koleksiyonunu silmek için bir yöntem çağrısında çağrılmak üzere ayarlanmış, bu işlem veritabanı bağlamı tarafından yapılır.
    protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }

    // Varlığı silmek için bir yöntem çağrısında çağrılmak üzere ayarlanmış, bu işlem veritabanı bağlamı tarafından yapılır.
    protected void SetEntityAsDeleted(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            setEntityAsSoftDeleted(entity);
        }
        else
        {
            _context.Remove(entity);
        }
    }

    // Bir varlık koleksiyonunu silmek için bir yöntem çağrısında çağrılmak üzere ayarlanmış, bu işlem veritabanı bağlamı tarafından yapılır.
    protected void SetEntityAsDeleted(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (var entity in entities)
            SetEntityAsDeleted(entity, permanent);
    }

    // Bir ilişkisel yükleyici sorgusu oluşturmak için yardımcı bir yöntem.
    protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        var queryProviderType = query.Provider.GetType();
        var createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((IEntityTimeStamps)x).DeletedDate.HasValue);
    }

    // Varlığın tek bir ilişkisi olduğunda, silme işlemi sırasında problemler oluşturabilecek bir yöntem.
    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        var foreignKeys = _context.Entry(entity).Metadata.GetForeignKeys();
        var hasEntityHaveOneToOneRelation =
            foreignKeys.Any()
            && foreignKeys.All(x =>
                x.DependentToPrincipal?.IsCollection == true
                || x.PrincipalToDependent?.IsCollection == true
                || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
            );
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key."
            );
    }

    // Bir varlığı soft delete (kalıcı olmayan silme) olarak işaretlemek için asenkron bir yöntem.
    private async Task setEntityAsSoftDeletedAsync(IEntityTimeStamps entity)
    {
        if (entity.DeletedDate.HasValue)
            return;
        entity.DeletedDate = DateTime.UtcNow;

        var navigations = _context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x =>
                x is
                {
                    IsOnDependent: false,
                    ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
                }
            )
            .ToList();
        foreach (var navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            var navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    var query = _context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType())
                        .ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimeStamps navValueItem in (IEnumerable)navValue)
                    await setEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    var query = _context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await setEntityAsSoftDeletedAsync((IEntityTimeStamps)navValue);
            }
        }

        _context.Update(entity);
    }

    // Bir varlığı soft delete (kalıcı olmayan silme) olarak işaretlemek için senkron bir yöntem.
    private void setEntityAsSoftDeleted(IEntityTimeStamps entity)
    {
        if (entity.DeletedDate.HasValue)
            return;
        entity.DeletedDate = DateTime.UtcNow;

        var navigations = _context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x =>
                x is
                {
                    IsOnDependent: false,
                    ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
                }
            )
            .ToList();
        foreach (var navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            var navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    var query = _context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType()).ToList();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimeStamps navValueItem in (IEnumerable)navValue)
                    setEntityAsSoftDeleted(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    var query = _context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType())
                        .FirstOrDefault();
                    if (navValue == null)
                        continue;
                }

                setEntityAsSoftDeleted((IEntityTimeStamps)navValue);
            }
        }

        _context.Update(entity);
    }

    #endregion
}