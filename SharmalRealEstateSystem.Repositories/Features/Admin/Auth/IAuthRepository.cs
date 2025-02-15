namespace SharmalRealEstateSystem.Repositories.Features.Admin.Auth;

public interface IAuthRepository
{
    Task<Result<UserListResponseModel>> GetUserList(int pageNo, int pageSize);
    Task<Result<JwtResponseModel>> Login(LoginRequestModel requestModel);
    Task<Result<JwtResponseModel>> LoginV1(LoginRequestModel requestModel);
    Task<Result<AuthResponseModel>> Register(RegisterRequestModel requestModel);
    Task<Result<AuthResponseModel>> ResetFailCount(string userId);
    Task<Result<AuthResponseModel>> UpdateUser(
        UpdateAuthRequestModel requestModel,
        string userId,
        string decryptedActionMakerId
    );
    Task<Result<AuthResponseModel>> UpdateProfile(
        UpdateProfileRequestModel requestModel,
        string userId,
        string decryptedActionTakerId
    );
    Task<Result<AuthResponseModel>> UpdatePassword(
        UpdatePasswordRequestModel requestModel,
        string userId,
        string decryptedActionTakerId
    );
    Task<Result<AuthResponseModel>> DeleteUser(string userId);
}
