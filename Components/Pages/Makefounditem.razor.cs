using Azure.Storage.Blobs.Models;
using LostAndFoundWeb.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Components.Pages;
public partial class Makefounditem
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; }
    [SupplyParameterFromForm]
    private Input FoundItem { get; set; } = new();
    private FoundItem foundItem { get; set; }
    private EditContext _EditContext { get; set; }
    private List<LostItem> ShowItems { get; set; }
    private ValidationMessageStore MessageStore { get; set; }
    private const int FILESCOUNT = 5;
    private bool MaxFilesExceeded = false;
    private const long MAXFILESIZE = 1024 * 1024 * 3;
    private string error;
    private bool SubmitButtonClicked = false;
    private IReadOnlyList<IBrowserFile>? files;
    
    protected override void OnInitialized()
    {
        _EditContext = new EditContext(FoundItem);
        MessageStore = new(_EditContext);
        _EditContext.OnValidationRequested += HandleValidationRequest;
    }
    private void HandleValidationRequest(object? sender, ValidationRequestedEventArgs e)
    {
        MessageStore?.Clear();
        if (MaxFilesExceeded)
        {
            MessageStore?.Add(() => FoundItem.Urls, "The maximum files allowed is 5.");
            _EditContext?.NotifyValidationStateChanged();
        }
        if (files.Count > 0 && files is not null )
        {
            foreach (var file in files)
            {
                if (file.Size > MAXFILESIZE)
                {
                    MessageStore?.Add(() => FoundItem.Urls, "The maximum file size is 3 MB");
                    _EditContext?.NotifyValidationStateChanged();
                }
            }
        }
    }
    private async Task Submit()
    {
        foundItem = new() { Name = FoundItem.Name, WordOne = FoundItem.WordOne, WordTwo = FoundItem.WordTwo, WordThree = FoundItem.WordThree, Location = FoundItem.Location };
        List<string> words = new List<string>() { FoundItem.WordOne, FoundItem.WordTwo, FoundItem.WordThree };
        SubmitButtonClicked = true;
        ShowItems = db.LostItems.ToList();
        ShowItems = db.LostItems.Where(l => l.HasBeenFound == false).Select(l => new
        {
            item = l,
            MatchEvaluation = (words.Any(x => x.ToLower() == l.WordOne.ToLower()) ? 1 : 0) + (words.Any(x => x.ToLower() == l.WordTwo.ToLower()) ? 1 : 0) + (words.Any(x => x.ToLower() == l.WordThree.ToLower()) ? 1 : 0),
        })
            .Where(x => x.MatchEvaluation > 0)
            .OrderByDescending(x => x.MatchEvaluation)
            .Select(l => l.item).ToList();
    }
    private async Task PostFoundItem()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = await UserManager.GetUserAsync(authState.User);
        var username = await UserManager.GetUserNameAsync(user);
        var containerClient = blob.GetBlobContainerClient(username.ToLower());
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        foreach (var file in files)
        {
            string blobname = $"{Guid.NewGuid().ToString()}.jpg";
            await containerClient.UploadBlobAsync(blobname, file.OpenReadStream(maxAllowedSize: MAXFILESIZE));
            var blob = containerClient.GetBlobClient(blobname);
            FoundItem.Urls.Add(blob.Uri.ToString());
        }
        foundItem.Finder = user;
        foundItem.Urls = FoundItem.Urls;
        user.FoundItems.Add(foundItem);
        await db.FoundItems.AddAsync(foundItem);
        await db.SaveChangesAsync();
        NavigationManager.NavigateTo("/foundlist");
    }
    private void LoadFiles(InputFileChangeEventArgs e)
    {
        if (e.FileCount > FILESCOUNT && e is not null)
        {
            MaxFilesExceeded = true;
            return;
        }
        else
        {
            files = e.GetMultipleFiles(FILESCOUNT);
        }
    }
    private class Input
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string WordOne { get; set; }
        [Required]
        [MaxLength(20)]
        public string WordTwo { get; set; }
        [Required]
        [MaxLength(20)]
        public string WordThree { get; set; }
        public List<string> Urls { get; set; } = new();
        [Required]
        [MaxLength(100)]
        public string Location { get; set; }
    }
}