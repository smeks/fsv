using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Interop;
using AutoMapper;
using Economy.Domain.IManager;
using Economy.Domain.Models;
using Economy.FSX.Models;
using Microsoft.FlightSimulator.SimConnect;
using System.Windows.Threading;
using Economy.Domain.Enum;

namespace Economy.FSX
{
    public class SimConnectManager : ISimConnectManager
    {
        public List<string> Events = new List<string>();
        public Action<FlightData> FlightDataUpdated { get; set; }
        public Action<AircraftSpecs> AircraftDataUpdated { get; set; }
        public Action<SimStatus> StatusUpdated { get; set; }
        public IntPtr WindowHandle { get; set; }
        

        private readonly IMapper _mapper;
        const int WmUserSimconnect = 0x0402;
        private FlightData FlightData { get; set; }
        private AircraftSpecs AircraftData { get; set; }
        private SimConnect _connection;
        private HwndSource _hws;
        private static SimStatus _simConnectStatus = SimStatus.Disconnected;
        private static SimStatus _simConnectLastStatus = SimStatus.Disconnected;
        private readonly Thread _connectionThread;

        public SimConnectManager()
        {
            _mapper = new MapperConfiguration(mapper => mapper.AddProfile(new SimConnectMappingProfile())).CreateMapper();

            // constantly retry connecting to fsx/p3d
            _connectionThread = new Thread(TryConnect);
            _connectionThread.Start();

            // tick timer
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(500),
                IsEnabled = true
            };
            timer.Tick += (sender, args) =>
            {
                if (_simConnectLastStatus == _simConnectStatus)
                    return;

                _simConnectLastStatus = _simConnectStatus;
                StatusUpdated.Invoke(_simConnectLastStatus);
            };
            timer.Start();
        }


