using PatientInfo.API.Models;
using PatientInfo.API.Utilities;
using System.Text;

namespace PatientInfo.API.Services
{
    public class CSVService : ICSVService
    {
        public async Task<(Status, string)> FormatCSV(Stream stream)
        {
            try
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var response = new StringBuilder();
                    var patients = new List<PatientDto>();

                    //Read the first line to make sure the header is well-formed.
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        var headers = CsvUtil.SplitString(line);
                        if (!CsvUtil.HeaderValidation(headers.ToArray()))
                            return (Status.BadRequest, "Invalid header input.");

                        response.AppendLine($"[{headers[0]}] [{headers[1]}] [{headers[2]}] [{headers[3]}] [{headers[4]}]");
                    }
                    else
                    {
                        return (Status.BadRequest, "Request body is empty.");
                    }

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var values = CsvUtil.SplitString(line);

                        // Assume that the patient input is well-formed.
                        var patient = new PatientDto
                        {
                            PatientName = values[0],
                            SSN = values[1],
                            Age = Int32.TryParse(values[2], out int age) ? age : null,
                            PhoneNumber = values[3],
                            Status = values[4],
                        };

                        patients.Add(patient);
                    }

                    foreach (var patient in patients)
                    {
                        response.AppendLine($"[{patient.PatientName}] [{patient.SSN}] [{patient.Age}] [{patient.PhoneNumber}] [{patient.Status}]");
                    }

                    return (Status.OK, response.ToString());
                }
            }
            catch (Exception ex)
            {
                return (Status.Exception, ex.Message);
            }
        }
    }
}
