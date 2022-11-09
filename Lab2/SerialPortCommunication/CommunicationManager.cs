using System;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using ByteStuffing;

namespace PCComm
{
    class CommunicationManager
    {
        #region Manager Enums
        public enum TransmissionType { Text }

        public enum MessageType { Incoming, Outgoing, Normal, Warning, Error };
        #endregion

        #region Manager Variables
        private readonly Color[] MessageColor = { Color.Blue, Color.Green, Color.Brown, Color.Orange, Color.Red };
        private readonly SerialPort comPort = new SerialPort();
        #endregion
        
        #region Manager Properties
        public string BaudRate { get; set; }

        public string Parity { get; set; }

        public string StopBits { get; set; }

        public string DataBits { get; set; }

        public string PortName { get; set; }

        public bool FlagSuffing { get; set; }

        public TransmissionType CurrentTransmissionType { get; set; }

        public RichTextBox DisplayWindow { get; set; }
        #endregion

        #region Manager Constructors
        public CommunicationManager(string baud, string par, string sBits, string dBits, string name, RichTextBox rtb)
        {
            BaudRate = baud;
            Parity = par;
            StopBits = sBits;
            DataBits = dBits;
            PortName = name;
            DisplayWindow = rtb;
            comPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
        }

        public CommunicationManager()
        {
            BaudRate = string.Empty;
            Parity = string.Empty;
            StopBits = string.Empty;
            DataBits = string.Empty;
            PortName = "COM1";
            DisplayWindow = null;
            comPort.DataReceived += new SerialDataReceivedEventHandler(ComPort_DataReceived);
        }
        #endregion

        #region WriteDataBitStuffing
        public void WriteDataBitStuffing(string msg)
        {
            if (msg.Length <= 0)
            {
                return;
            }

            if (!comPort.IsOpen) comPort.Open();

            var mess = new BitStaffing(msg);
            string messageInPacket = mess.WrapInAPackage();

            comPort.Write(messageInPacket);
            DisplayData(MessageType.Outgoing, "-> " + msg + "\n");
        }
        #endregion

        #region WriteDataByteStuffing
        public void WriteDataByteStuffing(string msg)
        {
            if (msg.Length <= 0)
            {
                return;
            }

            if (!comPort.IsOpen) comPort.Open();

            var bytee = new ByteStuffer();
            byte[] bytes = bytee.GetEncryptedBytes(msg);
            comPort.Write(bytes, 0, bytes.Length);
            DisplayData(MessageType.Outgoing, "-> " + msg + "\n");
        }
        #endregion

        #region DisplayData
        [STAThread]
        private void DisplayData(MessageType type, string msg)
        {
            DisplayWindow.Invoke(new EventHandler(delegate
            {
            DisplayWindow.SelectedText = string.Empty;
            DisplayWindow.SelectionFont = new Font(DisplayWindow.SelectionFont, FontStyle.Bold);
            DisplayWindow.SelectionColor = MessageColor[(int)type];
            DisplayWindow.AppendText(msg);
            DisplayWindow.ScrollToCaret();
            }));
        }
        #endregion

        #region OpenPort
        public bool OpenPort()
        {
            try
            {
                if (comPort.IsOpen) comPort.Close();

                comPort.BaudRate = int.Parse(BaudRate);    //BaudRate
                comPort.DataBits = int.Parse(DataBits);    //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBits);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), Parity);    //Parity
                comPort.PortName = PortName;   //PortName
                comPort.Open();
                DisplayData(MessageType.Normal, "Port opened at " + DateTime.Now + "\n");
                return true;
            }
            catch (Exception ex)
            {
                DisplayData(MessageType.Error, ex.Message + " Выберите другой доступный порт!");
                return false;
            }
        }
        #endregion

        public bool ChangeBaudRate()
        {
            if (comPort.IsOpen)
            {
                comPort.BaudRate = int.Parse(BaudRate);    //BaudRate
                return true;
            }

            return false;
        }

        public bool ClosePort()
        {
            try
            {
                if (comPort.IsOpen)
                {
                    comPort.Close();
                }
                DisplayData(MessageType.Normal, "Port closed at " + DateTime.Now + "\n");
                return true;
            }
            catch (Exception ex)
            {
                DisplayData(MessageType.Error, ex.Message);
                return false;
            }
        }

        #region SetParityValues
        public void SetParityValues(object obj)
        {
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region SetStopBitValues
        public void SetStopBitValues(object obj)
        {
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region SetPortNameValues
        public void SetPortNameValues(object obj)
        {

            foreach (string str in SerialPort.GetPortNames())
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region comPort_DataReceived
        void ComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (FlagSuffing)
            {
                string msg = comPort.ReadExisting();
                if (msg.Contains("~"))
                {
                    DisplayData(MessageType.Error, "Stuffing must be the same!" + "\n");
                    return;
                }
                var str = new BitStaffing(msg);
                msg = str.UnPacking();
                msg = msg.Length <= 7 ? msg.Remove(0, 1) : msg;
                
                DisplayData(MessageType.Incoming, "<- " + msg + "\n");
            }
            else
            {
                byte[] buff = new byte[comPort.BytesToRead];
                comPort.Read(buff, 0, comPort.BytesToRead);
                if (buff[0] != 0x7E)
                {
                    DisplayData(MessageType.Error, "Stuffing must be the same!" + "\n");
                    return;
                }

                var str = new ByteStuffer();
                List<byte> buffer = new List<byte>(buff);
                DisplayData(MessageType.Incoming, "<- " + str.GetDecryptedString(buffer) + "\n");
            }
        }
        #endregion
    }
}