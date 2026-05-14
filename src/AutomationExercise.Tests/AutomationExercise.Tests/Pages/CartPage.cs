namespace AutomationExercise.Tests.Pages;

class CartPage : BasePage
{
    public CartPage(IPage page) : base(page) { }

    // Locators
    private ILocator CartRows => _page.Locator("tbody tr");

    // Actions
    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://automationexercise.com/view_cart");
    }

    public async Task<int> GetNumberOfItemsInCartAsync()
    {
        await _page.Locator("tbody").WaitForAsync();
        return await CartRows.CountAsync();
    }

    public async Task<int> GetFirstItemQuantityAsync()
    {
        var quantityLocator = _page.Locator(".cart_quantity button").First;
        string quantityText = await quantityLocator.InnerTextAsync();

        return int.Parse(quantityText);
    }

    public async Task<int> GetFirstItemUnitPriceAsync()
    {
        var priceLocator = _page.Locator(".cart_price p").First;
        string priceText = await priceLocator.InnerTextAsync();
        
        string cleanPrice = priceText.Replace("Rs. ", "").Trim();
        return int.Parse(cleanPrice);
    }

    public async Task<int> GetFirstItemTotalPriceAsync()
    {
        var totalLocator = _page.Locator(".cart_total p").First;
        string totalText = await totalLocator.InnerTextAsync();

        string cleanTotal = totalText.Replace("Rs. ", "").Trim();
        return int.Parse(cleanTotal);
    }
}
