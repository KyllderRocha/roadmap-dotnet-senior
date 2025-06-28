
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories;

public class UserRepositoryTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
    

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenCalled()
    {
        await using var context = GetInMemoryDbContext();
        
        var userRepository = new UserRepository(context);
        var newUser = new User("Teste User", "teste@email.com");

        await userRepository.AddAsync(newUser);
        await context.SaveChangesAsync(); 
        var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Id == newUser.Id);
        
        Assert.NotNull(userInDb);
        Assert.Equal("Teste User", userInDb.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        await using var context = GetInMemoryDbContext();

        var userId = Guid.NewGuid();
        var newUser = new User(userId, "User Exists", "exists@email.com");
        
        context.Users.Add(newUser);
        await context.SaveChangesAsync();

        var userRepository = new UserRepository(context);

        var result = await userRepository.GetByIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }
}