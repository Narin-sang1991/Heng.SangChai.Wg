using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Collections;
using System.Management;

namespace Cet.Core.Utility
{
    public class HardwareInfo
    {
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public static string GetComputerName()
        {
            var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_ComputerSystem");

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                // Display the remote computer information
                //Console.WriteLine("Computer Name     : {0}", m["csname"]);
                //Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
                //Console.WriteLine("Operating System  : {0}", m["Caption"]);
                //Console.WriteLine("Version           : {0}", m["Version"]);
                //Console.WriteLine("Manufacturer      : {0}", m["Manufacturer"]);
                return m["Name"].ToString();
            }
            return string.Empty;
        }

        public static string GetHarddiskSerialNo()
        {
            ArrayList hdCollection = new ArrayList();

            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive Where DeviceID = '\\\\\\\\.\\\\PHYSICALDRIVE0'");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                //HardDrive hd = new HardDrive();
                //var Device = wmi_HD["DeviceID"].ToString();
                //hd.Model = wmi_HD["Model"].ToString();
                //hd.Type = wmi_HD["InterfaceType"] == null ? "Unknown" : wmi_HD["InterfaceType"].ToString();
                //if (hd.Type == "IDE")
                //{
                //hd.SerialNo = wmi_HD["SerialNumber"].ToString();
                //return hd.SerialNo;
                return wmi_HD["SerialNumber"].ToString();
                //}              
            }
            return string.Empty;
        }


        public static string GetCPUSerialNo()
        {

            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM win32_processor Where DeviceID = 'CPU0'");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                return wmi_HD["processorID"].ToString();
            }

            return string.Empty;
        }
    }


    public class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
    }

}
