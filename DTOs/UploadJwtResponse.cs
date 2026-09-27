namespace training_project_backend.DTOs
{
    public class UploadJwtResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;
        public JwtDetailsResponse? Token { get; set; }
    }
}