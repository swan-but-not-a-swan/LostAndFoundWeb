using Azure.Storage.Blobs.Models;
using LostAndFoundWeb.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Components.Pages;
public partial class Makelostitem
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; }
    [SupplyParameterFromForm]
    private Input LostItem { get; set; } = new();
    private LostItem lostItem { get; set; } = new();
    private List<FoundItem> ShowItems { get; set; }
    private EditContext _EditContext { get; set; }
    private ValidationMessageStore MessageStore { get; set; }
    private const int FILESCOUNT = 5;
    private bool MaxFilesExceeded = false;
    private const long MAXFILESIZE = 1024 * 1024 * 3;
    private string error;
    private IReadOnlyList<IBrowserFile>? files;
    private bool SubmitButtonClicked = false;

    protected override void OnInitialized()
    {
        _EditContext = new EditContext(LostItem);
        MessageStore = new(_EditContext);
        _EditContext.OnValidationRequested += HandleValidationRequest;
    }
    private void HandleValidationRequest(object? sender, ValidationRequestedEventArgs e)
    {
        MessageStore?.Clear();
        if (MaxFilesExceeded)
        {
            MessageStore?.Add(() => LostItem.Urls, "The maximum files allowed is 5.");
            _EditContext?.NotifyValidationStateChanged();
        }
        if (files.Count > 0 && files is not null)
        {
            foreach (var file in files)
            {
                if (file.Size > MAXFILESIZE)
                {
                    MessageStore?.Add(() => LostItem.Urls, "The maximum file size is 3 MB");
                    _EditContext?.NotifyValidationStateChanged();
                }
            }
        }
    }
    private async Task Submit()
    {
        lostItem = new() { Name = LostItem.Name, WordOne = LostItem.WordOne, WordTwo = LostItem.WordTwo, WordThree = LostItem.WordThree, Location = LostItem.Location };
        List<string> words = new List<string>() { LostItem.WordOne, LostItem.WordTwo, LostItem.WordThree };

        SubmitButtonClicked = true;
        ShowItems = db.FoundItems.Where(l =>l.OwnerFound == false).Select(l => new
        {
            item = l,
            MatchEvaluation = (words.Any(x => x == l.WordOne) ? 1 : 0) + (words.Any(x => x == l.WordTwo) ? 1 : 0) + (words.Any(x => x == l.WordThree) ? 1 : 0),
        })
            .Where(x => x.MatchEvaluation > 0)
            .OrderByDescending(x => x.MatchEvaluation)
            .Select(l => l.item).ToList();
    }
    private async Task PostLostItem()
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
            LostItem.Urls.Add(blob.Uri.ToString());
        }
        lostItem.LostUser = user;
        lostItem.Urls = LostItem.Urls;
        user.LostItems.Add(lostItem);
        await db.LostItems.AddAsync(lostItem);
        await db.SaveChangesAsync();
        NavigationManager.NavigateTo("/lostlist");
    }
    private void LoadFiles(InputFileChangeEventArgs e)
    {
        if (e.FileCount > FILESCOUNT && e.FileCount < 1)
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