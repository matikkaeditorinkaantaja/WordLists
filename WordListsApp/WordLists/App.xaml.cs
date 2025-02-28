﻿using Microsoft.Maui.Handlers;
using Microsoft.Extensions.Logging;
#if WINDOWS
using Microsoft.UI;
using Windows.UI.Text;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
#endif


namespace WordLists;

public partial class App : Application
{
    public App(ILogger logger)
    {
        InitializeComponent();
        SetUIHandlers();
        MainPage = new AppShell(logger);
    }
    private static void SetUIHandlers()
    {
#if WINDOWS
        var color = new Windows.UI.Color
        {
            A = 255,
            R = 162,
            G = 165,
            B = 173
        };

        SwitchHandler.Mapper.AppendToMapping("SwitchTextColorModifyer", (handler, view) =>
        {
            if (view is Switch switchView)
            {
                handler.PlatformView.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(color);
            }
        });
#endif
    }


   
}
