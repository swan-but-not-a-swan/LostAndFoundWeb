using LostAndFoundWeb.Data;
using LostAndFoundWeb.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LostAndFoundWeb.Components.Pages;
public partial class LostItemView
{
    [SupplyParameterFromForm]
    private LostItem LostItem { get; set; } = new();
    private string PickUpLocation { get; set; }
    protected override async Task OnInitializedAsync()
    {
        StateContainer.LostEvent += StateHasChanged;
        LostItem = StateContainer.LostItem;
    }
    private async Task Found()
    {
        var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = await UserManager.GetUserAsync(authstate.User);
        LostItem.HasBeenFound = true;
        FoundItem foundItem = new FoundItem()
        {
            Name = LostItem.Name,
            WordOne = LostItem.WordOne,
            WordTwo = LostItem.WordTwo,
            WordThree = LostItem.WordThree,
            Location = LostItem.Location,
            OwnerFound = true,
            Urls = LostItem.Urls
        };
        user.FoundItems.Add(foundItem);
        await db.FoundItems.AddAsync(foundItem);
        //TODO: Send email
        await EmailService.SendEmailAsync($"Your lost item has been found by {user.UserName}. Please collect at {PickUpLocation}.", LostItem.LostUser.Email, "Lost Item found");
        await db.SaveChangesAsync();
        NavigationManager.NavigateTo("/lostlist");

    }
}