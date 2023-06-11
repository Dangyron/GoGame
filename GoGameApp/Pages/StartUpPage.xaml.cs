namespace GoGameApp.Pages;

public partial class StartUpPage : Page
{
    public StartUpPage(bool isFirst)
    {
        InitializeComponent();
        Button1.Content = isFirst ? "Start game" : "Continue Game";
    }
}