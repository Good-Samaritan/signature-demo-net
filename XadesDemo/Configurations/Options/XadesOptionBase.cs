namespace XadesDemo.Configurations.Options
{
    public abstract class XadesOptionBase : OptionBase
    {
        public virtual string Password { get; set; }
        public virtual string BasicAuthorization { get; set; }
    }
}