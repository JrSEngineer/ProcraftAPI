namespace ProcraftAPI.Data.Settings
{
    public class ProcraftSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DevConnectionString { get; set; } = string.Empty;
        public int Salt { get; set; }
        public int KeySize { get; set; }
        public int Iterations { get; set; }

        public string GetConnectionString(bool returnDevConnectionString = false)
        {
            return returnDevConnectionString ? DevConnectionString : ConnectionString;
        }
    }
}
