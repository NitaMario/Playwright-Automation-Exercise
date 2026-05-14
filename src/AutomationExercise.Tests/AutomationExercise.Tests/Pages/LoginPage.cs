namespace AutomationExercise.Tests.Pages;

public class LoginPage : BasePage
{
    private ILocator LoginEmailInput => _page.Locator("[data-qa='login-email']");
    private ILocator LoginPasswordInput => _page.Locator("[data-qa='login-password']");
    private ILocator LoginButton => _page.Locator("[data-qa='login-button']");
    private ILocator LoggedInUserText => _page.Locator("text=Logged in as");
    private ILocator LogoutButton => _page.Locator("a[href='/logout']");
    private ILocator SignupNameInput => _page.Locator("[data-qa='signup-name']");
    private ILocator SignupEmailInput => _page.Locator("[data-qa='signup-email']");
    private ILocator SignupButton => _page.Locator("[data-qa='signup-button']");

    public LoginPage(IPage page) : base(page)
    {
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://automationexercise.com/login");

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

    public async Task LoginUserAsync(string email, string password)
    {
        await LoginEmailInput.FillAsync(email);
        await LoginPasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task<bool> IsUserLoggedInAsync()
    {
        await LoggedInUserText.WaitForAsync();
        return await LoggedInUserText.IsVisibleAsync();
    }

    public async Task LogoutUserAsync()
    {
        await LogoutButton.ClickAsync();
    }

    public async Task StartSignupAsync(string name, string email)
    {
        await SignupNameInput.FillAsync(name);
        await SignupEmailInput.FillAsync(email);
        await SignupButton.ClickAsync();
    }
}
