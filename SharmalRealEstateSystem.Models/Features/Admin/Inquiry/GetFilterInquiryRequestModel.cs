using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.Inquiry
{
    public class GetFilterInquiryRequestModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? Status { get; set; }
        public string? InquiryStatus { get; set; }
        public string? FilterType { get; set; }
    }
}
