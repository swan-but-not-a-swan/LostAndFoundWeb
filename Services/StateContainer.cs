using LostAndFoundWeb.Data;

namespace LostAndFoundWeb.Services
{
    public class StateContainer
    {
        public event Action? LostEvent;
        public event Action? FoundEvent;
        public event Action? UrlEvent;

        public LostItem LostItem { get; set; }
        public FoundItem FoundItem { get; set; }
        public string ProfilePictureUrl { get; set; }

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
        public void Url(string url)
        {
            ProfilePictureUrl = url;
            UrlEvent?.Invoke();
        }
    }
}
