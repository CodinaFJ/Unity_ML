using UnityEngine;
using System;
using UnityEditor.PackageManager;

public static class SimpleLogger
{
	public static bool		LogEnabled {get; set; } = false;
	public static LogLevel	Loggerlevel
	{
		get => loggerLevel;
		private set => loggerLevel = value;
	}

	private static LogLevel	loggerLevel = LogLevel.Error;

	private static void DoLog(Action<string, UnityEngine.Object> LogFunction, string prefix, UnityEngine.Object myObj, object msg)
	{
		if (!LogEnabled) return;
		LogFunction($"{prefix} [OBJ: {myObj.name}]: {msg}", myObj);
	}

	private static void DoLog(Action<string> LogFunction, string prefix, object myObj, object msg)
	{
		if (!LogEnabled) return;
		LogFunction($"{prefix} [CLASS: {myObj.GetType()}]: {msg}");
	}

	public static void Log(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Debug);
	public static void Log(this UnityEngine.Object obj, object msg, LogLevel logLevel)
	{
		if (loggerLevel < logLevel) return;
		if (logLevel == LogLevel.Error)
			DoLog(Debug.LogError, logLevel.ToString(), obj, msg);
		else if (logLevel == LogLevel.Warn)
			DoLog(Debug.LogWarning, logLevel.ToString(), obj, msg);
		else
			DoLog(Debug.Log, logLevel.ToString(), obj, msg);
	}

	public static void Log(this object obj, object msg) => Log(obj, msg, LogLevel.Debug);
	public static void Log(this object obj, object msg, LogLevel logLevel)
	{
		if (loggerLevel < logLevel) return;
		if (logLevel == LogLevel.Error)
			DoLog(Debug.LogError, logLevel.ToString(), obj, msg);
		else if (logLevel == LogLevel.Warn)
			DoLog(Debug.LogWarning, logLevel.ToString(), obj, msg);
		else
			DoLog(Debug.Log, logLevel.ToString(), obj, msg);
	}

	public static void LogDebug(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Debug);
	public static void LogDebug(this object obj, object msg) => Log(obj, msg, LogLevel.Debug);
	public static void LogVerbose(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Verbose);
	public static void LogVerbose(this object obj, object msg) => Log(obj, msg, LogLevel.Verbose);
	public static void LogInfo(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Info);
	public static void LogInfo(this object obj, object msg) => Log(obj, msg, LogLevel.Info);
	public static void LogWarning(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Warn);
	public static void LogWarning(this object obj, object msg) => Log(obj, msg, LogLevel.Warn);
	public static void LogError(this UnityEngine.Object obj, object msg) => Log(obj, msg, LogLevel.Error);
	public static void LogError(this object obj, object msg) => Log(obj, msg, LogLevel.Error);

	public static void StartLogger() => StartLogger(LogLevel.Error);
	public static void StartLogger(LogLevel loggerLevel)
	{
		SimpleLogger.Loggerlevel = loggerLevel; 
		LogEnabled = true;
	}

	public static void StopLogger()
	{
		LogEnabled = false;
	}
}