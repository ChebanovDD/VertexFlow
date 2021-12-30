using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace VertexFlow.WebAPI.Formatters
{
    public class StreamInputFormatter : InputFormatter
    {
        private const string MediaType = "application/octet-stream";

        public StreamInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(MediaType));
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(Stream);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            return await InputFormatterResult.SuccessAsync(context.HttpContext.Request.Body);
        }
    }
}