using Microsoft.Extensions.Logging;
using MixologyJournalApp.MAUI.ViewModel;
using MixologyJournalApp.MAUI.Views;

namespace MixologyJournalApp.MAUI
{
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

#if DEBUG
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<RecipeListPage>();
            builder.Services.AddTransient<RecipeViewPage>();
            builder.Services.AddSingleton<AppViewModel>();

            return builder.Build();
        }
    }
}