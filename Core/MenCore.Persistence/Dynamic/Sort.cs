namespace MenCore.Persistence.Dynamic;

#region Sort
// Bu sınıf, bir sıralama işlemini temsil eder.
// Belirli bir alana ve sıralama yönüne sahip sıralama kriterlerini içerir.
#endregion
public class Sort
{
    // Sıralama kriterinin uygulanacağı alanı temsil eden özellik.
    public string Field { get; set; }

    // Sıralama yönünü temsil eden özellik.
    public string Dir { get; set; }

    // Yapılandırıcı metot: Varsayılan değerlerle bir Sort örneği oluşturur.
    public Sort ()
    {
        Field = string.Empty;
        Dir = string.Empty;
    }

    // Yapılandırıcı metot: Belirli bir alan ve sıralama yönüyle bir Sort örneği oluşturur.
    public Sort (string field, string dir)
    {
        Field = field;
        Dir = dir;
    }
}