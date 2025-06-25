namespace WebApi.Configs
{
    public class JobSettings
    {
        public Setting ExpiredJob { get; set; }
    }

    public class Setting
    {
        public bool Enable { get; set; }
        public string Daily { get; set; }
    }
}
