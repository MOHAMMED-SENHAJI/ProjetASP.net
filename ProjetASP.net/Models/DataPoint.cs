using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProjetASP.net.Models
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string label, int y)
        {
            this.Label = label;
            this.Y = y;
        }

        public DataPoint(string v1, object v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
        private string v1;
        private object v2;
    }
}