using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using LogicAppSamplingMath.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogicAppSamplingMath
{
    public static class ComputeSampleSize
    {
        [FunctionName("ComputeSapleSize")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            ComputeSampleSizeRequest model = null;
            using (var reader = new StreamReader(req.Body))
            {
                var stringBody = await reader.ReadToEndAsync();
                model = JsonConvert.DeserializeObject<ComputeSampleSizeRequest>(stringBody);
            }

            var indexesToProcess = ComputeRandomIndexes(model.SampleSize, model.TotalNumberOfImages);

            var resultObject = new { indexes = indexesToProcess, numberOfImagesToProcess = indexesToProcess.Count() };

            return new OkObjectResult(resultObject);
        }

        private static IEnumerable<int> ComputeRandomIndexes(int sampleSize, int totalNumberOfImages)
        {
            var list = Enumerable.Range(0, totalNumberOfImages);
            var percent = sampleSize / 100.00m;
            var numberOfImagesToProcess = Convert.ToInt32(totalNumberOfImages * percent);

            if (numberOfImagesToProcess != totalNumberOfImages)
            {
                var random = new Random();
                var randomArray = list.OrderBy(_ => random.Next());

                list = randomArray.Take(numberOfImagesToProcess)
                    .OrderBy(_ => _);
            }

            return list;
        }
    }
}
