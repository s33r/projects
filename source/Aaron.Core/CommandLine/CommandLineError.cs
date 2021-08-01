namespace Aaron.Core.CommandLine
{
    public class CommandLineError
    {
        public int ArgumentPosition { get; set; }
        public string CommandName { get; set; } = string.Empty;

        public bool Fatal { get; set; }

        public string Message { get; set; } = string.Empty;
        public string ParameterName { get; set; } = string.Empty;
    }
}