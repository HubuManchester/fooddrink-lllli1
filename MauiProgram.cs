using FoodLens.Services;
using FoodLens.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace FoodLens;

/// <summary>
/// Application startup configuration. Registers services, views, view models, and routes.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates and configures the MAUI application with dependency injection.
    /// </summary>
    /// <returns>The configured MauiApp instance.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        Routing.RegisterRoute("recipedetail", typeof(Views.RecipeDetailPage));
        Routing.RegisterRoute("register", typeof(Views.RegisterPage));
        Routing.RegisterRoute("recipeedit", typeof(Views.RecipeEditPage));

        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<ISpeechService, SpeechService>();
        builder.Services.AddSingleton<ICameraService, CameraService>();
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<IVisionService, VisionService>();
        builder.Services.AddSingleton<ISpeechRecognitionService, SpeechRecognitionService>();
        builder.Services.AddSingleton<IAccelerometerService, AccelerometerService>();

        builder.Services.AddTransient<Views.SplashPage>();
        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddTransient<Views.CategoryListPage>();
        builder.Services.AddTransient<Views.RecipeDetailPage>();
        builder.Services.AddTransient<Views.SearchPage>();
        builder.Services.AddTransient<Views.FavoritesPage>();
        builder.Services.AddTransient<Views.MealPlanPage>();
        builder.Services.AddTransient<Views.SettingsPage>();
        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.RegisterPage>();
        builder.Services.AddTransient<Views.AboutPage>();
        builder.Services.AddTransient<Views.RecipeEditPage>();

        builder.Services.AddTransient<ViewModels.HomePageViewModel>();
        builder.Services.AddTransient<ViewModels.CategoryListViewModel>();
        builder.Services.AddTransient<ViewModels.RecipeDetailViewModel>();
        builder.Services.AddTransient<ViewModels.SearchViewModel>();
        builder.Services.AddTransient<ViewModels.FavoritesViewModel>();
        builder.Services.AddTransient<ViewModels.MealPlanViewModel>();
        builder.Services.AddTransient<ViewModels.LoginViewModel>();
        builder.Services.AddTransient<ViewModels.RegisterViewModel>();
        builder.Services.AddTransient<ViewModels.SettingsViewModel>();
        builder.Services.AddTransient<ViewModels.RecipeEditViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        BaseViewModel.SetServiceProvider(app.Services);
        return app;
    }
}
