using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Extensions.Extensions
{
    internal static class TaskExtensions
    {
        public static async void Forget(this Task task, bool configureAwait = true,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(configureAwait);
            }
            catch (Exception exception) when (onException != null)
            {
                onException(exception);
            }
        }
    }
}