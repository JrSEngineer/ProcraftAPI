namespace ProcraftAPI.Data.Settings
{
    public class ProcraftSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DevConnectionString { get; set; } = string.Empty;

        public string GetConnectionString(bool returnDevConnectionString = false)
        {
            return returnDevConnectionString ? DevConnectionString : ConnectionString;
        }
    }
}
