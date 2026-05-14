namespace AutomationExercise.Tests.Tests;

class ProductsTests : PageTest
{
    [Test]
    public async Task SearchProduct_WithValidKeyword_ShouldRenderCorrectProductsGrid()
    {
        // Arrange
        var productsPage = new ProductsPage(Page);
        string searchKeyword = "Shirt";

        // Act 1
        await productsPage.GotoAsync();

        // Act 2
        await productsPage.SearchForProductAsync(searchKeyword);

        // Assert 1
        bool isHeaderVisible = await productsPage.IsSearchedProductsHeaderVisibleAsync();
        Assert.That(isHeaderVisible, Is.True, "The 'Searched Products' header did not appear. The DOM failed to re-render.");

        // Assert 2
        var displayedProducts = await productsPage.GetVisibleProductNamesAsync();
        Assert.That(displayedProducts.Count, Is.GreaterThan(0), "No products were returned from the search query.");

        // Assert 3
        bool hasRelevantMatch = displayedProducts.Any(p => p.ToLower().Contains(searchKeyword.ToLower()));
        Assert.That(hasRelevantMatch, Is.True, 
            $"Search returned {displayedProducts.Count} items, but none contained the keyword '{searchKeyword}'.");

    }
}
