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
    private EditContext _EditContext { get; set; }
    private ValidationMessageStore MessageStore { get; set; }
    private const int FILESCOUNT = 5;
    private bool MaxFilesExceeded = false;
    private const long MAXFILESIZE = 1024 * 1024 * 3;
    private string error;
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
        foreach (var file in files)
        {
            if (file.Size > MAXFILESIZE)
            {
                MessageStore?.Add(() => FoundItem.Urls, "The maximum file size is 3 MB");
                _EditContext?.NotifyValidationStateChanged();
            }
        }
    }
    private async Task Submit()
    {
        FoundItem foundItem = new() { Name = FoundItem.Name, WordOne = FoundItem.WordOne, WordTwo = FoundItem.WordTwo, WordThree = FoundItem.WordThree, Location = FoundItem.Location };
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = await UserManager.GetUserAsync(authState.User);
        var username = await UserManager.GetUserNameAsync(user);
        var containerClient = blob.GetBlobContainerClient(username.ToLower());
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        foreach (var file in files)
        {
            string blobname = $"{Guid.NewGuid().ToString()}.jpg";
            await containerClient.UploadBlobAsync(blobname, file.OpenReadStream(maxAllowedSize:MAXFILESIZE));
            var blob = containerClient.GetBlobClient(blobname);
            FoundItem.Urls.Add(blob.Uri.ToString());
        }
        foundItem.Finder = user;
        foundItem.Urls = FoundItem.Urls;
        user.FoundItems.Add(foundItem);
        await db.FoundItems.AddAsync(foundItem);
        await db.SaveChangesAsync();
    }
    private void LoadFiles(InputFileChangeEventArgs e)
    {
        if (e.FileCount > FILESCOUNT)
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