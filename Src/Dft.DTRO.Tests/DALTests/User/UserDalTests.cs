namespace DfT.DTRO.Tests.DALTests
{
    [ExcludeFromCodeCoverage]
    public class UserDalTests : IDisposable
    {
        private readonly DtroContext _context;
        private readonly UserDal _userDal;

        public UserDalTests()
        {
            var options = new DbContextOptionsBuilder<DtroContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DtroContext(options);
            SeedDatabase().GetAwaiter().GetResult();
            _userDal = new UserDal(_context);
        }

        private async Task SeedDatabase()
        {
            var user1 = new User { Id = Guid.NewGuid(), Email = "user@test.com" };
            var user2 = new User { Id = Guid.NewGuid(), Email = "anotheruser@test.com" };

            var dtroUser1 = new DtroUser { Id = user1.Id, Name = "Some User" };
            var dtroUser2 = new DtroUser { Id = user2.Id, Name = "Another User" };

            _context.Users.AddRange(user1, user2);
            _context.DtroUsers.AddRange(dtroUser1, dtroUser2);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteUserDeletesUserAndDtroUserWhenUserAndDtroUserExist()
        {
            var userId = _context.Users.First().Id;

            await _userDal.DeleteUser(userId);
            var user = await _context.Users.FindAsync(userId);
            var dtroUser = await _context.DtroUsers.FindAsync(userId);

            Assert.Null(user);
            Assert.Null(dtroUser);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
