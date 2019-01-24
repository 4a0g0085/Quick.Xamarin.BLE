using Quick.Xamarin.BLE;
using Quick.Xamarin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BLE_TEST
{
    public partial class Service : ContentPage
    {
        public static AdapterConnectStatus BleStatus;
        List<IGattCharacteristic> AllCharacteristics = new List<IGattCharacteristic>();
        IGattCharacteristic SelectCharacteristic = null;
        ObservableCollection<CharacteristicsList> CharacteristicsList = new ObservableCollection<CharacteristicsList>();
        bool isnotify = false;
        public Service()
        {
            InitializeComponent();
            Search.ble.AdapterStatusChange += Ble_AdapterStatusChange;
            Search.ble.ServerCallBackEvent += Ble_ServerCallBackEvent;
            listView.ItemsSource = CharacteristicsList; 
            
        }

        private void Ble_ServerCallBackEvent(string uuid, byte[] value)
        {
            Device.BeginInvokeOnMainThread(() => {
                if (SelectCharacteristic != null)
                {
                    if (SelectCharacteristic.Uuid == uuid)
                    {
                        string str = BitConverter.ToString(value);
                        info_read.Text = "CallBack UUID:" + str;
                    }
                }
            });

        }

        private void Ble_AdapterStatusChange(object sender, AdapterConnectStatus e)
        {
            Device.BeginInvokeOnMainThread(async () => {
               Search.BleStatus = e;
                if(Search.BleStatus== AdapterConnectStatus.Connected)
                {
                    msg_txt.Text = "Success";
                    await Task.Delay(3000);
                    msg_layout.IsVisible = false;
                    listView.IsVisible = true;
                    ReadCharacteristics();
              
                }
                if (Search.BleStatus == AdapterConnectStatus.None)
                {
                  await  Navigation.PopToRootAsync(true);
                }
            });
        }
        void ReadCharacteristics()
        {
            Search.ConnectDevice.CharacteristicsDiscovered(cha =>
            {  
                Device.BeginInvokeOnMainThread(() => {
                    AllCharacteristics.Add(cha);
                    CharacteristicsList.Add(new CharacteristicsList( cha.Uuid,cha.CanRead(), cha.CanWrite(), cha.CanNotify()));
                });
            } );
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var select = (CharacteristicsList)e.Item;
            foreach (var c in AllCharacteristics)
            { 
                if (c.Uuid == select.Uuid)
                {
                    SelectCharacteristic = c;
                    info_uuid.Text ="UUID:"+SelectCharacteristic.Uuid;
                    info_read.Text = "CallBack UUID:";
                    notify_btn.Text = "Notify";
                    if (SelectCharacteristic.CanRead()) read_btn.IsVisible = true;
                    else read_btn.IsVisible = false;
                    if (SelectCharacteristic.CanWrite()) write_btn.IsVisible = true;
                    else write_btn.IsVisible = false;
                    if (SelectCharacteristic.CanNotify()) notify_btn.IsVisible = true;
                    else notify_btn.IsVisible = false;
                    background.IsVisible = true;
                    info.IsVisible = true;
                    break;
                }
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Search.ble.AdapterStatusChange -= Ble_AdapterStatusChange;
            Search.ble.ServerCallBackEvent -= Ble_ServerCallBackEvent;
            if (Search.ConnectDevice!=null) Search.ConnectDevice.DisconnectDevice();
        }

        private void background_click(object sender, EventArgs e)
        {
            background.IsVisible = false;
            info.IsVisible = false;
            SelectCharacteristic = null;
            if (isnotify)
            {
                SelectCharacteristic.StopNotify();
                SelectCharacteristic.NotifyEvent -= SelectCharacteristic_NotifyEvent;
            }
        }

        private void read_Clicked(object sender, EventArgs e)
        {
            if (SelectCharacteristic != null)
            {
                SelectCharacteristic.ReadCallBack();
            }
        }
        private void write_Clicked(object sender, EventArgs e)
        {
           var bytearray= StringToByteArray(info_write.Text);
            if (bytearray == null)
            {
                DisplayAlert("", "Input format error", "ok");
                return;
            }
            SelectCharacteristic.Write(bytearray);

        }
        private void notify_Clicked(object sender, EventArgs e)
        {
            if (SelectCharacteristic != null)
            {
                if (notify_btn.Text.ToLower() == "notify")
                {
                    isnotify = true;
                    notify_btn.Text = "Stop Notify";
                    SelectCharacteristic.NotifyEvent += SelectCharacteristic_NotifyEvent;
                    SelectCharacteristic.Notify();
                }
                else
                {
                    isnotify = false;
                    notify_btn.Text = "Notify";
                    SelectCharacteristic.StopNotify();
                    SelectCharacteristic.NotifyEvent -= SelectCharacteristic_NotifyEvent;
                }
               
            }
        }

        private void SelectCharacteristic_NotifyEvent(object sender, byte[] value)
        {
            Device.BeginInvokeOnMainThread(() => {
                string str = BitConverter.ToString(value);
                info_read.Text = "CallBack UUID:" + str;
            });
        }
       
        public static byte[] StringToByteArray(string hex)
        {
            try
            {
                return Enumerable.Range(0, hex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                                 .ToArray();
            }
            catch{ return null; }
        }
    }
}
