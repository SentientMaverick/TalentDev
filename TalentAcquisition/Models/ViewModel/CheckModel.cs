using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.Models.Core
{
    public class CheckModel
    {
            public int Id
            {
                get;
                set;
            }
            public string Name
            {
                get;
                set;
            }
            public bool Checked
            {
                get;
                set;
            }
    }
    public class Product
    {
        public string Index
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public decimal Price
        {
            get;
            set;
        }
    }
}