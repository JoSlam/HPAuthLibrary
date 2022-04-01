using HealthPass.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HealthPass.Auth.Tests
{
    public class MockDBContext: HealthPassContext
    {
        private List<User> Users = new List<User>();
        private List<LoginDetails> LoginDetails = new List<LoginDetails>();
        private readonly TableSeedHandler SeedHandler = new TableSeedHandler();

        public HealthPassContext BuildMockContext()
        {
            // Users DBSet setup
            var mockUsersSet = CreateDbSetMock(Users);
            mockUsersSet.Setup(w => w.Add(It.IsAny<User>())).Callback((User user) =>
            {
                Users.Add(user);
                user.ID = SeedHandler.GetNewID(user.GetType().Name);
            });
            mockUsersSet.Setup(m => m.Remove(It.IsAny<User>())).Callback((User user) => Users.Remove(user));


            // LoginDetails DBSet setup
            var mockLoginDetailsSet = CreateDbSetMock(LoginDetails);
            mockLoginDetailsSet.Setup(w => w.Add(It.IsAny<LoginDetails>())).Callback((LoginDetails details) =>
            {
                LoginDetails.Add(details);
                details.ID = SeedHandler.GetNewID(details.GetType().Name);
            });
            mockLoginDetailsSet.Setup(m => m.Remove(It.IsAny<LoginDetails>())).Callback((LoginDetails user) => LoginDetails.Remove(user));
            
            Mock<HealthPassContext> mockDbContext = new Mock<HealthPassContext>();
            mockDbContext.Setup(i => i.Users).Returns(mockUsersSet.Object);
            mockDbContext.Setup(i => i.LoginDetails).Returns(mockLoginDetailsSet.Object);

            return mockDbContext.Object;
        }


        private Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            // Add Range
            dbSetMock.Setup(w => w.AddRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> items) => {
                items.ToList().ForEach(i => dbSetMock.Object.Add(i));
            });

            // Remove Range
            dbSetMock.Setup(w => w.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback((IEnumerable<T> items) => {
                items.ToList().ForEach(i => dbSetMock.Object.Remove(i));
            });

            return dbSetMock;
        }
    }
}
