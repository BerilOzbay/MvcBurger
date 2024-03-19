using System.ComponentModel.Design;
using System.Drawing;
using static MvcBurger.Enums.Buyuluk;

namespace MvcBurger.Entities
{
    public class Siparis
    {
        public int Id { get; set; }
        public double ToplamFiyat
        {
            get
            {
                double totalPrice = 0;

                if (Menuler != null)
                {
                    foreach (var menu in Menuler)
                    {
                        // Menü fiyatı boyuta göre hesaplanıyor
                        switch (Buyukluk)
                        {
                            case Buyukluk.Kucuk:
                                totalPrice = menu.Fiyat * SiparisSayisi;
                                break;
                            case Buyukluk.Orta:
                                totalPrice = (menu.Fiyat * SiparisSayisi) +50 ;
                                break;
                            case Buyukluk.Buyuk:
                                totalPrice = (menu.Fiyat * SiparisSayisi)+100;
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (EkstraMalzemeler != null)
                {
                    foreach (var ekstra in EkstraMalzemeler)
                    {
                        totalPrice += ekstra.Fiyat;
                    }
                }

                return totalPrice;
            }
        }
        public int SiparisSayisi { get; set; }
        public Buyukluk Buyukluk { get; set; }
        public ICollection<Menu> Menuler { get; set; }
        public ICollection<EkstraMalzeme> EkstraMalzemeler { get; set; }

    }
}
