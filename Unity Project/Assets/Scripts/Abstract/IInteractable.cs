namespace CardGame.Abstract
{
    public interface IInteractable
    {
        bool IsInteractable { get; set; }
        
        void Interact();
    }
}
