 
## Quick Xamarin BLE
用於跨平台xamarin上快速訪問藍芽

![scan](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m3.jpg)
![read/write/notify](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m1.jpg)
![Characteristic list](https://github.com/4a0g0085/Quick.Xamarin.BLE/blob/master/src/m2.jpg)

## 使用限制

|平台  |限制  |
|--|--|
| xamarin.forms |  |
| xamarin.android   |API LEVEL>=18  |
| xamarin.ios | ios SDK>=7|
 
## 安裝
[**NuGet**](https://www.nuget.org/packages/Quick.Xamarin.BLE/)

	Install-Package Quick.Xamarin.BLE -Version 1.0.4
	
## 範例功能
由xamarin.forms製作的簡易藍芽搜尋範例，可支援android及ios，功能包含

 1. 搜尋藍芽
 2. 列出藍芽地址
 3. 藍芽連接/段開
 4. 尋找Service 和 Characteristic
 5. 列出Characteristic功能
 6. 讀出/寫入/通知 
## 權限
**Android**
AndroidManifest加入權限

    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-permission android:name="android.permission.BLUETOOTH" />
    <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />

## 開始
**初始化**

    IBle ble = CrossBle.Createble(); 
**搜尋藍芽**

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

**停止搜尋**

     ble.StopScanningForDevices();
 **藍芽連接**

     dev.ConnectToDevice();
  **藍芽段開**
  

    dev.DisconnectDevice();

 **目前藍芽連線狀態回傳**
 

     ble.AdapterStatusChange += (sender,Status) =>
     {
		 Device.BeginInvokeOnMainThread(() =>
	     { 
			 if (Status == AdapterConnectStatus.Connected)//Connect
			 if (Status == AdapterConnectStatus.None)//disconnect
	     });
     };

   
 **取得藍芽Characteristics特性**

      List<IGattCharacteristic> cha= new List<IGattCharacteristic>();
     dev.CharacteristicsDiscovered(value=>
     {  
	     Device.BeginInvokeOnMainThread(() =>
	     {   
		     //save value to list and use
		     cha.Add(chvaluea); 
	     });
     });
**取得Characteristics可使用功能**

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
## 注意

 - event 須加上Device.BeginInvokeOnMainThread
 - 避免藍芽連接後馬上取得特性，需延遲1-2秒
 - 藍芽一次只能連接一台，需Disconnect後才能連接其他台
 - 
