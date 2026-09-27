namespace training_project_backend.DTOs;

public class JwtSummaryResponse
{
    public Guid Id { get; set; }

    public string TokenName { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public DateTime? IssuedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string Status { get; set; } = string.Empty;
}