namespace GoGame.Utility;

public readonly record struct StoneIndexer(int I, int J)
{
    public override string ToString()
    {
        return $"{I},{J}";
    }
}