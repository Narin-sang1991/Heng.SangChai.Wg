using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.Hw.Core.AppService
{
    public class ActivateSoftwareService
    {
        IUnityContainer container;

        public ActivateSoftwareService(IUnityContainer container)
        {
            this.container = container;
        }

        public ActivateStatus ValidateActivationFromRegistry()
        {
            // Status 
            // 0 = Activate
            // 1 = Not Activate
            // 2 = DataVerify Incorect;
            string _PublicKey = "<RSAKeyValue><Modulus>iG6J+TpjIy3nTfQ26TzvSaENsFSFSMbPd3e9FJvMGyidUt2WPqUzSWgDrMECAg5LYk3L6F/kjf7c+ruHwbjSlmHU1cgs1+/zAdAVGr5kFzWGYmcrAcYMbev0upNn5MhfQBQiNKln9/+JQanmg+nbvCLWNXcURHDAlUd8DXhBphE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSAParameters _publicKey;
            DigitalSignature.GeyRSAFromXML2(_PublicKey, out _publicKey);
            DigitalSignature.ReadActivationCodeFromRegistry(out activationCode, out activationUser);
            var thisHardwareInfo = GETData();

            ActivateStatus Status = ActivateStatus.DataVerifyIncorect;

            string ActivateRegis = string.Format("{0}<Activate>{1}<Activate>", thisHardwareInfo.CPUNo, thisHardwareInfo.HarddiskNo);

            if (!string.IsNullOrWhiteSpace(activationCode))
            {
                if (DigitalSignature.VerifyDataPass(ActivateRegis, activationCode, _publicKey))
                    Status = ActivateStatus.Activate;
                else
                    Status = ActivateStatus.DataVerifyIncorect;
            }
            else
                Status = ActivateStatus.NotActivate;

            return Status;
        }

        public ActivationData GETData()
        {
            var activateData = new ActivationData();
            //activateData.ActivationCode = this.ActivationCode;
            activateData.CPUNo = HardwareInfo.GetCPUSerialNo();
            activateData.HarddiskNo = HardwareInfo.GetHarddiskSerialNo();
            // activateData.MacAddress = //HardwareInfo.GetMACAddress();
            //string ActivatePack; string ActCode; string ExpireDate; string ActivationUser;
            //ReadActivationCodeFromRegistry(out ActivatePack, out ActCode, out ExpireDate, out ActivationUser);
            //activateData.ActivationUser = ActivationUser;

            return activateData;
        }
    }


    public class ActivationData
    {
        public String ActivationCode_Old { get; set; }
        public String ActivationCode { get; set; }
        public String HarddiskNo { get; set; }
        public String CPUNo { get; set; }
        public String MacAddress { get; set; }
        public String ActivationUser { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public enum ActivateStatus
    {
        Activate = 0,
        NotActivate = 1,
        DataVerifyIncorect = 2
    }
}
