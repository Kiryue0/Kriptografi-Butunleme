Proje Özeti: SHA-512 ve ECC Tabanlı Kriptografi Simülasyonu
Bu proje, modern kriptografi kavramlarını temel alan iki ana modül içerir: SHA-512 özet fonksiyonu ve Eliptik Eğri Kriptografisi (ECC) tabanlı anahtar değişimi ile AES şifreleme/şifre çözme işlemleri.

Giriş sayfası
Bu giriş sayfasından Sha512 veya Ecc Seçilebilinir
![image](https://github.com/user-attachments/assets/fdb85da3-1e97-4321-8179-0da6df66ce32)

SHA-512 Modülü
Amaç: Verilen metin ya da dosyaların SHA-512 algoritması ile özetini (hash) çıkarmak.
![image](https://github.com/user-attachments/assets/c0bc7da4-04ed-45bb-be14-88f39f0f5e7d)

Nasıl çalışır:


Kullanıcı arayüzüne metin girebilir veya dosya yükleyebilir.

Sistem, girilen verinin SHA-512 hash’ini hesaplar ve gösterir.

Kullanım alanı: Veri bütünlüğü kontrolü, parola saklama, dijital imza gibi güvenlik uygulamalarında yaygın olarak kullanılır.

ECC Modülü
![image](https://github.com/user-attachments/assets/0e6bbca0-18c1-4bc5-b667-545bd333dcf3)

Amaç:

İki kullanıcı arasında güvenli iletişim için Eliptik Eğri Kriptografisi kullanarak anahtar değişimi ve AES şifreleme yapmak.

Kullanıcıların birbirlerinin public key’lerini paylaşmasıyla ortak gizli anahtar oluşturması.

Ortak anahtar üzerinden AES ile mesajların şifrelenmesi ve çözülmesi.

Nasıl çalışır:

Her kullanıcı kendi private ve public key çiftini oluşturur (private key gizlidir).

Karşı tarafın public key’i, kullanıcı tarafından manuel girilir ve “Eşle” butonuyla ortak anahtar hesaplanır.

Ortak anahtar kullanılarak kullanıcı mesajını şifreler.

Karşı taraf, kendi private key’i ve gönderilen public key üzerinden ortak anahtarı oluşturup mesajı çözer.

Simülasyon özellikleri:

UI ikiye bölünmüş, her kullanıcı kendi anahtarlarını ve işlemlerini görebiliyor.

Private key’ler gösterilmeyerek güvenlik sağlanmıştır.

Ortak anahtar oluşturulmadan şifreleme/şifre çözme işlemleri aktif değildir.

AES şifreleme/deşifreleme işlemi güvenli ortak anahtar kullanılarak gerçekleştirilir.

Kullanılan Teknolojiler
ASP.NET Core MVC (.NET 8)

C# ile kriptografi işlemleri

System.Security.Cryptography namespace

Bootstrap 5 ile kullanıcı arayüzü

SHA-512 ve ECC algoritmaları
