namespace AutomationExercise.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class CheckoutTests : PageTest
{
    [Test]
    public async Task EndToEnd_CheckoutFlow_ShouldProcessOrderSuccessfully()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        var signupPage = new SignupPage(Page);
        var productsPage = new ProductsPage(Page);
        var cartPage = new CartPage(Page);
        var checkoutPage = new CheckoutPage(Page);
        var paymentPage = new PaymentPage(Page);

        string firstName = "User";
        string lastName = "Tester";

        await CreateThrowawayUserAsync(loginPage, signupPage, firstName, lastName);

        // Act 1
        await productsPage.GotoAsync();
        await productsPage.AddProductToCartAsync(1);

        await cartPage.GotoAsync();
        await cartPage.ClickProceedToCheckoutAsync();

        // Assert 1
        string displayedName = await checkoutPage.GetDeliveryNameAsync();
        Assert.That(displayedName, Does.Contain(firstName), "Checkout shipping name did not match the registered user.");

        // Act 2
        await checkoutPage.PlaceOrderAsync("Leave package at the front door.");
        await paymentPage.FillPaymentDetailsAndSubmitAsync($"{firstName} {lastName}", "4111111111111111", "123", "12", "2030");

        // Assert 2
        bool isOrderSuccessful = await paymentPage.IsOrderPlacedSuccessfullyAsync();
        Assert.That(isOrderSuccessful, Is.True, "Order Placed success message was not visible.");

        await signupPage.DeleteAccountAsync();
    }

    [Test]
    public async Task EndToEnd_GuestCheckout_ShouldPromptRegistration_AndPreserveCartState()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        var signupPage = new SignupPage(Page);
        var productsPage = new ProductsPage(Page);
        var cartPage = new CartPage(Page);
        var checkoutPage = new CheckoutPage(Page);
        var paymentPage = new PaymentPage(Page);

        string firstName = "Guest";
        string lastName = "Tester";
        string randomString = System.Guid.NewGuid().ToString().Substring(0, 8);
        string uniqueEmail = $"guestuser_{randomString}@example.com";

        // Act 1
        await productsPage.GotoAsync();
        await productsPage.AddProductToCartAsync(1);

        // Act 2
        await cartPage.GotoAsync();
        await cartPage.ClickProceedToCheckoutAsync();
        await cartPage.ClickRegisterOnCheckoutModalAsync();

        // Act 3
        await loginPage.StartSignupAsync($"{firstName} {lastName}", uniqueEmail);
        await signupPage.FillAccountInfoAsync("Password123!", firstName, lastName, "123 Test St", "CA", "Los Angeles", "90001", "5551234567");
        await signupPage.ClickContinueAsync();

        // Act 4
        await cartPage.GotoAsync();
        await cartPage.ClickProceedToCheckoutAsync();

        // Assert 1
        string displayedName = await checkoutPage.GetDeliveryNameAsync();
        Assert.That(displayedName, Does.Contain(firstName), "Cart session was dropped or shipping name failed after mid-checkout registration.");

        // Act 5
        await checkoutPage.PlaceOrderAsync("Leave package at the front door.");
        await paymentPage.FillPaymentDetailsAndSubmitAsync($"{firstName} {lastName}", "4111111111111111", "123", "12", "2030");

        // Assert 2
        Assert.That(await paymentPage.IsOrderPlacedSuccessfullyAsync(), Is.True, "Order Placed success message was not visible.");

        await signupPage.DeleteAccountAsync();
    }

    private async Task CreateThrowawayUserAsync(LoginPage loginPage, SignupPage signupPage, string firstName, string lastName)
    {
        string randomString = System.Guid.NewGuid().ToString().Substring(0, 8);
        string uniqueEmail = $"testuser_{randomString}@example.com";

        await loginPage.GotoAsync();
        await loginPage.StartSignupAsync($"{firstName} {lastName}", uniqueEmail);
        await signupPage.FillAccountInfoAsync("Password123!", firstName, lastName, "123 Test St", "CA", "Los Angeles", "90001", "5551234567");
        await signupPage.ClickContinueAsync();
    }
}
