using training_project_backend.DTOs;
using training_project_backend.Requests;

namespace training_project_backend.Providers
{
    public interface IJwtProvider
    {
        Task<List<JwtSummaryResponse>> GetAllAsync();

        Task<JwtDetailsResponse?> GetByIdAsync(Guid id);

        Task<UploadJwtResponse> UploadAsync(UploadJwtRequest request);

        Task<ValidateJwtResponse>
    ValidateAsync(
        IFormFile tokenFile);

        Task<bool> DeleteAsync(Guid id);
    }
}