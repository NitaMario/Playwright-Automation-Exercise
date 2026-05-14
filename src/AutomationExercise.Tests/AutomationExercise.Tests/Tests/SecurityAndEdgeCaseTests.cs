namespace AutomationExercise.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class SecurityAndEdgeCaseTests : PageTest
{
    [Test]
    public async Task UnauthorizedRouteBypassing_ShouldPreventAccessToPaymentPage()
    {
        // Arrange
        // We ensure we are logged out and have an empty cart.

        // Act
        await Page.GotoAsync("https://automationexercise.com/payment");

        // Assert
        Assert.That(Page.Url, Does.Not.Contain("/payment"), "Unauthenticated user was able to force-navigate to the /payment route.");
    }

    [Test]
    public async Task BoundaryValueInput_NegativeQuantity_ShouldNotBeProcessed()
    {
        // Arrange
        var detailsPage = new ProductDetailsPage(Page);
        var cartPage = new CartPage(Page);
        int negativeQuantity = -5;

        // Act
        await detailsPage.GotoAsync(1);
        await detailsPage.SetQuantityAndAddToCartAsync(negativeQuantity);

        // Assert
        int actualQuantity = await cartPage.GetFirstItemQuantityAsync();
        Assert.That(actualQuantity, Is.GreaterThan(0), $"Validation failed: The cart accepted a negative quantity of {actualQuantity}.");
    }

    [Test]
    public async Task IncorrectDataHandling_InvalidEmailFormat_ShouldBeBlocked()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        await loginPage.GotoAsync();

        // Act
        string invalidEmail = "not-an-email.com";
        await loginPage.StartSignupAsync("Test User", invalidEmail);

        // Assert
        Assert.That(Page.Url, Does.Contain("/login"), "The system accepted an incorrect email and proceeded to registration,");
    }

    [Test]
    public async Task NetworkInterception_BlockImages_PageShouldStillRender()
    {
        // Arrange
        await Page.RouteAsync("**/*.(png|jpg|jpeg|gif|svg)", route => route.AbortAsync());

        // Act
        var productsPage = new ProductsPage(Page);
        await productsPage.GotoAsync();

        // Assert
        var products = await productsPage.GetVisibleProductNamesAsync();
        Assert.That(products.Count, Is.GreaterThan(0), "The entire page crashed or failed to render data because the images were blocked.");
    }

    [Test]
    public async Task BrowserNavigation_BackButtonAfterSubmission_ShouldNotCrash()
    {
        // Arrange
        var contactPage = new ContactUsPage(Page);
        await contactPage.GotoAsync();
        await contactPage.FillContactFormAsync("User", "user@example.com", "Test subject", "Testing back button");

        // Act 1
        await contactPage.SubmitFormAndAcceptAlertAsync();
        Assert.That(await contactPage.IsSuccessMessageVisibleAsync(), Is.True);

        // Act 2
        await Page.GoBackAsync();
        string pageTitle = await Page.TitleAsync();
        Assert.That(pageTitle, Does.Not.Contain("Error").And.Not.Contain("500"), "Clicking the back button caused a server crash or duplicate POST request.");
    }
}
