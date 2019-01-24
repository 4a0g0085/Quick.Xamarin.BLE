 
## ![enter image description here](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/BLEicon.png)Quick Xamarin BLE
quick access  Bluetooth Low Energy on xamarin Forms Android and IOS

![scan](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m3.jpg)
![read/write/notify](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m1.jpg)
![Characteristic list](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m2.jpg)

##  Limitations

|Platform  |Version|
|--|--|
| xamarin.forms |  |
| xamarin.android   |API LEVEL>=18  |
| xamarin.ios | ios SDK>=7|
 
##  Installation
 
[**NuGet**](https://www.nuget.org/packages/Quick.Xamarin.BLE/)

	Install-Package Quick.Xamarin.BLE -Version 1.0.4
	
## Sample
 Simple bluetooth search example. created by xamarin.forms  
 
 **key features include**
 1. Scan Bluetooth 
 2. List Bluetooth addresses
 3. Bluetooth  connect/disconnect
 4. Search service and  characteristic
 5. List characteristic features
 6. Read/Write/Notify
## Permission
**Android**
AndroidManifest add permission 

    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

## Start
**Initialization**

    IBle ble = CrossBle.Createble(); 
**Scan Bluetooth**

     ble.OnScanDevicesIn += (sender, device) =>
     {
         Device.BeginInvokeOnMainThread(() =>
         { 
              //use device.Name or Uuid or Rssi or connect dev
              IDev dev=device;
         });
     };
     //start scan
     ble.StartScanningForDevices();

**Stop scan**

     ble.StopScanningForDevices();
 **Device connect**

     dev.ConnectToDevice();
  **Device disconnect**
  

    dev.DisconnectDevice();

 **Bluetooth device Status callback event**
 

     ble.AdapterStatusChange += (sender,Status) =>
     {
		 Device.BeginInvokeOnMainThread(() =>
	     { 
			 if (Status == AdapterConnectStatus.Connected)//Connect
			 if (Status == AdapterConnectStatus.None)//disconnect
	     });
     };

   
 **Get device characteristics**

      List<IGattCharacteristic> cha= new List<IGattCharacteristic>();
     dev.CharacteristicsDiscovered(value=>
     {  
	     Device.BeginInvokeOnMainThread(() =>
	     {   
		     //save value to list and use
		     cha.Add(chvaluea); 
	     });
     });
**Get  characteristics properties**

    cha[n].CanRead();
    cha[n].CanWrite();
    cha[n].CanNotify();

  
 **Read**
 
	//Call once
    Search.ble.ServerCallBackEvent += (uuid, value) => 
    {
    
    };
    
    cha[n].ReadCallBack();
	
or

    var value=await cha[n].ReadCallBack();

 **Write**
 

      cha[n].Write(byte[]);

  **Notify**

     cha[n].NotifyEvent += SelectCharacteristic_NotifyEvent;
     cha[n].Notify();

  **Stop Notify**
  

    enter code here
     cha[n].StopNotify();
     cha[n].NotifyEvent -= SelectCharacteristic_NotifyEvent;
     
  **Notify Event**
  

     private void SelectCharacteristic_NotifyEvent(object sender, byte[] value)
     {
         Device.BeginInvokeOnMainThread(() => { 
         });
     }
## Important limitations

 - Event must be added Device.BeginInvokeOnMainThread
 - Characteristicsmust be 1-2 seconds after the Bluetooth connection
 - Bluetooth can only connect one at a time,When you want to connect other,Must call DisconnectDevice()  first
 
