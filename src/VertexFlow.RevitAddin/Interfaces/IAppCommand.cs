namespace VertexFlow.RevitAddin.Interfaces
{
    public interface IAppCommand
    {
        string Cmd { get; }
        string Name { get; }
        string ToolTip { get; }
    }
}