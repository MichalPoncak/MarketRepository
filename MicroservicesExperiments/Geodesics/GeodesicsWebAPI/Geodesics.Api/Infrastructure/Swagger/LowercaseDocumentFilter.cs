using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Geodesics.Api.Infrastructure.Swagger
{
    /// <summary>
    /// A Swagger document filter, which transforms all paths to lower case.
    /// </summary>
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context) =>
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(entry =>
                    LowercaseEverythingButParameters(entry.Key), entry => entry.Value);

        private static string LowercaseEverythingButParameters(string key) =>
            string.Join('/', key.Split('/')
                .Select(x => x.Contains("{") ? x : x.ToLowerInvariant()));
    }
}
