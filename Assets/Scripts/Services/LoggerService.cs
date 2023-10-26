public class LoggerService
{
	public LoggerService()
	{
		SimpleLogger.StartLogger(UnityEditor.PackageManager.LogLevel.Debug);
		this.LogDebug("Started");
	}
}
