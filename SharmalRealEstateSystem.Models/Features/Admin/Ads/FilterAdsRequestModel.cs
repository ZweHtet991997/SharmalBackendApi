using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.Ads
{
    public class FilterAdsRequestModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? Status { get; set; }
        public string? Pages { get; set; }
        public string? Layout { get; set; }
    }
}
