﻿using WordDataAccessLibrary.CollectionBackupServices;
using WordDataAccessLibrary.CollectionBackupServices.JsonServices;
using WordDataAccessLibrary.DataBaseActions;
using WordDataAccessLibrary.DataBaseActions.Interfaces;
using WordDataAccessLibrary.Generators;
using WordListsMauiHelpers;
using WordListsMauiHelpers.DependencyInjectionExtensions;
using WordListsMauiHelpers.DeviceAccess;
using WordListsMauiHelpers.Imaging;
using WordListsMauiHelpers.Logging;
using WordListsMauiHelpers.Settings;
using WordListsServices.FileSystemServices;
using WordListsServices.ProcessServices;
using WordListsUI.WordDataPages.OcrListGeneratorPage;
using WordListsViewModels;
using WordListsViewModels.Helpers;
using WordListsViewModels.Interfaces;
using WordValidationLibrary;

namespace WordLists;

internal static class Injections
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<IWordPairService, WordPairService>();
        services.AddSingleton<IWordCollectionOwnerService, WordCollectionOwnerService>();
        services.AddSingleton<IWordCollectionService, WordCollectionService>();
        services.AddSingleton<ICollectionExportService, WordCollectionExportService>();
        services.AddSingleton<ICollectionImportService, JsonWordCollectionImportService>();
        services.AddSingleton<IWordCollectionInfoService, WordCollectionInfoService>();
        services.AddSingleton<IUserInputWordValidator, UserInputWordValidator>();
        services.AddTransient<IFolderHandler, FolderHandler>();
        services.AddTransient<IFileHandler, FileHandler>();
        services.AddTransient<IProcessLauncher, ProcessLauncher>();
        services.AddTransient<IWordPairParser, OtavaWordPairParser>();
        services.AddTransient<IWordPairParser, NewOtavaWordPairParser>();
        services.AddTransient<IWordPairParser, OcrWordPairParser>();
        services.AddTransient<IImageScaler, ImageScaler>();
        services.AddSingleton<ILoggingInfoProvider, DefaultLoggingProvider>();
        services.AddSingleton<IFilePickerService, FilePickerService>();
        services.AddSingleton<ISettings, Settings>();
        services.AddSingleton(Clipboard.Default); 
        services.AddSingleton(FilePicker.Default);

        // Platform specific implementations
#if WINDOWS
        services.AddSingleton<IMediaPicker, WindowsMediaPicker>();
#else
        services.AddSingleton(MediaPicker.Default);
#endif
        return services;
    }

    public static IServiceCollection AddAppPages(this IServiceCollection services)
    {
        services.AddSingleton<HomePage>();
        services.AddTransient<FlipCardTrainingPage>();
        services.AddTransient<StartTrainingPage>();
        services.AddTransient<WordCollectionEditPage>();
        services.AddTransient<ListGeneratorPage>();
        services.AddTransient<WordDataPage>();
        services.AddTransient<AppInfoPage>();
        services.AddSingleton<WritingTestPage>();
        services.AddSingleton<WritingTestConfigurationPage>();
        services.AddTransient<WriteTestResultPage>();
        services.AddTransient<JsonExportPage>();
        services.AddTransient<JsonImportPage>();
        services.AddTransient<WordListPage>();
        services.AddTransient<TrainingConfigPage>();
        services.AddSingleton<OcrListGeneratorPage>();
        services.AddSingleton<PhoneOcrListGeneratorPage>();

        return services;
    }

    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IWordTrainingViewModel, WordTrainingViewModel>();
        services.AddTransient<IStartTrainingViewModel, StartTrainingViewModel>();
        services.AddTransient<IWordCollectionHandlingViewModel, WordCollectionHandlingViewModel>();
        services.AddTransient<IWordDataViewModel, WordDataViewModel>();
        services.AddTransient<IAppInfoViewModel, AppInfoViewModel>();
        services.AddTransient<ITestResultViewModel, TestResultViewModel>();
        services.AddTransient<IWordListViewModel, WordListViewModel>();
        services.AddTransient<ITrainingConfigViewModel, TrainingConfigViewModel>();
        services.AddTransient<IOcrListGeneratorViewModel, OcrListGeneratorViewModel>();
        services.AddAbstractFactory<IListGeneratorViewModel, ListGeneratorViewModel>();
        services.AddAbstractFactory<IJsonExportViewModel, JsonExportViewModel>();
        services.AddAbstractFactory<IJsonImportViewModel, JsonImportViewModel>();
        services.AddAbstractFactory<IWriteWordViewModel, WriteWordViewModel>();
        services.AddAbstractFactory<IWritingTestConfigurationViewModel, WritingTestConfigurationViewModel>();

        return services;
    }
}
