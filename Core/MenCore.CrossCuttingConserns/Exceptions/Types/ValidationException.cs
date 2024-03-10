namespace MenCore.CrossCuttingConserns.Exceptions.Types
{
    #region ValidationException sınıfı, doğrulama istisnaları için kullanılır
    public class ValidationException : Exception
    {
        // Doğrulama hatalarını içeren bir koleksiyon
        public IEnumerable<ValidationExceptionModel> Errors { get; set; }

        #region Parametresiz kurucu metod, varsayılan hataları boş bir koleksiyon ile ayarlar
        public ValidationException () : base()
        {
            Errors = Array.Empty<ValidationExceptionModel>();
        }
        #endregion

        #region Bir mesajla kurucu metod, varsayılan hataları boş bir koleksiyon ile ayarlar
        public ValidationException (string? message) : base(message)
        {
            Errors = Array.Empty<ValidationExceptionModel>();
        }
        #endregion

        #region Bir mesaj ve iç istisna ile kurucu metod, varsayılan hataları boş bir koleksiyon ile ayarlar
        public ValidationException (string? message, Exception? exception) : base(message, exception)
        {
            Errors = Array.Empty<ValidationExceptionModel>();
        }
        #endregion

        #region Doğrulama hatalarını içeren bir koleksiyonla kurucu metod, mesajı bu hatalara göre oluşturur
        public ValidationException (IEnumerable<ValidationExceptionModel> errors) : base(BuilderErrorMessage(errors))
        {
            Errors = errors;
        }
        #endregion

        #region Doğrulama hatalarından oluşturulan hata mesajını döndüren yardımcı metot
        private static string? BuilderErrorMessage (IEnumerable<ValidationExceptionModel> errors)
        {
            IEnumerable<string> arr = errors.Select(
                x => $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors)}"
                );
            return $"Validation failed: {string.Join(string.Empty, arr)}";
        }
        #endregion

    }
    #endregion

    #region Doğrulama hatalarını temsil eden model sınıfı
    public class ValidationExceptionModel
    {
        // Hatanın ilgili özelliği
        public string? Property { get; set; }

        // Hata mesajları
        public IEnumerable<string>? Errors { get; set; }
    }
    #endregion
}
