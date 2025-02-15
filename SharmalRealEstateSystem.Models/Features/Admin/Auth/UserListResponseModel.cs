using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharmalRealEstateSystem.Models.Features.Admin.Auth
{
    public record UserListResponseModel(List<UserModel> DataLst, PageSettingModel PageSetting);
}
