using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using IronWebScraper;


[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
       public async Task<APIGatewayProxyResponse> Scrape(APIGatewayProxyRequest request, ILambdaContext context)
       {
          Scraper Scraper = JsonConvert.DeserializeObject<Scraper>(request.Body);
          Scraper.Start();

          var body = new Dictionary<string, string>() { { "title",  Scraper.title }, { "body",  Scraper.body } };
          return new APIGatewayProxyResponse
          {
              StatusCode = 200,
              Body = JsonConvert.SerializeObject(body),
              Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
          };
       }

       
    }

    public class Scraper : WebScraper
    {
      public string url {get; set;}
      public string title {get; set;}
      public string body {get; set;}

      public override void Init()
        {
          this.Request("https://"+url, Parse);
        }

      public override void Parse(Response response)
        {
            this.title = response.GetElementsByTagName("title")[0].InnerTextClean;
            this.body = response.GetElementsByTagName("body")[0].InnerTextClean;
        }
    }




}
