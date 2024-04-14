namespace WeatherQuality.Telegram.Jobs;

public class HangfireActivator : Hangfire.JobActivator
{
    private readonly IServiceCollection _services;

    public HangfireActivator(IServiceCollection services) => _services = services;

    public override object ActivateJob(Type type) 
        => _services.BuildServiceProvider().GetRequiredService(type);
}