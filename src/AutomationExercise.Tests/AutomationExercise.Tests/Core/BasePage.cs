namespace AutomationExercise.Tests.Core;

public abstract class BasePage
{
    protected readonly IPage _page;
    
    protected BasePage(IPage page)
    {
        _page = page;
    }
}
