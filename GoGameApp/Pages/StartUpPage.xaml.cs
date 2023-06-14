namespace GoGameApp.Pages;

public partial class StartUpPage
{
    public StartUpPage(bool isStarted)
    {
        InitializeComponent();
        ShowContinue(isStarted);
    }

    public void ShowContinue(bool isNotStarted)
    {
        Button0.Height = isNotStarted ? 0 : 50;
        Button0.Visibility = isNotStarted ? Visibility.Hidden : Visibility.Visible;
    }
}