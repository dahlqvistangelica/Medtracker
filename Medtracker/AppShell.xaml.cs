namespace Medtracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AddMedicationPage), typeof(AddMedicationPage));
        Routing.RegisterRoute(nameof(ShowMedicationPage), typeof(ShowMedicationPage));
    }
}
