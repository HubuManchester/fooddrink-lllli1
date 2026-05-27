using FoodLens.Services;
using FoodLens.ViewModels;
using Microsoft.Extensions.Logging;

namespace FoodLens;

public static class MauiProgram
{
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

        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<ISpeechService, SpeechService>();
        builder.Services.AddSingleton<ICameraService, CameraService>();
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<IVisionService, VisionService>();

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

        builder.Services.AddTransient<ViewModels.HomePageViewModel>();
        builder.Services.AddTransient<ViewModels.CategoryListViewModel>();
        builder.Services.AddTransient<ViewModels.RecipeDetailViewModel>();
        builder.Services.AddTransient<ViewModels.SearchViewModel>();
        builder.Services.AddTransient<ViewModels.FavoritesViewModel>();
        builder.Services.AddTransient<ViewModels.MealPlanViewModel>();
        builder.Services.AddTransient<ViewModels.LoginViewModel>();
        builder.Services.AddTransient<ViewModels.RegisterViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
