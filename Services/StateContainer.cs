using LostAndFoundWeb.Data;

namespace LostAndFoundWeb.Services
{
    public class StateContainer
    {
        public event Action? MadeEvent;
        public LostItem LostItem { get; set; }
        public void LostItemFound(LostItem lostitem)
        {
            LostItem = lostitem;
            MadeEvent?.Invoke();
        }
    }
}
