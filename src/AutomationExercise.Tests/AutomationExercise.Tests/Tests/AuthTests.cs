namespace AutomationExercise.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
class AuthTests : PageTest
{
    [Test]
    public async Task LoginUser_WithCorrectCredentials_ShouldSucceed_AndPersistSession()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        var signupPage = new SignupPage(Page);
        string uniqueEmail = await CreateThrowawayUserAsync(loginPage, signupPage);

        // Act
        await loginPage.LogoutUserAsync();
        await loginPage.LoginUserAsync(uniqueEmail, "Password123!");

        // Assert
        bool isLoggedIn = await loginPage.IsUserLoggedInAsync();
        Assert.That(isLoggedIn, Is.True, "The user was not successfully logged in.");
        await Page.ReloadAsync();

        bool isStillLoggedIn = await loginPage.IsUserLoggedInAsync();
        Assert.That(isStillLoggedIn, Is.True, "Session was dropped after page reload.");

        await signupPage.DeleteAccountAsync();
    }

    [Test]
    public async Task LogoutUser_ShouldReturnToLoginPage()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        var signupPage = new SignupPage(Page);
        string uniqueEmail = await CreateThrowawayUserAsync(loginPage, signupPage);

        // Act
        await loginPage.LogoutUserAsync();

        // Assert
        Assert.That(Page.Url, Does.Contain("/login"), "User was not redirected to the login page after logging out");

        await loginPage.LoginUserAsync(uniqueEmail, "Password123!");
        await signupPage.DeleteAccountAsync();
    }

    [Test]
    public async Task RegisterUser_WithValidData_ShouldCreateAndLogUserIn()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        var signupPage = new SignupPage(Page);
        string randomString = System.Guid.NewGuid().ToString().Substring(0, 8);
        string uniqueEmail = $"user_{randomString}@example.com";
        string name = "User Tester";

        // Act 1
        await loginPage.GotoAsync();
        await loginPage.StartSignupAsync(name, uniqueEmail);

        // Act 2
        await signupPage.FillAccountInfoAsync("Password123!", "User", "Tester", "123 Test St", "CA", "Los Angeles", "90001", "5551234567");

        // Assert 1
        bool isCreated = await signupPage.IsAccountCreatedAsync();
        Assert.That(isCreated, Is.True, "Account Created confirmation was not visible.");

        // Act 3 & Assert 2
        await signupPage.ClickContinueAsync();
        bool isLoggedIn = await loginPage.IsUserLoggedInAsync();
        Assert.That(isLoggedIn, Is.True, "User was not logged in after continuing from creation screen.");

        await signupPage.DeleteAccountAsync();
    }

    /// <summary>
    /// Creates a user and returns the generated email address.
    /// Leaves the browser in a "Logged in" state.
    /// </summary>
    private async Task<string> CreateThrowawayUserAsync(LoginPage loginPage, SignupPage signupPage)
    {
        string randomString = System.Guid.NewGuid().ToString().Substring(0, 8);
        string uniqueEmail = $"testuser_{randomString}@example.com";

        await loginPage.GotoAsync();
        await loginPage.StartSignupAsync("Temp User", uniqueEmail);
        await signupPage.FillAccountInfoAsync("Password123!", "Temp", "User", "123 Test St", "CA", "Los Angeles", "90001", "5551234567");
        await signupPage.ClickContinueAsync();

        return uniqueEmail;
    }
}
