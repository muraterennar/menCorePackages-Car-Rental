namespace MenCore.Persistence.Repositories;

public interface IQuary<T>
{
    #region Query
    // Veritabanı sorguları oluşturmak için kullanılan bir IQueryable örneği döndürür.
    #endregion
    IQueryable<T> Query ();
}