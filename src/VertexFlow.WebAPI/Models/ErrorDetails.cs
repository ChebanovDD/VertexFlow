using System.Text.Json;

namespace VertexFlow.WebAPI.Models
{
    public record ErrorDetails(int StatusCode, string Message)
    {
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}