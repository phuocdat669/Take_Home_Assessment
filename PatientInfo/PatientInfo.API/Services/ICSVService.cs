namespace PatientInfo.API.Services
{
    public enum Status
    {
        OK,
        BadRequest,
        Exception,
    }

    public interface ICSVService
    {
        Task<(Status, string)> FormatCSV(Stream stream);
    }
}
