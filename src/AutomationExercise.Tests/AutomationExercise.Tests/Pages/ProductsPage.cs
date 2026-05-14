namespace AutomationExercise.Tests.Pages;

class ProductsPage : BasePage
{
    public ProductsPage(IPage page) : base(page) { }

    private ILocator SearchInput => _page.Locator("#search_product");
    private ILocator SearchButton => _page.Locator("#submit_search");
    private ILocator SearchedProductsHeader => _page.Locator("h2:has-text('Searched Products')");
    private ILocator ProductNames => _page.Locator(".productinfo p");

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://automationexercise.com/products");

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

    public async Task SearchForProductAsync(string productName)
    {
        await SearchInput.FillAsync(productName);
        await SearchButton.ClickAsync();
    }

    public async Task<bool> IsSearchedProductsHeaderVisibleAsync()
    {
        await SearchedProductsHeader.WaitForAsync();
        return await SearchedProductsHeader.IsVisibleAsync();
    }

    public async Task<IReadOnlyList<string>> GetVisibleProductNamesAsync()
    {
        return await ProductNames.AllTextContentsAsync();
    }
}
