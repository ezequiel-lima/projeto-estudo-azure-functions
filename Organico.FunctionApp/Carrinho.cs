using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Organico.Library;
using Organico.Library.Model;

namespace Organico.FunctionApp
{
    public class Carrinho
    {
        private readonly ILogger _logger;

        public Carrinho(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Carrinho>();
        }

        [Function("Carrinho")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var cosmosClient = CarrinhoCosmosClient.Instance();

            if (req.Method == "GET")
            {
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                var cart = await cosmosClient.Get();

                response.WriteString(JsonSerializer.Serialize(cart.Items));
            }

            if (req.Method == "POST")
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var cartItem = JsonSerializer.Deserialize<CartItem>(content);

                await cosmosClient.Post(cartItem);
            }

            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
