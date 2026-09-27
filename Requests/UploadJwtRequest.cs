using Microsoft.AspNetCore.Http;

namespace training_project_backend.Requests
{
    public class UploadJwtRequest
    {
        public string TokenName { get; set; } = string.Empty;

        public IFormFile TokenFile { get; set; } = null!;
    }
}