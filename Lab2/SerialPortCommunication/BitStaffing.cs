using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCComm
{
    class BitStaffing
    {
        public string Message { get; set; }

        public BitStaffing(string message)
        {
            this.Message = message;
        }

        private readonly string flag = "0" + Convert.ToString(0x7E, 2);
        private readonly string oldMess = "111111";
        private readonly string newStaffMess = "111110";

        /// <summary>
        /// Method wraps and packages data with bit-staffing
        /// </summary>
        /// <returns></returns> 
        public string WrapInAPackage()
        {
            byte[] buf = Encoding.UTF8.GetBytes(this.Message);
            StringBuilder sb = new StringBuilder(buf.Length * 8);
            foreach (byte b in buf)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            string bitStr = sb.ToString(); // Сообщение в двоичном формате в строковом формате

            bitStr = bitStr.Replace(oldMess, newStaffMess);
            
            return bitStr.Insert(0, flag).Insert(this.Message.Length, flag);
        }

        /// <summary>
        /// Method for unpacking data from buffer
        /// </summary>
        /// <returns></returns>
        public string UnPacking()
        {
            string msg = this.Message;
            
            msg = msg.Replace(flag, string.Empty);

            var stringArray = Enumerable.Range(0, msg.Length / 8).Select(i => Convert.ToByte(msg.Substring(i * 8, 8), 2)).ToArray();
            msg = Encoding.UTF8.GetString(stringArray);

            return msg;
        }
    }
}
