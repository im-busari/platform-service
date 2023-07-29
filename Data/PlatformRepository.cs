using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context; 
    
    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // This is being called after any update/create/delete operation to make sure that our data is flushed out to our database
    public bool SaveChanges() => _context.SaveChanges() >= 0;

    public IEnumerable<Platform> GetPlatforms() => _context.Platforms.ToList();

    public Platform GetPlatformById(int id) => _context.Platforms.FirstOrDefault(platform => platform.Id == id);

    public void CreatePlatform(Platform platform)
    {
        if (platform == null) throw new ArgumentNullException(nameof(platform));

        _context.Platforms.Add(platform);
    }
}