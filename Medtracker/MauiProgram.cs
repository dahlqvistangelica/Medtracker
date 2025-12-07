using Microsoft.Extensions.Logging;
using Medtracker.Interfaces;
using Medtracker.Services;
using MedTrackConsole.Persistence;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Services;
using Medtracker.ViewModels;


namespace Medtracker;

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
		//Filestorage
		builder.Services.AddSingleton<IPathProvider, MAUIPathProvider>();

		builder.Services.AddSingleton<IFileStorage>(ServiceProvider =>
		{
			var pathProvider = ServiceProvider.GetRequiredService<IPathProvider>();
			string path = pathProvider.GetDatabasePath();

			return new StoreData(path);
		});
		//Repository registry
		
		builder.Services.AddSingleton<IHandlerRepo, DataManager>();
		builder.Services.AddSingleton<MainPageViewModel>();
		
		//Add pages
        builder.Services.AddTransient<AddMedicationPage>();
        builder.Services.AddTransient<AddMedicationViewModel>();
		builder.Services.AddTransient<ShowMedicationPage>();
		builder.Services.AddTransient<ShowMedicationsViewModel>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
