﻿using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;
using Exception = System.Exception;

namespace WordListsMauiHelpers.Logging;
public class ExceptionHandler
{
    public ExceptionHandler(AppDomain domain, ILogger logger)
    {
        Domain = domain;
        Logger = logger;
        logger.LogInformation("Added global exception handler");
    }

    ILogger Logger { get; }
    AppDomain Domain { get; }

    public void AddExceptionHandling()
    {
        Domain.FirstChanceException += FirstChanceException;
        Domain.UnhandledException += UnhandledException;
        HandleAndroidException();
        HandleWindowsExceptions();
    }

#pragma warning disable CA1822 // Mark members as static

    private void HandleWindowsExceptions()
    {
#if WINDOWS
        Microsoft.UI.Xaml.Application.Current.UnhandledException += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = GetLimitedStackTrace(ex.StackTrace) ?? "NULL";
            Logger.LogError("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
                sender?.GetType().Name, exType, exMessage, nameof(HandleAndroidException), trace);
        };
#endif
    }
    private void HandleAndroidException()
    {
#if ANDROID
        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
        {
            Exception ex = args.Exception;
            string exType = ex.GetType().Name;
            string exMessage = ex.Message;
            string trace = GetLimitedStackTrace(ex.StackTrace) ?? "NULL";
            Logger.LogError("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
                sender?.GetType().Name, exType, exMessage, nameof(HandleAndroidException), trace);
        };
#endif
    }

#pragma warning restore CA1822 // Mark members as static

    private void FirstChanceException(object? sender, FirstChanceExceptionEventArgs e)
    {
        Exception ex = e.Exception;
        string exType = ex.GetType().Name;
        string exMessage = ex.Message;
        string trace = GetLimitedStackTrace(ex.StackTrace) ?? "NULL";


        Logger.LogError("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
            sender?.GetType().Name, exType, exMessage, nameof(FirstChanceException), trace);
    }
    private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string exType;
        string exMessage;
        string trace;
        if (e.ExceptionObject is Exception ex)
        {
            exType = ex.GetType().Name;
            exMessage = ex.Message;
            trace = GetLimitedStackTrace(ex.StackTrace) ?? "NULL";
        }
        else
        {

            exType = e.ExceptionObject.GetType().Name;
            exMessage = "NULL, cant get real exception from ExceptionObject";
            trace = "NULL";
        }
        Logger.LogError("'{sender}' threw exception '{ex}': '{msg}', caught with '{methodName}', see trace \n{trace}",
            sender?.GetType().Name, exType, exMessage, nameof(UnhandledException), trace);
    }

    const int MaxAndroidStackTraceCharLen = 300;

    /// <summary>
    /// Android exceptions might include long stack traces from unmanaged calls that do not help debuggin process
    /// </summary>
    /// <param name="trace"></param>
    /// <returns>If android returns trace with max length of MaxAndroidStackTraceCharLen, otherwise trace</returns>
    private static string? GetLimitedStackTrace(string? trace)
    {
#if ANDROID
        if (trace?.Length > MaxAndroidStackTraceCharLen)
        {
            return trace[..MaxAndroidStackTraceCharLen];
        }
#endif
        return trace;
    }
}
