namespace VertexFlow.RevitAddin.Interfaces
{
    public interface IApp
    {
        T Resolve<T>();
    }
}