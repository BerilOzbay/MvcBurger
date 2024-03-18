using System.ComponentModel.Design;
using static MvcBurger.Enums.Buyuluk;

namespace MvcBurger.Entities
{
    public class Siparis
    {
        public int Id { get; set; }
        public double ToplamFiyat { get; set; }
        public int SiparisSayisi { get; set; }
        public Buyukluk Buyukluk { get; set; }
        public ICollection<Menu> Menuler { get; set; }
        public ICollection<EkstraMalzeme> EkstraMalzemeler { get; set; }

        public double ToplamFiyatHesapla(Menu menu, List<EkstraMalzeme> ekstraMalzemeler, Buyukluk buyukluk)
        {
            return 0;
        }

    }
}
