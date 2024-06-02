# Men Core Car Rental

Men Core Car Rental, araç kiralama işlemlerini yönetmek için tasarlanmış bir .NET Core uygulamasıdır. Bu uygulama, kullanıcıların araçları listelemesine, araçları kiralamasına ve kiralanan araçların yönetimine olanak sağlar.

## Başlarken

Bu bölüm, projeyi yerel makinenizde çalıştırmanız için gerekli adımları içerir.

### Gereksinimler

- .NET 5.0 veya üstü
- SQL Server 2019 veya üstü

### Kurulum

1. Projeyi GitHub'dan klonlayın:

```bash
git clone https://github.com/muraterennar/MenCoreCarRental.git
```

2. Solution dosyasını Visual Studio veya Rider'da açın.

3. `MenData` projesindeki `appsettings.json` dosyasını açın ve aşağıdaki kısımları kendi veritabanı bilgilerinize göre düzenleyin.

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.....;Database=......;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
```

4.  `RentACar.Persistence` projesine sağ tıklayın ve `Terminalde` açın. Daha sonra aşağıdaki komutu çalıştırarak veritabanını oluşturun.

```bash
dotnet ef database update
```

5. Projeyi çalıştırın.

## Kullanılan Teknolojiler

- .NET 8.0
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- MSSQL
- FluentValidation
- MailKit
- AOP
- JWT
- Serilog
- OTP.NET
- Syetem.Linq.Dynamic.Core
- MediatR
- RedisCache
- AutoMapper
- Swagger

## Katkıda Bulunma

1. Bu projeyi fork edin.
2. Yeni bir dal (branch) oluşturun (`git checkout -b feature/fooBar`).
3. Değişikliklerinizi kaydedin (`git commit -am 'Add some fooBar'`).
4. Dalınıza push yapın (`git push origin feature/fooBar`).
5. Bir pull request oluşturun.

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için [LICENSE](https://github.com/muraterennar/menCorePackages/blob/b456511208d0e7e43272f25d0e56ed4e00e89a75/LICENSE)