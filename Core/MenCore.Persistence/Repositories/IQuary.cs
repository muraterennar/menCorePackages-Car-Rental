namespace MenCore.Persistence.Repositories;

public interface IQuary<T>
{
    IQueryable<T> Query();

    #region Query

    // Veritabanı sorguları oluşturmak için kullanılan bir IQueryable örneği döndürür.

    #endregion
}