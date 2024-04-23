using PatientInfo.API.Services;
using System.Text;

namespace PatientInfo.API.Test
{
    public class CsvFormatTests
    {
        [Fact]
        public async Task FormatCSV_ValidInput_ReturnsOK()
        {
            // Arrange
            var csvData = "\"Patient Name\",\"SSN\",\"Age\",\"Phone Number\",\"Status\"\n" +
                              "\"Prescott, Zeke\",\"542-51-6641\",21,\"801-555-2134\",\"Opratory=2,PCP=1\"\n" +
                              "\"Goldstein, Bucky\",\"635-45-1254\",42,\"435-555-1541\",\"Opratory=1,PCP=1\"\n" +
                              "\"Vox, Bono\",\"414-45-1475\",51,\"801-555-2100\",\"Opratory=3,PCP=2\"";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var csvService = new CSVService();

            // Act
            var result = await csvService.FormatCSV(stream);

            // Assert
            Assert.Equal(Status.OK, result.Item1);
            Assert.NotNull(result.Item2);

            // Add more assertions as needed to verify the content of the response
            Assert.Contains("[Patient Name] [SSN] [Age] [Phone Number] [Status]", result.Item2);
            Assert.Contains("[Prescott, Zeke] [542-51-6641] [21] [801-555-2134] [Opratory=2,PCP=1]", result.Item2);
            Assert.Contains("[Goldstein, Bucky] [635-45-1254] [42] [435-555-1541] [Opratory=1,PCP=1]", result.Item2);
            Assert.Contains("[Vox, Bono] [414-45-1475] [51] [801-555-2100] [Opratory=3,PCP=2]", result.Item2);
        }

        [Fact]
        public async Task FormatCSV_EmptyInput_ReturnsBadRequest()
        {
            // Arrange
            var stream = new MemoryStream();
            var csvService = new CSVService();

            // Act
            var result = await csvService.FormatCSV(stream);

            // Assert
            Assert.Equal(Status.BadRequest, result.Item1);
            Assert.Equal("Request body is empty.", result.Item2);
        }

        [Fact]
        public async Task FormatCSV_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var csvData = "\"Patient Name\",\"SSN\",\"Age\",\"PhoneNumber\",\"Status\"\n" +
                              "\"Prescott, Zeke\",\"542-51-6641\",21,\"801-555-2134\",\"Opratory=2,PCP=1\"\n" +
                              "\"Goldstein, Bucky\",\"635-45-1254\",42,\"435-555-1541\",\"Opratory=1,PCP=1\"\n" +
                              "\"Vox, Bono\",\"414-45-1475\",51,\"801-555-2100\",\"Opratory=3,PCP=2\"";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            var csvService = new CSVService();

            // Act
            var result = await csvService.FormatCSV(stream);

            // Assert
            Assert.Equal(Status.BadRequest, result.Item1);
            Assert.Equal("Invalid header input.", result.Item2);
        }
    }
}
