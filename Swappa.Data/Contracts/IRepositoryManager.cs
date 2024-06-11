namespace Swappa.Data.Contracts
{
    public interface IRepositoryManager
    {
        ITokenRepository Token { get; }
        IUserRepository User { get; }
        IUserFeedbackRepository Feedback { get; }
    }
}
