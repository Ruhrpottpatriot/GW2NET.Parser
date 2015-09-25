namespace ParsingEngineTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage response;
        
        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            this.response = response;
        }

        public static HttpMessageHandler GetHttpMessageHandler(string content, HttpStatusCode httpStatusCode)
        {
            MemoryStream memStream = new MemoryStream();

            StreamWriter streamWriter = new StreamWriter(memStream);
            streamWriter.Write(content);
            streamWriter.Flush();
            memStream.Position = 0;
            
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StreamContent(memStream)
            };

            return new FakeHttpMessageHandler(response);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            TaskCompletionSource<HttpResponseMessage> completionSource = new TaskCompletionSource<HttpResponseMessage>();

            completionSource.SetResult(this.response);

            return await completionSource.Task;
        }
    }
}