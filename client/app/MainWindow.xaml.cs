using System;
using System.Windows;
using System.Windows.Media;
using Economy.Client.DTO;
using Economy.Domain;
using Economy.Domain.Enum;
using FontAwesome.WPF;

namespace Economy.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFlightManager _flightManager;
        private PlayerDC player;

        public MainWindow(IFlightManager flightManager)
        {
            _flightManager = flightManager;
            InitializeComponent();

            StatusTextValue.Text = "Flight Simulator Disconnected";
            EconomyStatusTextValue.Text = "Economy Disconnected";

            _flightManager.StatusUpdated += s =>
            {
                switch (s)
                {
                    case SimStatus.Connected:
                        StatusIconValue.Icon = FontAwesomeIcon.ToggleOn;
                        StatusIconValue.Foreground = Brushes.Green;
                        StatusTextValue.Text = "Flight Simulator Connected";
                        break;
                    default:
                        StatusIconValue.Icon = FontAwesomeIcon.ToggleOff;
                        StatusIconValue.Foreground = Brushes.Red;
                        StatusTextValue.Text = "Flight Simulator Disconnected";
                        break;
                }
            };

            _flightManager.EconomyStatusUpdated += status =>
            {
                switch (status)
                {
                    case EconomyStatus.Connected:
                        EconomyStatusIconValue.Foreground = Brushes.Green;
                        EconomyStatusTextValue.Text = "Economy Connected";

                        LoginGroupBox.Visibility = Visibility.Hidden;
                        FlightStatusGroupBox.Visibility = Visibility.Visible;
                        MyHangerGroupBox.Visibility = Visibility.Visible;
                        break;
                    default:
                        EconomyStatusIconValue.Foreground = Brushes.Red;
                        EconomyStatusTextValue.Text = "Economy Disconnected";
                        break;
                }
            };

            _flightManager.AircraftUpdated += specs =>
            {
                if (specs.IsPlaneCompatible)
                {
                    CurrentAircraftIconValue.Icon = FontAwesomeIcon.CheckCircleOutline;
                    CurrentAircraftIconValue.Foreground = Brushes.Green;
                    CurrentAircraftValue.Text = specs.Title;
                }
                else
                {
                    CurrentAircraftIconValue.Icon = FontAwesomeIcon.ExclamationCircle;
                    CurrentAircraftIconValue.Foreground = Brushes.Red;
                    CurrentAircraftValue.Text = $"{specs.Title} is an unsupported aircraft";
                }

                if (specs.IsRented)
                {
                    RentedAircraftIconValue.Icon = FontAwesomeIcon.CheckCircleOutline;
                    RentedAircraftIconValue.Foreground = Brushes.Green;
                    RentedAircraftTextValue.Text = $"{specs.Title} is rented and ready to fly";
                }
                else
                {
                    RentedAircraftIconValue.Icon = FontAwesomeIcon.ExclamationCircle;
                    RentedAircraftIconValue.Foreground = Brushes.Red;
                    RentedAircraftTextValue.Text = $"{specs.Title} is not rented at current location";
                }

                if (specs.IsPlaneCompatible && specs.IsRented)
                {
                    StartFlightButton.Visibility = Visibility.Visible;
                    EndFlightButton.Visibility = Visibility.Hidden;
                }
            };

            _flightManager.LocationUpdated += icao =>
            {
                CurrentLocationIconValue.Icon = FontAwesomeIcon.CheckCircleOutline;
                CurrentLocationIconValue.Foreground = Brushes.Green;
                CurrentLocationValue.Text = $"{icao.ICAO} - {icao.Name} - {icao.City}, {icao.State} {icao.Country}";
            };

            _flightManager.FlightStatusUpdated += flightStatus =>
            {
                switch (flightStatus)
                {
                    case PlayerFlightStatus.Ground:
                        StartFlightButton.Visibility = Visibility.Visible;
                        EndFlightButton.Visibility = Visibility.Hidden;
                        break;
                    case PlayerFlightStatus.Enroute:
                        StartFlightButton.Visibility = Visibility.Hidden;
                        EndFlightButton.Visibility = Visibility.Visible;
                        break;
                }
            };
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var userName = UsernameTextValue.Text;
            var password = PasswordTextValue.Text;
            await _flightManager.Login(userName, password);
        }

        private void StartFlightButton_OnClick(object sender, RoutedEventArgs e)
        {
            _flightManager.StartFlight();
        }

        private void EndFlightButton_OnClick(object sender, RoutedEventArgs e)
        {
            _flightManager.EndFlight();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
        }
    }
}
