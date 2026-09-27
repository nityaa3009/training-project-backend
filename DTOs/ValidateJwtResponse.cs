namespace training_project_backend.DTOs;

public class ValidateJwtResponse
{
    public bool IsValid { get; set; }

    public string Message { get; set; } = string.Empty;
}