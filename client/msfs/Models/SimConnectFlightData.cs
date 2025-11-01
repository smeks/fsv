using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Economy.FSX.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SimConnectFlightData
    {
        public int altitude;
        public int altitude_agl;
        public double latitude;
        public double longitude;
        public int onground;
        public int wheelRpm;
        public int parkingBreak;
        public int groundVelocity;
        public int cht1;
        public int cht2;
        public int cht3;
        public int cht4;
        public int mixture1;
        public int mixture2;
        public int mixture3;
        public int mixture4;
        public int rpm1;
        public int rpm2;
        public int rpm3;
        public int rpm4;
        public int combustion1;
        public int combustion2;
        public int combustion3;
        public int combustion4;
        public float totalFuel;
        public int zuluTime;
        public float rpmPercentage;
        public int timeOfDay;
        public int visibility;
        public float crosswind;
    }
}
