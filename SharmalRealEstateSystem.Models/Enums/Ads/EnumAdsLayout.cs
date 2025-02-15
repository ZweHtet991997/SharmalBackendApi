using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Enums.Ads
{
    public class AdsLayout
    {
        public static List<string> GetAdsLayout()
        {
            return new List<string>
            {
                "Banner",
                "Carousel",
                "Side Bar"
            };
        }
    }
}
