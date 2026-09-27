using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using training_project_backend.Data;
using training_project_backend.DTOs;
using training_project_backend.Entities;
using training_project_backend.Requests;

namespace training_project_backend.Providers
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtDbContext _db;

        public JwtProvider(JwtDbContext db)
        {
            _db = db;
        }


        public async Task<ValidateJwtResponse>
    ValidateAsync(
        IFormFile tokenFile)
        {
            using var reader =
                new StreamReader(
                    tokenFile.OpenReadStream());

            var token =
                await reader.ReadToEndAsync();

            JwtSecurityToken jwt;

            try
            {
                var handler =
                    new JwtSecurityTokenHandler();

                jwt =
                    handler.ReadJwtToken(token);
            }
            catch
            {
                return new ValidateJwtResponse
                {
                    IsValid = false,
                    Message =
                        "File does not contain a valid JWT."
                };
            }

            if (jwt.ValidTo < DateTime.UtcNow)
            {
                return new ValidateJwtResponse
                {
                    IsValid = false,
                    Message =
                        "JWT has expired."
                };
            }

            return new ValidateJwtResponse
            {
                IsValid = true,
                Message = "JWT is valid."
            };
        }


        public async Task<List<JwtSummaryResponse>> GetAllAsync()
        {
            return await _db.JwtTokens.Select(t => new JwtSummaryResponse
            {
                Id = t.Id,
                TokenName = t.TokenName,
                Issuer = t.Issuer,
                Subject = t.Subject,
                IssuedAt = t.IssuedAt,
                ExpiresAt = t.ExpiresAt,
                Status = t.Status
            }).ToListAsync();
        }

        public async Task<JwtDetailsResponse?> GetByIdAsync(Guid id)
        {
            var token = await _db.JwtTokens.FirstOrDefaultAsync(t => t.Id == id);
            if (token == null) {
                return null;
            }

            return new JwtDetailsResponse
            {
                Id = token.Id,
                FileName = token.FileName,
                TokenName = token.TokenName,
                FileSize = token.FileSize,
                UploadedAt = token.UploadedAt,
                Issuer = token.Issuer,
                Subject = token.Subject,
                Audience = token.Audience,
                TokenId = token.TokenId,
                IssuedAt = token.IssuedAt,
                ValidFrom = token.ValidFrom,
                ExpiresAt = token.ExpiresAt,
                RemainingTime = token.RemainingTime,
                Status = token.Status,
                HeaderJson = token.HeaderJson,
                PayloadJson = token.PayloadJson
            };
        }

        public async Task<UploadJwtResponse> UploadAsync(
UploadJwtRequest request)

        {
            var tokenFile =
request.TokenFile;
            using var reader = new StreamReader(tokenFile.OpenReadStream());
            var token = await reader.ReadToEndAsync();

            JwtSecurityToken jwt;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                jwt = handler.ReadJwtToken(token);
            }
            catch
            {
                return new UploadJwtResponse
                {
                    Success = false,
                    Message = "File does not contain a valid JWT."
                };
            }

            if (jwt.ValidTo < DateTime.UtcNow)
            {
                return new UploadJwtResponse
                {
                    Success = false,
                    Message = "JWT has expired and cannot be uploaded."
                };
            }

            string? GetClaim(JwtSecurityToken jwt, string claimType) {
                return jwt.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }

            var issuer = jwt.Issuer;
            var subject = GetClaim(jwt, "sub");
            var audience = jwt.Audiences.FirstOrDefault();
            var tokenId = GetClaim(jwt, "jti");
            var iatClaim = GetClaim(jwt, "iat");

            DateTime? issuedAt = null;

            if (long.TryParse (iatClaim, out long iat))
            {
                issuedAt = DateTimeOffset.FromUnixTimeSeconds(iat).UtcDateTime;
            }

            string remainingTime = "N/A";

            if (jwt.ValidTo > DateTime.UtcNow)
            {
                var remaining = jwt.ValidTo-DateTime.UtcNow;
                remainingTime = remaining.ToString(@"dd\.hh\:mm\:ss");
            }

            var status = jwt.ValidTo < DateTime.UtcNow? "Expired": "Valid";
            var entity = new JwtEntity
                {
                    Id = Guid.NewGuid(),
                TokenName = request.TokenName,
                FileName = tokenFile.FileName,
                    FileSize = tokenFile.Length,
                    UploadedAt = DateTime.UtcNow,

                    Issuer = string.IsNullOrWhiteSpace(issuer)? "N/A": issuer,

                    Subject = string.IsNullOrWhiteSpace(subject)? "N/A": subject,

                    Audience =
                        string.IsNullOrWhiteSpace(
                            audience)
                            ? "N/A"
                            : audience,

                    TokenId =
                        string.IsNullOrWhiteSpace(
                            tokenId)
                            ? "N/A"
                            : tokenId,

                    IssuedAt =
                        issuedAt,

                    ValidFrom =
                        jwt.ValidFrom,

                    ExpiresAt =
                        jwt.ValidTo,

                    RemainingTime =
                        remainingTime,

                    Status =
                        status,

                    HeaderJson =
                        System.Text.Json
                            .JsonSerializer
                            .Serialize(
                                jwt.Header),

                    PayloadJson =
                        System.Text.Json
                            .JsonSerializer
                            .Serialize(
                                jwt.Payload)
                };

            _db.JwtTokens.Add(entity);

            await _db.SaveChangesAsync();

            return new UploadJwtResponse
            {
                Success = true,

                Message =
                    "Token uploaded successfully.",

                Token =
                    await GetByIdAsync(
                        entity.Id)
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.JwtTokens.FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) {
                return false;
            }

            _db.JwtTokens.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
