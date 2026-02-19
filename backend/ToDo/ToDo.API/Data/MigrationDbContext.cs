using Microsoft.EntityFrameworkCore;

namespace ToDo.API.Data;

public class MigrationDbContext(DbContextOptions<MigrationDbContext> options) 
    : DbContext(options)
{
    
}