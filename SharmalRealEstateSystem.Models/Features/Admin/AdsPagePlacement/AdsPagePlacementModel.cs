using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.AdsPagePlacement
{
    public class AdsPagePlacementModel
    {
        public string AdsPagePlacementId { get; set; } = null!;

        public string? AdsId { get; set; }

        public string? AdsPageId { get; set; }
    }
}
