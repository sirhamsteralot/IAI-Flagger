namespace Flagger;

using Flagger.Client;
using Flagger.Models;

[QueryProperty(nameof(flaggerClient), "Client")]
public partial class LobbyPage : ContentPage
{
    private int counter = 0;

    public FlaggerClient flaggerClient { set { if (value != null) ((FlagScreenModel)BindingContext).FlaggerClient = value; } }
    public bool IsConnected { get; set; } = false;

    public LobbyPage()
	{
		InitializeComponent();

        Loaded += LobbyPage_Loaded;
    }

    private void LobbyPage_Loaded(object? sender, EventArgs e)
    {
        ((FlagScreenModel)BindingContext).DispatcherTimer = Application.Current.Dispatcher.CreateTimer();
        if (((FlagScreenModel)BindingContext).DispatcherTimer != null)
        {
            ((FlagScreenModel)BindingContext).DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            ((FlagScreenModel)BindingContext).DispatcherTimer.Tick += (s, e) => RefreshFlags();
            ((FlagScreenModel)BindingContext).DispatcherTimer.Start();
        }
    }

    private void RefreshFlags()
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (await ((FlagScreenModel)BindingContext).SyncFlags())
            {
                if (!IsConnected)
                {
                    IsConnected = true;
                    IndicatorA.Fill = Colors.MidnightBlue;
                    IndicatorB.Fill = Colors.LightGreen;
                }
            } else
            {
                if (IsConnected)
                {
                    IndicatorA.Fill = Colors.Red;
                    IndicatorB.Fill = Colors.Orange;
                    IsConnected = false;
                }
            }

            var Brush = IndicatorB.Fill;
            IndicatorB.Fill = IndicatorA.Fill;
            IndicatorA.Fill = Brush;
        });
    }
}