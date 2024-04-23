using Microsoft.AspNetCore.Mvc;
using PatientInfo.API.Services;

namespace PatientInfo.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly ICSVService _csvService;

        public PatientsController(ICSVService csvService)
        {
            _csvService = csvService ?? throw new ArgumentNullException(nameof(csvService));
        }

        /// <summary>
        /// Read csv input from the request body and format it.
        /// </summary>
        /// <returns>Formatted string with each element enclosed in square bracket</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> FormatPatientsInput()
        {
            (Status status, string result) = await _csvService.FormatCSV(Request.Body);

            if (status == Status.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {result}");
            }

            if (status == Status.BadRequest)
            {
                return BadRequest(result);
            }

            return Content(result, "text/plain");
        }
    }
}
