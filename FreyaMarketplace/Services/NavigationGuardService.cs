namespace FreyaMarketplace.Services;

public interface INavigationGuardService
{
    void SetNavigationGuard(Func<Task<bool>> canNavigateCallback);
    void ClearNavigationGuard();
    Task<bool> CanNavigateAsync();
}


public class NavigationGuardService : INavigationGuardService
{
    private Func<Task<bool>> _canNavigate;

    public void SetNavigationGuard(Func<Task<bool>> canNavigateCallback)
    {
        _canNavigate = canNavigateCallback;
    }

    public void ClearNavigationGuard()
    {
        _canNavigate = null;
    }

    public async Task<bool> CanNavigateAsync()
    {
        return _canNavigate == null || await _canNavigate.Invoke();
    }
}
