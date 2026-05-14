namespace AutomationExercise.Tests.Pages;

class CheckoutPage : BasePage
{
    public CheckoutPage(IPage page) : base(page) { }

    // Locators
    private ILocator DeliveryFirstName => _page.Locator("#address_delivery .address_firstname");
    private ILocator CommentInput => _page.Locator("textarea[name='message']");
    private ILocator PlaceOrderButton => _page.Locator("a[href='/payment']");

    // Actions
    public async Task<string> GetDeliveryNameAsync()
    {
        return await DeliveryFirstName.InnerTextAsync();
    }

    public async Task PlaceOrderAsync(string comment)
    {
        await CommentInput.FillAsync(comment);
        await PlaceOrderButton.ClickAsync();
    }
}
