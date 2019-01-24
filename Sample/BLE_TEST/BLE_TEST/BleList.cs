using Quick.Xamarin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BLE_TEST
{
    public class BleList  
    {
        public string Name { get; set; }
        public string Uuid { get; set; }
      
        public BleList(string name,string uuid )
        {
          
            Uuid = uuid;
            Name = name;
        }

    }
}
