using SharmalRealEstateSystem.Models.Enums.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.Inquiry
{
    public class InquiryDataModel
    {
        public string InquiresId { get; set; } = null!;

        public string? PropertyId { get; set; }

        public string? CarId { get; set; }

        public string? CreatedDate { get; set; }

        public string? UpdatedDate { get; set; }

        public string UserName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsDone { get; set; }

        public bool IsDeleted { get; set; }

        public string? Status { get; set; }

        public string InquiryStatus => IsDone ? EnumInquiryStatus.Done.ToString() : EnumInquiryStatus.Unread.ToString();
    }
}
