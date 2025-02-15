using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.Dashboard
{
    public class DashboardResponseModel
    {
        public int TotalInquiryCount { get; set; }
        public int InquiryPropertyForSaleCount { get; set; }
        public int InquiryPropertyForRentCount { get; set; }
        public int TotalPropertyCount { get; set; }
        public int PropertyForSaleCount { get; set; }
        public int PropertyForRentCount { get; set; }
        public int TotalCarCount { get; set; }
        public int InquiryCarCount { get; set; }
        public int InquiryOtherCount { get; set; }
        public int TotalAdsCount { get; set; }
    }
}
