using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace PayDel.Common.Helpers
{
    public class IonOutputFormatter : TextOutputFormatter
    {
        private readonly SystemTextJsonOutputFormatter _jsonOutputFormatter;

        public IonOutputFormatter(SystemTextJsonOutputFormatter jsonOutputFormatter)
        {
            if (jsonOutputFormatter == null)
                throw new ArgumentNullException(nameof(jsonOutputFormatter));

            _jsonOutputFormatter = jsonOutputFormatter;


            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/ion+json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        => _jsonOutputFormatter.WriteResponseBodyAsync(context, selectedEncoding);
    }
}
