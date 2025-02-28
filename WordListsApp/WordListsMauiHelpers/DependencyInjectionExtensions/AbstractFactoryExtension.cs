﻿namespace WordListsMauiHelpers.DependencyInjectionExtensions;

#nullable disable
public static class AbstractFactoryExtension
{
    public static void AddAbstractFactory<TInterface, TImplementation>(this IServiceCollection services)
    where TInterface : class
    where TImplementation : class, TInterface
    {
        services.AddTransient<TInterface, TImplementation>();
        services.AddSingleton<Func<TInterface>>(x => () => x.GetService<TInterface>());
        services.AddSingleton<IAbstractFactory<TInterface>, AbstractFactory<TInterface>>();
    }
}