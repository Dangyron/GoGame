using System.IO;
using System.Text;
using GoGame.Models.Models;
using GoGame.Utility;
using GoGame.Utility.Constants;

namespace GoGame.Models.ReadWriters;

public class BoardReadWriter : IReadWriter<StonesList>
{
    public StonesList ReadFromFile()
    {
        using var reader = new StreamReader("Board.txt");
        var board = new StonesList();
        int i = 0;
        while (!reader.EndOfStream)
        {
            if (i >= Constants.CountOfCells)
                break;
            var str = reader.ReadLine()!.Split(';');

            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                if (str[j] != "0")
                {
                    board.Place(i, j, new Stone(str[j] == "1" ? StonesStates.White : StonesStates.Black));
                }
            }

            i++;
        }

        return board;
    }
    
    public void WriteToFile(StonesList board)
    {
        using var writer = new StreamWriter("Board.txt");

        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                if (board[i][j].StoneStates == StonesStates.Empty)
                {
                    writer.Write("0");
                }
                else
                {
                    writer.Write(board[i][j].StoneStates == StonesStates.White ? "1" : "-1");
                }
                writer.Write(";");
            }
            writer.Write("\n");
        }
    }

    public void Clear()
    {
        List<StringBuilder> board = new List<StringBuilder>(Constants.CountOfCells);

        for (int i = 0; i < Constants.CountOfCells; i++)
        {
            board.Add(new StringBuilder(Constants.CountOfCells));
            for (int j = 0; j < Constants.CountOfCells; j++)
            {
                board[i].Append("0;");
            }
            board[i].Append('\n');
        }
        
        File.WriteAllText("Board.txt", string.Concat(board));
    }
}