namespace Dft.DTRO.Tests.Mocks;

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