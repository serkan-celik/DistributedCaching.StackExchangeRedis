using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace StackExchangeRedisCache.Controllers
{
    public class ProductController : Controller
    {
        IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        //Metinsel türde key-value tarzında veri depolamasını gerçekleştiren metot
        public IActionResult CacheSetString()
        {
            _distributedCache.SetString("date", DateTime.Now.ToString());
            return View();
        }

        //Metinsel türde key-value tarzında veri depolamasını gerçekleştiren metot
        //Cache ömrünün yapılanrılması
        public IActionResult CacheSetString2()
        {
            _distributedCache.SetString("date", DateTime.Now.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(5),//Mutlak ömür
                SlidingExpiration = TimeSpan.FromSeconds(60) //İstekli periyodik ömür
            });
            return View();
        }

        //Metinsel türde depolanmış verilerden key değerine karşılık value değerini döndüren fonksiyon
        public string CacheGetString()
        {
            string value = _distributedCache.GetString("date");
            return value;
        }

        //Key değeri verilen datayı silen metot
        public IActionResult CacheRemove()
        {
            _distributedCache.Remove("date");
            return View();
        }

        //Cache’de binary olarak data tutmamızı sağlayan fonksiyon
        public IActionResult CacheSet()
        {
            byte[] dateByte = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
            _distributedCache.Set("date", dateByte);
            return View();
        }

        //Cache’de binary olarak data tutmamızı sağlayan fonksiyon
        public IActionResult CacheSet2()
        {
            byte[] dateByte = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
            _distributedCache.Set("date", dateByte, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(1200),
                SlidingExpiration = TimeSpan.FromSeconds(60)
            });
            return View();
        }

        //Binary olarak tutulan datayı geri binary olarak elde etmemizi sağlayan fonksiyon
        public IActionResult CacheGet()
        {
            byte[] dateByte = _distributedCache.Get("date");
            string value = Encoding.UTF8.GetString(dateByte);
            return View();
        }

        //Resim dosyasının cachelendiği örnek kod bloğu
        public IActionResult CacheFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resim.jpg");
            byte[] fileByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("file", fileByte);
            return View();
        }

        //Cachelenmiş Dosyayı Okuma
        public IActionResult CacheFileRead()
        {
            byte[] fileByte = _distributedCache.Get("file");
            return File(fileByte, "image/jpg");
        }
    }
}
