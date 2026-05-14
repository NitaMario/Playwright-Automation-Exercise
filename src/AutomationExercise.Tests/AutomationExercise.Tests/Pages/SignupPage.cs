namespace AutomationExercise.Tests.Pages;

public class SignupPage : BasePage
{
    public SignupPage(IPage page) : base(page) { }

    private ILocator TitleMrRadio => _page.Locator("#id_gender1");
    private ILocator PasswordInput => _page.Locator("[data-qa='password']");
    private ILocator DaysSelect => _page.Locator("[data-qa='days']");
    private ILocator MonthsSelect => _page.Locator("[data-qa='months']");
    private ILocator YearsSelect => _page.Locator("[data-qa='years']");
    private ILocator FirstNameInput => _page.Locator("[data-qa='first_name']");
    private ILocator LastNameInput => _page.Locator("[data-qa='last_name']");
    private ILocator AddressInput => _page.Locator("[data-qa='address']");
    private ILocator CountrySelect => _page.Locator("[data-qa='country']");
    private ILocator StateInput => _page.Locator("[data-qa='state']");
    private ILocator CityInput => _page.Locator("[data-qa='city']");
    private ILocator ZipcodeInput => _page.Locator("[data-qa='zipcode']");
    private ILocator MobileNumberInput => _page.Locator("[data-qa='mobile_number']");
    private ILocator CreateAccountButton => _page.Locator("[data-qa='create-account']");
    private ILocator AccountCreatedText => _page.Locator("[data-qa='account-created']");
    private ILocator ContinueButton => _page.Locator("[data-qa='continue-button']");
    private ILocator DeleteAccountLink => _page.Locator("a[href='/delete_account']");
    private ILocator AccountDeletedText => _page.Locator("[data-qa='account-deleted']");

    public async Task FillAccountInfoAsync(string password, string firstName, string lastName, string address, 
        string state, string city, string zipcode, string mobileNumber)
    {
        await TitleMrRadio.ClickAsync();
        await PasswordInput.FillAsync(password);
        await DaysSelect.SelectOptionAsync(new[] { "1" });
        await MonthsSelect.SelectOptionAsync(new[] { "1" });
        await YearsSelect.SelectOptionAsync(new[] { "2000" });
        await FirstNameInput.FillAsync(firstName);
        await LastNameInput.FillAsync(lastName);
        await AddressInput.FillAsync(address);
        await CountrySelect.SelectOptionAsync(new[] { "United States" });
        await StateInput.FillAsync(state);
        await CityInput.FillAsync(city);
        await ZipcodeInput.FillAsync(zipcode);
        await MobileNumberInput.FillAsync(mobileNumber);

        await CreateAccountButton.ClickAsync();
    }

    public async Task<bool> IsAccountCreatedAsync()
    {
        await AccountCreatedText.WaitForAsync();
        return await AccountCreatedText.IsVisibleAsync();
    }

    public async Task ClickContinueAsync()
    {
        await ContinueButton.ClickAsync();
    }

    public async Task DeleteAccountAsync()
    {
        await DeleteAccountLink.ClickAsync();
        await AccountDeletedText.WaitForAsync();
        await ContinueButton.ClickAsync();
    }
}
