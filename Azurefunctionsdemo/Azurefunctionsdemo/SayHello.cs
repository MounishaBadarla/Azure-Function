using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azurefunctionsdemo
{
    public static class FibonacciSeriesfunc
    {
        [FunctionName("FibonacciSeriesfunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string fno = req.Query["fibonaccinumber"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            fno = fno ?? data?.name;
            long a = 0; long b = 1; long r = 0;
            string result = "";
            try
            {
                if (Convert.ToInt64(fno) == 0 || Convert.ToInt64(fno) == 1)
                {
                    result = fno;
                }
                else if (Convert.ToInt64(fno) > 1)
                {
                    result = a + " " + b;
                    for (int i = 2; i < Convert.ToInt64(fno); i++)
                    {
                        r = a + b;
                        result = result + " " + r;
                        a = b;
                        b = r;
                    }
                }
                else
                {
                    result = "invalid. Enter a valid positive integer";
                }

            }
             catch(Exception ex)
            {
                result ="invalid. "+ ex.Message;
            }
            string responseMessage = string.IsNullOrEmpty(fno)
                ? "This HTTP triggered function executed successfully. Pass the fibonaccinumber in the query string."
                : $"Hello, fibonacci series for {fno} is {result}";

            return new OkObjectResult(responseMessage);
        }
    }
}
