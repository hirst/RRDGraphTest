using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

using RrdSharp.Core;
using RrdSharp.Graph;

namespace RRDGraphTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Log database location
            string path = "C:\\Documents and Settings\\Aimee\\My Documents\\logs\\Field.rrd";

            // Time range
            DateTime startTime = new DateTime(2012, 2, 7, 12, 00, 0);
            DateTime endTime = new DateTime(2012, 2, 15, 12, 00, 0);

            // Y Axis ranges
            double lower = -70;
            double upper = 70;
            double error_lower = -0.05;
            double error_upper = 0.05;


            createVectorGraphPNG(path, "current.png", "I", startTime, endTime, "Amps");
            createVectorGraphPNG(path, "error.png", "BE", startTime, endTime, "uT", error_lower, error_upper);
            createVectorGraphPNG(path, "setpoint.png", "BS", startTime, endTime, "uT", lower, upper);
            createVectorGraphPNG(path, "centre.png", "BC", startTime, endTime, "uT", lower, upper);
             createVectorGraphPNG(path, "sensor1.png", "B1", startTime, endTime, "uT", lower, upper);
            createVectorGraphPNG(path, "sensor2.png", "B2", startTime, endTime, "uT", lower, upper);
            createVectorGraphPNG(path, "sensor3.png", "B3", startTime, endTime, "uT", lower, upper);
        }

        private static void createVectorGraphPNG(string rrdPath, string pngPath, string ds, DateTime start, DateTime end, string vertLabel, double lower = 0, double upper = 0)
        {
            RrdGraphDef graphDef = new RrdGraphDef();
            graphDef.SetTimePeriod(getUnixTimeStamp(start), getUnixTimeStamp(end));
            graphDef.VerticalLabel = vertLabel;

            if (lower != upper)
            {
                graphDef.SetGridRange(lower, upper, true);
            }

            string dsx = ds + ".X";
            string dsy = ds + ".Y";
            string dsz = ds + ".Z";

            graphDef.Datasource(dsx, rrdPath, dsx, "AVERAGE");
            graphDef.Datasource(dsy, rrdPath, dsy, "AVERAGE");
            graphDef.Datasource(dsz, rrdPath, dsz, "AVERAGE");

            graphDef.Line(dsx, Color.Blue, dsx, 2);
            graphDef.Line(dsy, Color.Green, dsy, 2);
            graphDef.Line(dsz, Color.Red, dsz, 2);

            RrdGraph graph = new RrdGraph(graphDef);
            graph.SaveAsPNG(pngPath, 800, 600);
        }

        private static long getUnixTimeStamp(DateTime time)
        {
            return Convert.ToInt32((time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }
    }
}
