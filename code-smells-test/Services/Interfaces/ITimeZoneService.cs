namespace code_smells_test.Services.Interfaces
{
    public interface ITimeZoneService
    {
        DateTime ConvertToUtc(DateTime localTime);

        DateTime ConvertFromUtc(DateTime utcTime);
    }
}
