using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using somelib;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.model;

namespace webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecuteController : ControllerBase
    {
        private readonly ILogger<ExecuteController> _logger;

        public ExecuteController(ILogger<ExecuteController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// Runtime compilation of the code provided and its execution
        /// </summary>
        /// <param name="toBeExecuted">Runtime code + input</param>
        /// <returns>The spec and its result</returns>
        /// <example>
        /// {
        ///     "code": "return _ => _ * 2;",
        ///     "input": 10
        /// }
        /// </example>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(summary: "Runtime compilation of the code provided and its execution")]
        public RuntimeCodeExecResult Post([FromBody] RuntimeCodeSpec toBeExecuted)
        {
            var instance = Generator.GenerateInstance(toBeExecuted.Code);

            var aDelegate = instance.DoOneThing();

            int radius = 10;

            var result = aDelegate(toBeExecuted.Input);

            return new RuntimeCodeExecResult() { Result = result, Spec = toBeExecuted };
        }
    }
}


