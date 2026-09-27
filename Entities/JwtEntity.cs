using Azure.Core;

namespace training_project_backend.Entities;

public class JwtEntity
{
    public Guid Id { get; set; }

    public string TokenName { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime UploadedAt { get; set; }

    public string Issuer { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string TokenId { get; set; } = string.Empty;

    public DateTime? IssuedAt { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string RemainingTime { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string HeaderJson { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = string.Empty;

  
}
