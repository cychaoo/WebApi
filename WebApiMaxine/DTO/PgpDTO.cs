namespace Net6Api.DTO
{
    public class PgpDTO
    {
        public string? inputFile { get; set; }
        public string? outputFile { get; set; }

        public string? publicKeyFile { get; set; }

        public bool armor { get; set; }

        public bool withIntegrityCheck { get; set; }
    }
}
