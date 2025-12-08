using Medtracker.ViewModels;
using Medtracker.Pages;

namespace Medtracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AddMedicationPage), typeof(AddMedicationPage));
        Routing.RegisterRoute(nameof(ShowMedicationPage), typeof(ShowMedicationPage));
        Routing.RegisterRoute(nameof(RemoveMedicationPage), typeof(RemoveMedicationPage));
        Routing.RegisterRoute(nameof(EditMedicationPage), typeof(EditMedicationPage));
    }
}
