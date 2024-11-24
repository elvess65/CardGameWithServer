namespace CardGame.Abstract
{
    public interface IUpdatable
    {
        bool IsActive { get; }
        void Update(float deltaTime);
    }
}
