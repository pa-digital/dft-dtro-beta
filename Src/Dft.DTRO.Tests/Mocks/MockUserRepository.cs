namespace Dft.DTRO.Tests.Mocks;

[ExcludeFromCodeCoverage]
public static class MockUserRepository
{
    public static Mock<IDtroUserDal> Setup()
    {
        var repository = new Mock<IDtroUserDal>();
        repository.Setup(it => it.GetAllDtroUsersAsync().Result)
            .Returns(() => MockTestObjects.UserResponses);


        return repository;
    }
}