using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ByteStuffing
{
    class ByteStuffer
    {
        public byte[] GetEncryptedBytes(string message)
        {
            List<string> packeges = message.Split(' ').ToList();
            List<byte> commonData = new List<byte>();

            for (int i = 0; i < packeges.Count; i++)
            {
                List<byte> partialData = Encoding.Unicode.GetBytes(packeges[i] + ((i == packeges.Count - 1) ? "" : " ")).ToList();

                Encrypt(partialData);

                commonData.AddRange(partialData);
            }

            return commonData.ToArray();
        }

        public string GetDecryptedString(List<byte> data)
        {
            Decrypt(data);
            return Encoding.Unicode.GetString(data.ToArray(), 0, data.Count);
        }

        private void Encrypt(List<byte> data)
        {
            data.Insert(0, 0x7e);
            for (int i = 1; i < data.Count; i++)
            {
                if (data[i] == 0x7e)
                {
                    data[i] = 0x7d;
                    data.Insert(i + 1, 0x5e);
                }
                if (data[i] == 0x7d)
                {
                    data.Insert(i + 1, 0x5d);
                }
            }
        }

        private void Decrypt(List<byte> data)
        {
            data.RemoveAt(0);
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == 0x7d)
                {
                    if (data[i + 1] == 0x5e)
                    {
                        data[i] = 0x7e;
                        data.RemoveAt(i + 1);
                    }
                    else if (data[i + 1] == 0x5d)
                    {
                        data.RemoveAt(i + 1);
                    }
                }
                else if (data[i] == 0x7e)
                {
                    data.RemoveAt(i);
                }
            }
        }
    }
}