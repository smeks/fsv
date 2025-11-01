using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Economy.FSX.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SimConnectPayload
    {
        public int stationx;

        public SimConnectPayload(int sx)
        {
            this.stationx = sx;
        }
    }
}
