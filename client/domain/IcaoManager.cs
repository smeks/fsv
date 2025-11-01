using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Economy.Domain.IManager;
using Economy.Domain.Models;
using Newtonsoft.Json;

namespace Economy.Domain
{
    public class IcaoManager : IIcaoManager
    {
        private List<Icao> Airports;

        public IcaoManager()
        {
            //string icodata = System.IO.File.ReadAllText(@"icaodata.json");
            //Airports = JsonConvert.DeserializeObject<List<Icao>>(icodata);
            //Console.Out.Write($"{Airports.Count}");
        }

        public Icao GetIcaoByIcao(string icao)
        {
            return Airports.SingleOrDefault(x => x.ICAO == icao);
        }

        public Icao GetCurrentLocation(double lat, double lon)
        {
            var lat_min = lat - 5 / 69.0;
            var lat_max = lat + 5 / 69.0;

            var filtered = Airports.Where(x => x.Lat > lat_min && x.Lat < lat_max);

            Icao nearest = null;
            double nearestDistance = 999.0;
            foreach (var icao in filtered)
            {
                var distance = this.distance(lat, lon, icao.Lat, icao.Lon, 'K');
               
                if (!(distance < nearestDistance)) continue;

                nearest = icao;
                nearestDistance = distance;
            }

            return nearest;
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((Math.Abs(lat1 - lat2) < 0.01) && (Math.Abs(lon1 - lon2) < 0.01))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
