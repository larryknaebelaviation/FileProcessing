using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using spatial;


namespace flightPlanProcessing
{
    public class FlightPlan
    {

        private List<FlightPlanSegment> segmentlist = new List<FlightPlanSegment>();
        private FlightPlanSegment[] segmentarray = null;
        private Position startingposition = null;
        private int firstnavsegmentindex = -1;
        private int currentnavsegmentindex = -1;
        private int currentsegmentindex = 0;
        private int numNavSegments = 0;
        private int plansize = 0;

        
        public FlightPlan(string filepathname)
        {
            loadFromFile(filepathname);
            segmentarray = segmentlist.ToArray();
            setNumNavSegments();
            plansize = segmentarray.Length;
        }
        private void loadFromFile(string filepathname)
        {
            char[] delimiters = new char[] { ',' };
            //Console.WriteLine("filepathname:" + filepathname);
            
            using (StreamReader reader = new StreamReader(filepathname))
            //using (StreamReader reader = new StreamReader("C:\\Flight\\fp1.fpn"))
            {
                Boolean isFirstLine = true;
                int currentline = 0;
                while (true)
                {
                    FlightPlanSegment fps = null;
                    string line = reader.ReadLine();
                    //Console.WriteLine("line:" + line);
                    if (line == null )
                    {
                        break;
                    }
 
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    string[] parts = line.Split(delimiters);
                    //Console.WriteLine("Line:" + line + " Parts[0]:" + parts[0].ToString());

                    // comment record
                    if (parts[0] == "0")
                    {
                        fps = new FlightPlanSegment(Convert.ToInt16(parts[0]), parts[1]);
                    }
                    // normal record
                    else if (parts[0] == "1")
                    {
                        fps = new FlightPlanSegment(Convert.ToInt16(parts[0]), parts[1],Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3]), Convert.ToDouble(parts[4]), Convert.ToDouble(parts[5]));
                        if (firstnavsegmentindex < 0)
                        {
                            firstnavsegmentindex = currentline;
                        }
                    }
                    // starting position
                    else if (parts[0] == "2")
                    {
                        fps = new FlightPlanSegment(Convert.ToInt16(parts[0]), parts[1], Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3]));
                        startingposition = new Position(Convert.ToDouble(parts[2]), Convert.ToDouble(parts[3]),0d);
                    }

                    segmentlist.Add(fps);
                    currentline++;
                }
            }
        }

        public FlightPlanSegment getNextNavSegment()
        {
            if (currentnavsegmentindex < 0) // need to find the first one
            {
                currentnavsegmentindex = firstnavsegmentindex;
            }
                
            if ((currentnavsegmentindex) > (plansize - 1))
            {
                //return new FlightPlanSegment(0,"OVER INDEX CurrIdx:" + currentnavsegmentindex);
                return new FlightPlanSegment(0, "END OF FLIGHT PLAN");
            }
            else
            {
                //Console.WriteLine("CurNavSegIdx:{0} Outside Loop", currentnavsegmentindex);
                int c = currentnavsegmentindex;
                for (int i = c; i <= (plansize - 1); i++)
                {
                    if (i + 1 >= plansize)
                    {
                        //return new FlightPlanSegment(0, "OVER INDEX CurrIdx+1:" + currentnavsegmentindex + 1);
                        return new FlightPlanSegment(0, "END OF FLIGHT PLAN");
                    }
                    //Console.WriteLine("CurNavSegIdx:{0} Inside Loop", i);
                    if (segmentarray[i + 1].recordtype == 1) // peek ahead
                    {
                        currentnavsegmentindex = i + 1 ;
                        //Console.WriteLine("Found RecordType 1, Set Index to {0}", currentnavsegmentindex);
                        break;
                    }
                    //Console.WriteLine("Idx:{0}", i);
                }
                    
            }
    
            return segmentarray[currentnavsegmentindex];
        }

        private void setNumNavSegments()
        {
            foreach (FlightPlanSegment fps in segmentarray)
            {
                if (fps.recordtype == 1)
                {
                    numNavSegments++;
                }
            }
        }
        public int getSize()
        {
            return segmentlist.Count;
        }
        public Position getStartingPosition()
        {
            return startingposition;
        }

        public int getFirstNavSegmentIndex()
        {
            return firstnavsegmentindex;
        }
        
        public void setCurrentSegment(int i)
        {
            currentsegmentindex = i;
        }

        public int getCurrentSegment()
        {
            return currentsegmentindex;
        }
        public FlightPlanSegment getSegment(int i)
        {
            return segmentarray[i];
        }
        public void dump()
        {
            foreach (FlightPlanSegment seg in segmentlist)
            {
                if (seg.recordtype == 0)
                {
                    Console.WriteLine(seg.comment);
                }
                else if (seg.recordtype == 1)
                {
                    Console.WriteLine("comment:" + seg.comment + " speed:" + seg.speed + " heading:" + seg.heading
                        + " altitude:" + seg.altitudefeet + " minutes:" + seg.minutes);
                }
                else if (seg.recordtype == 2)
                {
                    Console.WriteLine("starting:" + seg.comment + " lat:" + seg.lat + " lon:" + seg.lon);
                }
            }
        }
    }
}
