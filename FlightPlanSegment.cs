using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace flightPlanProcessing
{
    public class FlightPlanSegment
    {
        public int recordtype;
        public string comment;
        public double speed;
        public double heading;
        public double altitudefeet;
        public double minutes;
        public double lat;
        public double lon;
      

        public FlightPlanSegment(int recordtype, string comment, double speed,double heading,double altitudefeet,double minutes)
        {
            this.recordtype = recordtype;
            this.comment = comment;
            this.speed = speed;
            this.heading = heading;
            this.altitudefeet = altitudefeet;
            this.minutes = minutes;
            this.lat = 0d;
            this.lon = 0d;
            
        }

        public FlightPlanSegment(int recordtype, string comment)
        {
            this.recordtype = recordtype;
            this.comment = comment;
            this.speed = 0d;
            this.heading = 0d;
            this.altitudefeet = 0d;
            this.minutes = 0d;
            this.lat = 0d;
            this.lon = 0d;

        }

        public FlightPlanSegment(int recordtype,string icaocode,double lat, double lon)
        {
            this.recordtype = recordtype;
            this.comment = icaocode;
            this.speed = 0d;
            this.heading = 0d;
            this.altitudefeet = 0d;
            this.minutes = 0d;
            this.lat = lat;
            this.lon = lon;


        }

        public override string ToString()
        {
            String sval = "BAD DATA";
            if (recordtype == 0) // comment
            {
                sval = new String(recordtype.ToString() + "," + comment.ToString());
            }
            else if (recordtype == 1) // normal
            {
                sval = new String(recordtype.ToString() + "," + comment.ToString() + "," + speed.ToString() + "," 
                    + heading.ToString() + "," + altitudefeet.ToString() + "," + minutes.ToString());
            }
            else if (recordtype == 2) // lat, lon
            {
                sval = new string(recordtype.ToString() + "," + comment.ToString() + lat.ToString() + "," + lon.ToString());
            }
            return sval;
        }


    }
}
