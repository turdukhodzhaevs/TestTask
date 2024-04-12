using TestTask.Services;

namespace TestTask.Interfaces
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);
        ExecutionContext HandleRequest(ExecutionContext request);
    }
}