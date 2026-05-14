namespace AutomationExercise.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
class CartTests : PageTest
{
    [Test]
    public async Task AddProductsToCart_ShouldPersistDataAndDisplayCorrectQuantity()
    {
        // Arrange
        var productsPage = new ProductsPage(Page);
        var cartPage = new CartPage(Page);

        // Act 1
        await productsPage.GotoAsync();
        await productsPage.AddProductToCartAsync(1);
        await productsPage.AddProductToCartAsync(2);

        // Act 2
        await cartPage.GotoAsync();
        int cartItemCount = await cartPage.GetNumberOfItemsInCartAsync();

        // Assert
        Assert.That(cartItemCount, Is.EqualTo(2), "Cart state was dropped or synchronized incorrectly. Expected 2 items.");
    }

    [Test]
    public async Task AddProduct_WithCustomQuantity_ShouldDisplayExactQuantity_AndCalculateTotalCorrectly()
    {
        // Arrange
        var detailsPage = new ProductDetailsPage(Page);
        var cartPage = new CartPage(Page);
        int targetQuantity = 4;
        int productId = 1;

        // Act 1
        await detailsPage.GotoAsync(productId);
        await detailsPage.SetQuantityAndAddToCartAsync(targetQuantity);

        // Act 2
        int actualQuantity = await cartPage.GetFirstItemQuantityAsync();
        int unitPrice = await cartPage.GetFirstItemUnitPriceAsync();
        int displayedTotal = await cartPage.GetFirstItemTotalPriceAsync();
        int expectedTotal = unitPrice * targetQuantity;

        // Assert 1
        Assert.That(actualQuantity, Is.EqualTo(targetQuantity),
            $"Expected {targetQuantity}, but found {actualQuantity}.");

        // Assert 2
        Assert.That(displayedTotal, Is.EqualTo(expectedTotal),
            $"Math failure. Unit price: {unitPrice} x Quantity: {targetQuantity} should equal {expectedTotal}, but UI showed {displayedTotal}.");
    }
}
