using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using System.IO.Ports;
using System.Management;

namespace VUmeter
{
    class Program
    {

        static void Main(string[] args)
        {





            string esp8266DeviceName = "USB-SERIAL CH340"; // Change this to match your ESP8266 device name

            string comPort = FindESP8266COMPort(esp8266DeviceName);

            if (!string.IsNullOrEmpty(comPort))
            {
                Console.WriteLine($"ESP8266 is connected to COM port: {comPort}");
                MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
                SerialPort _serialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
                _serialPort.Handshake = Handshake.None;
                _serialPort.Open();


                var selectedDevice = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);

                _serialPort.WriteLine("1000");
                System.Threading.Thread.Sleep(1500);
                while (true)
                {
                    //Not necessary to increase the value explicitly, it's only to make it a bit more spectacular
                    int val = Convert.ToInt32(selectedDevice.AudioMeterInformation.MasterPeakValue * 100) + 5;
                    if (val > 80) val = 80;
                    Console.WriteLine(val);
                    _serialPort.WriteLine(val.ToString());
                    System.Threading.Thread.Sleep(15); //Optional, it feels more synchronized with my bluetooth headset
                }
            }
            else
            {
                Console.WriteLine("ESP8266 is not found on any COM port.");
            }
            Console.ReadLine();
        }


        static string FindESP8266COMPort(string deviceName)
        {
            string comPort = null;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject device in collection)
                {
                    object nameProperty = device["Name"];
                    if (nameProperty != null && nameProperty.ToString().Contains(deviceName))
                    {
                        object captionProperty = device["Caption"];
                        if (captionProperty != null)
                        {
                            string caption = captionProperty.ToString();
                            int startIndex = caption.LastIndexOf("(COM");
                            if (startIndex != -1)
                            {
                                int endIndex = caption.LastIndexOf(")");
                                if (endIndex != -1)
                                {
                                    comPort = caption.Substring(startIndex + 1, endIndex - startIndex - 1);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return comPort;
        }

    }
}
