using Cet.SmartClient.Client;
using H.SangChai.WgRecords.Shell.Enums;
using H.SangChai.WgRecords.Shell.Models;
using Microsoft.Practices.Unity;
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace H.SangChai.WgRecords.Shell.IOModels
{
    public class RS232SerialPortControlBase : ViewModelBase
    {
        public IUnityContainer Container { get; set; }

        public RS232SerialPortControlBase() { }

        public RS232SerialPortControlBase(IUnityContainer iContainer)
        {
            this.Container = iContainer;
        }

        #region Connect RS232 Port
        #region PortConfig

        protected SerialPort serialPort;         //<-- declares a SerialPort Variable to be used throughout the form
        protected bool _continue;
        Thread readThread;

        public string ComportStatus
        {
            get { return serialPort != null ? (serialPort.IsOpen ? "ON" : "OFF") : "NONE"; }
        }

        private string portName;
        public string PortName
        {
            get { return portName; }
            private set
            {
                portName = value;
                OnPropertyChanged("PortName");
            }
        }

        private int baudRate;
        public int BaudRate
        {
            get { return baudRate; }
            private set
            {
                baudRate = value;
                OnPropertyChanged("BaudRate");
            }
        }

        private int parityValue;
        public int ParityValue
        {
            get { return parityValue; }
            private set
            {
                parityValue = value;
                OnPropertyChanged("ParityValue");
            }
        }

        private int length;
        public int Length
        {
            get { return length; }
            private set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        private int stopBit;
        public int StopBit
        {
            get { return stopBit; }
            private set
            {
                stopBit = value;
                OnPropertyChanged("StopBit");
            }
        }

        #endregion

        #region WeightProperties
        private string weightDisplay;
        public string WeightDisplay
        {
            get { return weightDisplay; }
            protected set
            {
                weightDisplay = value;
                OnPropertyChanged("WeightDisplay");
            }
        }

        private decimal errorValue;
        public decimal ErrorValue
        {
            get { return errorValue; }
            protected set { errorValue = value; }
        }


        private decimal weight;
        public decimal Weight
        {
            get { return weight; }
            protected set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        private string setStartText;
        public string SetStartText
        {
            get { return setStartText; }
            protected set { setStartText = value; }
        }

        private int setSubStrPoint1;
        public int SetSubStrPoint1
        {
            get { return setSubStrPoint1; }
            protected set { setSubStrPoint1 = value; }
        }

        private int setSubStrPoint2;
        public int SetSubStrPoint2
        {
            get { return setSubStrPoint2; }
            protected set { setSubStrPoint2 = value; }
        }

        private string setEndText;
        public string SetEndText
        {
            get { return setEndText; }
            protected set { setEndText = value; }
        }
        #endregion

        public virtual void SetComport(string iComport, string iStartCharRead, string iEndCharRead, int iStartPoint, int iLength)
        {
            this.PortName = iComport;
            this.SetStartText = iStartCharRead;
            this.SetEndText = iEndCharRead;
            this.SetSubStrPoint1 = iStartPoint;
            this.SetSubStrPoint2 = iLength;
        }

        public void SetComport(string iComport, bool allowReconnectPort)
        {
            this.PortName = iComport;
            if (allowReconnectPort)
                ConnectPort();
        }

        public void ConnectPort()
        {

            readThread = new Thread(Read);

            BaudRate = 9600;
            Length = 8;
            ParityValue = 0;
            StopBit = 1;

            serialPort = new SerialPort(PortName, BaudRate, (Parity)ParityValue, Length, (StopBits)StopBit);
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            //serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

            //Set the read / write timeouts
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            _continue = true;

            try
            {
                if (serialPort != null)
                {
                    if (serialPort.IsOpen)
                        CloseConnect();

                    serialPort.Open();
                    SendCommand(RS232SaveStatus.Ready);
                }

                if (serialPort != null && serialPort.IsOpen)
                    readThread.Start();

                OnPropertyChanged("ComportStatus");
            }
            catch (Exception ex)
            {
                OnPropertyChanged("ComportStatus");
                MessageBox.Show(ex.Message);
            }

            //readThread.Join();
        }

        public event EventHandler WeightChanged;
        public event EventHandler InfoChanged;
        public decimal lastestWeight { get; protected set; }

        protected virtual void VerifyWeightChange()
        {
            if (Weight > 0
                    && (Weight <= lastestWeight - ErrorValue
                        || Weight >= lastestWeight + ErrorValue
                        || lastestWeight == 0))
            {
                if (WeightChanged != null)
                    WeightChanged(this, null);
                lastestWeight = Weight;
            }
        }

        protected virtual void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = serialPort.ReadLine();
                    if (InfoChanged != null)
                        InfoChanged(this, new InfoData() { Text = message });
                    //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
                    if (!string.IsNullOrEmpty(message))
                    {
                        string weightStr = message.Replace("\r", string.Empty).Replace("\n", string.Empty);

                        if (weightStr.Length > 0)
                        {
                            weightStr = weightStr.Replace(SetEndText, string.Empty);
                            WeightDisplay = weightStr.Substring(setSubStrPoint1, setSubStrPoint2).Trim();
                            Console.WriteLine(WeightDisplay);
                            Weight = Decimal.Parse(WeightDisplay);
                            VerifyWeightChange();

                            if (weightStr.StartsWith(SetStartText))
                                Weight = -1 * Weight;
                        }
                        else if (weightStr.Length > 10)
                        {
                            if (weightStr.EndsWith(SetEndText))
                            {
                                WeightDisplay = weightStr.Substring(0, weightStr.IndexOf(SetEndText)).Trim();
                                Console.WriteLine(WeightDisplay);
                                Weight = Decimal.Parse(WeightDisplay);
                            }
                            else
                            {
                                WeightDisplay = weightStr.Trim();
                                Console.WriteLine(WeightDisplay);
                                Weight = Decimal.Parse(WeightDisplay);
                            }
                        }

                        if (InfoChanged != null)
                            InfoChanged(this, new InfoData() { Text = weightStr });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(ex.ToString(), ApplicationLogCategory.General, (int)Priority.High, (int)GeneralApplicationLogEvent.DispatcherUnhandledException, System.Diagnostics.TraceEventType.Error);
                }
            }
        }

        public void SendCommand(RS232SaveStatus saveStatus)
        {
            //Display += string.Format("Command : {0}\r\n", CommandCode);
            if (serialPort != null && serialPort.IsOpen)
            {
                if (saveStatus == RS232SaveStatus.Ready)
                    serialPort.Write("S" + "\r\n");
                else if (saveStatus == RS232SaveStatus.Pass)
                    serialPort.Write("P" + "\r\n");
                else if (saveStatus == RS232SaveStatus.Reject)
                    serialPort.Write("R" + "\r\n");
                else if (saveStatus == RS232SaveStatus.Closed)
                    serialPort.Write("N" + "\r\n");
            }
        }

        public virtual void ResetWeight()
        {
            this.Weight = 0;
            this.WeightDisplay = "0";
        }

        public void CloseConnect()
        {
            _continue = false;
            if (serialPort != null && serialPort.IsOpen)
            {
                //Display += string.Format("************** Close Port : {0} **************\r\n", PortName);
                serialPort.Close();
            }

            if (readThread != null)
                readThread.Abort();
        }
        #endregion

    }
}
