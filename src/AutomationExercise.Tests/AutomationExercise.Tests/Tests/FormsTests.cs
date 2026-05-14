namespace AutomationExercise.Tests.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class FormsTests : PageTest
{
    [Test]
    public async Task SubmitContactForm_WithValidPayloadAndFile_ShouldSucceed()
    {
        // Arrange
        var contactPage = new ContactUsPage(Page);

        string dummyFilePath = "test_payload.txt";
        await File.WriteAllTextAsync(dummyFilePath, "This is a test file for contact form submission.");

        try
        {
            // Act 1
            await contactPage.GotoAsync();
            await contactPage.FillContactFormAsync("User", "user@example.com", "Bug Report", "Found an issue with the cart calculation.");

            // Act 2
            await contactPage.UploadFileAsync(dummyFilePath);

            // Act 3
            await contactPage.SubmitFormAndAcceptAlertAsync();

            // Assert
            bool isSuccess = await contactPage.IsSuccessMessageVisibleAsync();
            Assert.That(isSuccess, Is.True, "The success message was not visible after submitting the contact form.");
        }
        finally
        {
            if (File.Exists(dummyFilePath))
            {
                File.Delete(dummyFilePath);
            }
        }
    }

    [Test]
    public async Task ClickTestCasesButton_ShouldRouteToCorrectPage()
    {
        // Arrange
        var testCasesButton = Page.Locator("header a[href='/test_cases']").First;

        // Act
        await Page.GotoAsync("https://automationexercise.com/");
        var consentButton = Page.Locator(".fc-cta-consent");
        try
        {
            await consentButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
            await consentButton.ClickAsync();
        }
        catch
        {
        }
        await testCasesButton.ClickAsync();

        // Assert
        Assert.That(Page.Url, Does.Contain("/test_cases"), "Routing failed: URL did not update to /test_cases.");
    }
}
