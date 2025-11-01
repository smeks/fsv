using System;
using Economy.Domain.Enum;
using Economy.Domain.Models;

namespace Economy.Domain.IManager
{
    public interface ISimConnectManager
    {
        void Connect();
        void SetWeight(int weight);
        Action<FlightData> FlightDataUpdated { get; set; }
        Action<AircraftSpecs> AircraftDataUpdated { get; set; }
        IntPtr WindowHandle { get; set; }
        Action<SimStatus> StatusUpdated { get; set; }
    }
}