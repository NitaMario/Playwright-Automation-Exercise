namespace AutomationExercise.Tests.Pages;

public class ContactUsPage : BasePage
{
    public ContactUsPage(IPage page) : base(page) { }

    // Locators
    private ILocator NameInput => _page.Locator("[data-qa='name']");
    private ILocator EmailInput => _page.Locator("[data-qa='email']");
    private ILocator SubjectInput => _page.Locator("[data-qa='subject']");
    private ILocator MessageInput => _page.Locator("[data-qa='message']");
    private ILocator UploadFileInput => _page.Locator("input[name='upload_file']");
    private ILocator SubmitButton => _page.Locator("[data-qa='submit-button']");
    private ILocator SuccessMessage => _page.Locator(".status.alert.alert-success");

    // Actions
    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://automationexercise.com/contact_us");

        var consentButton = _page.Locator(".fc-cta-consent");
        try
        {
            await consentButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
            await consentButton.ClickAsync();
        }
        catch
        {
        }
    }

    public async Task FillContactFormAsync(string name, string email, string subject, string message)
    {
        await NameInput.FillAsync(name);
        await EmailInput.FillAsync(email);
        await SubjectInput.FillAsync(subject);
        await MessageInput.FillAsync(message);
    }

    public async Task UploadFileAsync(string filePath)
    {
        await UploadFileInput.SetInputFilesAsync(filePath);
    }

    public async Task SubmitFormAndAcceptAlertAsync()
    {
        _page.Dialog += async(_, dialog) => await dialog.AcceptAsync();
        await SubmitButton.ClickAsync();
    }

    public async Task<bool> IsSuccessMessageVisibleAsync()
    {
        await SuccessMessage.WaitForAsync();
        return await SuccessMessage.IsVisibleAsync();
    }
}
