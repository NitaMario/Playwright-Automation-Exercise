namespace AutomationExercise.Tests.Pages;

class ProductDetailsPage : BasePage
{
    public ProductDetailsPage(IPage page) : base(page) { }

    // Locators
    private ILocator QuantityInput => _page.Locator("#quantity");
    private ILocator AddToCartButton => _page.Locator("button:has-text('Add to cart')");
    private ILocator ViewCartLink => _page.Locator("u:has-text('View Cart')");

    // Actions
    public async Task GotoAsync(int productId)
    {
        await _page.GotoAsync($"https://automationexercise.com/product_details/{productId}");

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

    public async Task SetQuantityAndAddToCartAsync(int quantity)
    {
        await QuantityInput.FillAsync(quantity.ToString());
        await AddToCartButton.ClickAsync();

        await ViewCartLink.WaitForAsync();
        await ViewCartLink.ClickAsync();
    }
}
