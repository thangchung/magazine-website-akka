using System.Threading.Tasks;
using Cik.Magazine.ApiGateway.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cik.Magazine.ApiGateway.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ILogger _logger;
        private readonly ISchema _schema;

        public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema, ILogger<GraphQLController> logger)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var executionOptions = new ExecutionOptions {Schema = _schema, Query = query.Query};
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                _logger.LogError("GraphQL errors: {0}", result.Errors);
                return BadRequest(result);
            }

            _logger.LogDebug("GraphQL execution result: {result}", JsonConvert.SerializeObject(result.Data));
            return Ok(result);
        }
    }
}