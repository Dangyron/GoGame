namespace GoGame.Models.ReadWriters;

public interface IReadWriter<T>
{
    public T ReadFromFile();

    public void WriteToFile(T type);

    public void Clear();
}