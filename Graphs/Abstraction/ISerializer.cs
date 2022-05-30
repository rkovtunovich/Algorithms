namespace Graphs.Abstraction;
public interface ISerializer<T> where T : notnull
{
    public string Seralize(); 
}
