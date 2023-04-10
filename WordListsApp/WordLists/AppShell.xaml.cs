﻿using Microsoft.Extensions.Logging;
using WordDataAccessLibrary.Helpers;
using WordListsMauiHelpers.PageRouting;
using WordListsUI.WordDataPages.OcrListGeneratorPage;

namespace WordLists;

public partial class AppShell : Shell
{
    public AppShell(ILogger logger)
	{
		InitializeComponent();
        Logger = logger;

        string training = PageRoutes.Get(Route.Training);
        string handling = PageRoutes.Get(Route.WordHandling);
        string lifetime = PageRoutes.Get(Route.LifeTime);
        string backup = PageRoutes.Get(Route.Backup);

        // main pages
        Register<HomePage>();
        Register<AppInfoPage>();

        //  training
        Register<StartTrainingPage>(training);
        Register<FlipCardTrainingPage>(training);
        Register<WritingTestPage>(training);
        Register<WritingTestConfigurationPage>(training);
        Register<WriteTestResultPage>(training);
        Register<WordListPage>(training);
        Register<TrainingConfigPage>(training);

        //  handling
        Register<WordDataPage>(handling);

        //  handling/backup
        Register<JsonImportPage>(handling, backup);
        Register<JsonExportPage>(handling, backup);

        //  handling/lifetime
        Register<ListGeneratorPage>(handling, lifetime);
        Register<OcrListGeneratorPage>(handling, lifetime);
        Register<WordCollectionEditPage>(handling, lifetime);
    }

    ILogger Logger { get; }
    bool CommandLineArgsChecked { get; set; } = false;

    /// <summary>
    /// Register T type to shell. Route will be type: 'Route/Pieces/TName'
    /// <para/>Example: Register&lt;MyType&gt;("home") =&gt; home/MyType
    /// </summary>
    /// <typeparam name="T">Type to register to shell</typeparam>
    /// <param name="routePieces">
    /// Will be separated with '/', order will be the same. 
    /// Leave null or empty if no route pieces need to be added.
    /// </param>
    private static void Register<T>(params string[]? routePieces) where T : ContentPage
    {
        string route;
        if (routePieces is null || routePieces.Length is 0)
        {
            route = typeof(T).Name;
        }
        else
        {
            route = $"{string.Join('/', routePieces)}/{typeof(T).Name}";
        }
        Routing.RegisterRoute(route, typeof(T));
    }






    private async void Shell_Loaded(object sender, EventArgs e) => await CheckCommandLineArgs();
    private async Task CheckCommandLineArgs()
    {
        if (CommandLineArgsChecked)
        {
            return;
        }
        CommandLineArgsChecked = true;
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        Logger.LogInformation("Check if commandline args were provided.");
        try
        {
            string appFileExtension = AppFileExtension.Get(FileExtension.Wordlist);
            var path = Environment.GetCommandLineArgs()
                .Where(x => x is not null)
                .Where(x => Path.GetExtension(x) == appFileExtension && Path.Exists(x))
                .FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.LogInformation("No path with app file extension {extension} was provided, no need to open edit page.", appFileExtension);
                return;
            }
            Logger.LogInformation("Path '{path}' is a path to .wordlist file", path);
            await Current.GoToAsync($"{PageRoutes.Get(Route.WordHandling)}/{PageRoutes.Get(Route.Backup)}/{nameof(JsonImportPage)}", new Dictionary<string, object>()
            {
                [nameof(JsonImportPage.DefaultImportPath)] = path
            });
        }
        catch (NotSupportedException)
        {
            Logger.LogError("Cannot get command line args in current platform.");
            return;
        }
    }
}