        private void TryConnect()
        {
            while (true)
            {
                if (_simConnectStatus == SimStatus.Connected ||
                    _simConnectStatus == SimStatus.Connecting ||
                    WindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                    
                try
                {
                    Connect();
                }
                catch (COMException ex)
                {
                    SetStatus(SimStatus.Disconnected);
                }
            }
        }

        private void SetStatus(SimStatus status)
        {
            _simConnectStatus = status;
        }

        public void Connect()
        {
            SetStatus(SimStatus.Connecting);

            if (_hws == null)
            {
                _hws = HwndSource.FromHwnd(WindowHandle);
                _hws.AddHook(ProcessSimConnectWin32Events);
            }

            _connection = new SimConnect("Flight Sim Economy", _hws.Handle, WmUserSimconnect, null, 0);
            _connection.OnRecvEvent += OnEvent;
            _connection.OnRecvSimobjectData += OnData;
            _connection.OnRecvException += OnException;
            _connection.OnRecvOpen += OnOpen;
            _connection.OnRecvQuit += OnQuit;

            #region Register AIRCRAFT_SPECS
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Plane latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Plane longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Number of engines", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Engine type", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank center capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank center2 capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank center3 capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank left main capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank left aux capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank left tip capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank right main capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank right aux capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank right tip capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank external1 capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Fuel tank external2 capacity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Empty Weight", "kg", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "max gross weight", "kg", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Estimated cruise speed", "knots", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Estimated Fuel Flow", "gallon per hour", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_SPECS, "Payload station count", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.RegisterDataDefineStruct<SimConnectAircraftSpecs>(DATA_DEF.AIRCRAFT_SPECS);
            #endregion

            #region Register AIRCRAFT_SPECS
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Plane altitude", "feet", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Plane alt above ground", "feet", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Plane latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Plane longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Sim on ground", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Wheel RPM", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Brake parking indicator", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Ground Velocity", "knots", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Recip eng cylinder head temperature:1", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Recip eng cylinder head temperature:2", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Recip eng cylinder head temperature:3", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Recip eng cylinder head temperature:4", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng mixture lever position:1", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng mixture lever position:2", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng mixture lever position:3", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng mixture lever position:4", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Prop RPM:1", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Prop RPM:2", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Prop RPM:3", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Prop RPM:4", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng combustion:1", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng combustion:2", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng combustion:3", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "General eng combustion:4", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Fuel total quantity", "gallons", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Zulu time", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Prop Max RPM Percent:1", null, SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Time of day", null, SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Ambient Visibility", "meters", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.AddToDataDefinition(DATA_DEF.FLIGHT_DATA, "Aircraft wind X", "knots", SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.RegisterDataDefineStruct<SimConnectFlightData>(DATA_DEF.FLIGHT_DATA);


            ToggleDataFeeds(true);

            #endregion
        }

        public void Disconnect()
        {
            ToggleDataFeeds(false);
        }

        public void SetWeight(int weight)
        {
            var payloadPerStation = weight / AircraftData.NumPayloadStations;
            for (var c = 0; c < AircraftData.NumPayloadStations; c++)
                setPayloadStation(c + 1, payloadPerStation);
        }

        private void ToggleDataFeeds(bool enabled)
        {
            var period = enabled ? SIMCONNECT_PERIOD.SECOND : SIMCONNECT_PERIOD.NEVER;
            _connection.RequestDataOnSimObject(DATA_REQUESTID.FLIGHT_DATA, DATA_DEF.FLIGHT_DATA, SimConnect.SIMCONNECT_OBJECT_ID_USER, period, SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT, 0, 0, 0);
            _connection.RequestDataOnSimObject(DATA_REQUESTID.AIRCRAFT_SPECS, DATA_DEF.AIRCRAFT_SPECS, SimConnect.SIMCONNECT_OBJECT_ID_USER, period, SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT, 0, 0, 0);
        }

        private void setPayloadStation(int station, int weight)
        {
            SimConnectPayload payload = new SimConnectPayload(weight);
            _connection.ClearDataDefinition(DATA_DEF.AIRCRAFT_PAYLOAD);
            _connection.AddToDataDefinition(DATA_DEF.AIRCRAFT_PAYLOAD, $"Payload station weight:{station}", "kg", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            _connection.SetDataOnSimObject(DATA_DEF.AIRCRAFT_PAYLOAD, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_DATA_SET_FLAG.DEFAULT, payload);
        }

        private void OnQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            SetStatus(SimStatus.Disconnected);
            _connection.Dispose();
            _connection = null;
            this.Events.Add("OnQuit");
        }

        private void OnOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            SetStatus(SimStatus.Connected);
            this.Events.Add("OnOpen");
        }

        private void OnException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            this.Events.Add("OnException");
        }

        private async void OnData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            
            try
            {
                switch ((DATA_REQUESTID)data.dwRequestID)
                {
                    case DATA_REQUESTID.FLIGHT_DATA:
                        var flightData = (SimConnectFlightData)data.dwData[0];
                        FlightData = _mapper.Map<FlightData>(flightData);
                        FlightDataUpdated.Invoke(FlightData);
                        break;

                    case DATA_REQUESTID.AIRCRAFT_SPECS:
                        var aircraftData = (SimConnectAircraftSpecs)data.dwData[0];

                        // changed aircraft or moved location
                        var differentAircraft = AircraftData != null && AircraftData.Title != aircraftData.title;
                        var pos1 = AircraftData != null ? Math.Abs(AircraftData.Latitude - aircraftData.latitude) : 0f;
                        var pos2 = AircraftData != null ? Math.Abs(aircraftData.longitude - aircraftData.longitude) : 0f;
                        var aircraftMoved = AircraftData != null && (pos1 > 0.1f || pos2 > 0.1f);
                        if (AircraftData == null || differentAircraft || aircraftMoved)
                        {
                            AircraftData = _mapper.Map<AircraftSpecs>(aircraftData);
                            AircraftDataUpdated.Invoke(AircraftData);
                        }
                        
                        break;
                }

                this.Events.Add("OnData");
            }
            catch (Exception e)
            {
                this.Events.Add(e.Message);
            }
        }

        private void OnEvent(SimConnect sender, SIMCONNECT_RECV_EVENT data)
        {
            this.Events.Add("OnEvent");
        }

        private IntPtr ProcessSimConnectWin32Events(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            _connection?.ReceiveMessage();
            return (IntPtr)0;
        }
    }
}
