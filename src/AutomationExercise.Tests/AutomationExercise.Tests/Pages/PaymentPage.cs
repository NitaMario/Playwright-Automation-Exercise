namespace AutomationExercise.Tests.Pages;

class PaymentPage : BasePage
{
    public PaymentPage(IPage page) : base(page) { }

    // Locators
    private ILocator NameOnCardInput => _page.Locator("[data-qa='name-on-card']");
    private ILocator CardNumberInput => _page.Locator("[data-qa='card-number']");
    private ILocator CvcInput => _page.Locator("[data-qa='cvc']");
    private ILocator ExpiryMonthInput => _page.Locator("[data-qa='expiry-month']");
    private ILocator ExpiryYearInput => _page.Locator("[data-qa='expiry-year']");
    private ILocator PayAndConfirmButton => _page.Locator("[data-qa='pay-button']");
    private ILocator OrderPlacedSuccessMessage => _page.Locator("[data-qa='order-placed'] > b");

    // Actions
    public async Task FillPaymentDetailsAndSubmitAsync(string name, string cardNumber, string cvc, string month, string year)
    {
        await NameOnCardInput.FillAsync(name);
        await CardNumberInput.FillAsync(cardNumber);
        await CvcInput.FillAsync(cvc);
        await ExpiryMonthInput.FillAsync(month);
        await ExpiryYearInput.FillAsync(year);
        await PayAndConfirmButton.ClickAsync();
    }

    public async Task<bool> IsOrderPlacedSuccessfullyAsync()
    {
        await OrderPlacedSuccessMessage.WaitForAsync();
        return await OrderPlacedSuccessMessage.IsVisibleAsync();
    }
}
