using LostAndFoundWeb.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LostAndFoundWeb.Components.Layout;
public partial class NavMenu
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;
    private ApplicationUser user = default!;
    private string username = "";
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            username = user.Identity.Name;
        }
    }
}