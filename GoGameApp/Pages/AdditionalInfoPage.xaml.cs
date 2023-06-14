using System.Windows.Controls;
using GoGame.Models.ReadWriters;
using GoGameApp.Windows;

namespace GoGameApp.Pages;

public partial class AdditionalInfoPage
{
    public AdditionalInfoPage()
    {
        InitializeComponent();
    }

    private void Button3_OnClick(object sender, RoutedEventArgs e)
    {
        bool isLoaded = new LoadReadWriter().ReadFromFile();
        
        if (Application.Current.MainWindow is not StartUpWindow mainWindow)
            return;

        var page = new StartUpPage(!isLoaded);
        mainWindow.Content = page;
        mainWindow.BindAllButtons(page);
        mainWindow.Show();
    }

    private void Button1_OnClick(object sender, RoutedEventArgs e)
    {
        var res = """
1. The board is empty at the onset of the game (unless players agree to place a handicap).
2. Black makes the first move, after which White and Black alternate.
3. A move consists of placing one stone of one's own color on an empty intersection on the board.
4. A player may pass their turn at any time.
5. A stone or solidly connected group of stones of one color is captured and removed from the board when all the intersections directly adjacent to it are occupied by the enemy. (Capture of the enemy takes precedence over self-capture.)
6. No stone may be played so as to recreate a former board position.
7. Two consecutive passes end the game.
8. A player's area consists of all the points the player has either occupied or surrounded.
9. The player with more area wins.
""";

        MessageBox.Show(Application.Current.MainWindow,res, "Rules", MessageBoxButton.OK);
    }

    private void Button2_OnClick(object sender, RoutedEventArgs e)
    {
        var res = """
1. Left mouse click - place stone,
2. Right mouse click - skip move
3. ⚑ - resign
""";

        MessageBox.Show(Application.Current.MainWindow,res, "Controls", MessageBoxButton.OK);
    }
}