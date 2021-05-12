namespace ImageParser.App.Options
{
    public class StorageAccountOptions
    {
        public const string SectionName = "StorageAccount";

        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}