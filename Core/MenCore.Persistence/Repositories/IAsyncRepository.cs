using System.Linq.Expressions;
using MenCore.Persistence.Dynamic;
using MenCore.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;

namespace MenCore.Persistence.Repositories;

public interface IAsyncRepository<TEntity, TEntityId> : IQuary<TEntity> where TEntity : Entity<TEntityId>
{
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<Paginate<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> AddAsync(TEntity entity);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);

    #region GetAsync

    // Belirli bir koşulu sağlayan bir varlık (entity) almak için kullanılır.
    // Koşul, bir lambda ifadesiyle belirtilir.
    // İlgili varlıkla ilişkili nesneleri de almak için `include` parametresi kullanılabilir.
    // Eğer `withDeleted` parametresi true ise, silinmiş varlıklar da döndürülür.
    // `enableTracking` parametresi, varlıkların değişiklik izleme (change tracking) durumunu belirler.
    // İşlem iptal edilmek istenirse `cancellationToken` kullanılır.

    #endregion

    #region GetListAsync

    // Belirli bir koşula uygun varlıkları liste olarak getirmek için kullanılır.
    // İsteğe bağlı olarak koşul, sıralama, ilişkili nesnelerin dahil edilmesi, sayfalama gibi parametreler sağlanabilir.
    // Eğer `withDeleted` parametresi true ise, silinmiş varlıklar da döndürülür.
    // `enableTracking` parametresi, varlıkların değişiklik izleme (change tracking) durumunu belirler.
    // İşlem iptal edilmek istenirse `cancellationToken` kullanılır.

    #endregion

    #region GetListByDynamicAsync

    // Dinamik sorguları desteklemek için kullanılır.
    // `dynamic` parametresi, dinamik sorgu bilgilerini içerir.
    // İsteğe bağlı olarak bir koşul belirtilebilir.
    // Sayfalama için `index` ve `size` parametreleri kullanılır.
    // İlgili varlıkla ilişkili nesneleri almak için `include` parametresi kullanılabilir.
    // Eğer `withDeleted` parametresi true ise, silinmiş varlıklar da döndürülür.
    // `enableTracking` parametresi, varlıkların değişiklik izleme (change tracking) durumunu belirler.
    // İşlem iptal edilmek istenirse `cancellationToken` kullanılır.

    #endregion

    #region AnyAsync

    // Belirli bir koşulu sağlayan herhangi bir varlık olup olmadığını kontrol etmek için kullanılır.
    // Koşul, bir lambda ifadesiyle belirtilir.
    // Eğer `withDeleted` parametresi true ise, silinmiş varlıklar da dikkate alınır.
    // `enableTracking` parametresi, varlıkların değişiklik izleme (change tracking) durumunu belirler.
    // İşlem iptal edilmek istenirse `cancellationToken` kullanılır.

    #endregion

    #region AddAsync

    // Yeni bir varlık eklemek için kullanılır.
    // Eklenen varlık geri döndürülür.

    #endregion

    #region AddRangeAsync

    // Bir koleksiyon içindeki varlıkları toplu olarak eklemek için kullanılır.
    // Eklenen varlıklar geri döndürülür.

    #endregion

    #region UpdateAsync

    // Varlığın güncellenmesi için kullanılır.
    // Güncellenen varlık geri döndürülür.

    #endregion

    #region UpdateRangeAsync

    // Bir koleksiyon içindeki varlıkları toplu olarak güncellemek için kullanılır.
    // Güncellenen varlıklar geri döndürülür.

    #endregion

    #region DeleteAsync

    // Varlığı silmek için kullanılır.
    // Eğer `permanent` parametresi true ise, varlık kalıcı olarak silinir.

    #endregion

    #region DeleteRangeAsync

    // Bir koleksiyon içindeki varlıkları toplu olarak silmek için kullanılır.
    // Eğer `permanent` parametresi true ise, varlıklar kalıcı olarak silinir.

    #endregion
}