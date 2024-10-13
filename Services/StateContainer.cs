using LostAndFoundWeb.Data;

namespace LostAndFoundWeb.Services
{
    public class StateContainer
    {
        public event Action? LostEvent;
        public event Action? FoundEvent;
        public LostItem LostItem { get; set; }
        public FoundItem FoundItem { get; set; }

        public void LostItemFound(LostItem lostitem)
        {
            LostItem = lostitem;
            LostEvent?.Invoke();
        }
        public void FoundItemLost(FoundItem founditem)
        {
            FoundItem = founditem;
            FoundEvent?.Invoke();
        }
    }
}
