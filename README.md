
# Facebook API ve Hangfire Entegrasyonu

Bu proje, **Facebook API** ile **Hangfire** entegrasyonunu bir .NET ortamında nasıl gerçekleştirebileceğinizi gösterir. Bu entegrasyon sayesinde, Hangfire kullanarak Facebook ile ilgili görevleri arka planda zamanlayabilir ve yönetebilirsiniz.

## Özellikler

- **Facebook API Entegrasyonu**: Facebook verilerine API aracılığıyla erişim sağlar ve bunlarla etkileşimde bulunur.
- **Hangfire Desteği**: Arka planda görevlerin işlenmesini ve zamanlanmasını sağlar.
- **Kolay Zamanlama**: Arka plan görevlerini belirli bir zaman diliminde veya periyodik olarak çalıştırabilirsiniz.
- **Görev Yönetimi**: Hangfire Dashboard ile görevleri takip edebilir ve yönetebilirsiniz.

## Kullanılan Teknolojiler

- **ASP.NET Core**
- **Hangfire**
- **Facebook API**

## Kurulum

1. **Proje Deposu**nu klonlayın:
    ```bash
    git clone https://github.com/BurakSekmenn/Facebook-api-and-hangfire.git
    ```
   
2. **Gerekli Bağımlılıkları Yükleyin**:
    ```bash
    dotnet restore
    ```

3. **Facebook API Anahtarlarını** ayarlayın:
   - Facebook API erişimi için gerekli olan `App ID` ve `App Secret` bilgilerini proje ayar dosyasına ekleyin.

4. **Veritabanı Bağlantısını** yapılandırın:
   - Hangfire'ın kullanacağı veritabanı bağlantı ayarlarını `appsettings.json` dosyasında güncelleyin.

5. **Uygulamayı Çalıştırın**:
    ```bash
    dotnet run
    ```

## Hangfire Dashboard

Hangfire Dashboard ile arka plan görevlerini takip edebilirsiniz. Hangfire Dashboard'a tarayıcı üzerinden şu adresten erişebilirsiniz:

```
http://localhost:5000/hangfire
```

## Katkıda Bulunma

Katkıda bulunmak için lütfen bir pull request gönderin veya bir issue açın.


