using System.Runtime.InteropServices;

namespace Economy.FSX.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SimConnectAircraftSpecs
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string title;
        public double latitude;
        public double longitude;
        public int numEngines;
        public int engineType;
        public float fuelCenter;
        public float fuelCenter2;
        public float fuelCenter3;
        public float fuelLeftMain;
        public float fuelLeftAux;
        public float fuelLeftTip;
        public float fuelRightMain;
        public float fuelRightAux;
        public float fuelRightTip;
        public float fuelExt1;
        public float fuelExt2;
        public int emptyWeight;
        public int maxGrossWeight;
        public int estimateCruise;
        public float estimateFuelFlow;
        public int numPayloadStations;
    }
}
