using Microsoft.EntityFrameworkCore;
using training_project_backend.Entities;

namespace training_project_backend.Data;

public class JwtDbContext : DbContext
{
    public JwtDbContext(
        DbContextOptions<JwtDbContext> options)
        : base(options)
    {
    }

    public DbSet<JwtEntity> JwtTokens
        => Set<JwtEntity>();
}