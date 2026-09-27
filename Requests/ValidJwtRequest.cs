namespace training_project_backend.Requests;

public class ValidateJwtRequest
{
    public IFormFile TokenFile { get; set; } = null!;
}