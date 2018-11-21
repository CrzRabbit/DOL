using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Net;

namespace Assets.Scripts.Net
{
    class Message
    {
        static int ldLen = 4;                   //保存msg长度信息所占字节数    
        private byte[] data = new byte[10240];
        private int startIndex = 0;             //已存字节数

        //解析data，获取msg
        public void ParseMessage(int newDataAmount, Action<ActionCode, string> processDataCallBack)
        {
            //更新开始索引
            startIndex += newDataAmount;
            //死循环，有data一直解析
            while (true)
            {
                //msglength信息是否完整
                if (startIndex <= 4)
                {
                    return;
                }
                //获取长度
                int msgLength = BitConverter.ToInt32(data, 0);
                //剩余的是否为完整的msg
                if (startIndex - 4 >= msgLength)
                {
                    //requestCode
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                    //data
                    string msg = Encoding.UTF8.GetString(data, 8, msgLength - 4);
                    //回调
                    processDataCallBack(actionCode, msg);

                    Array.Copy(data, ldLen + msgLength, data, 0, startIndex - ldLen - msgLength);
                    startIndex -= (ldLen + msgLength);
                }
                //信息不足
                else
                {
                    return;
                }
            }
        }

        public static byte[] PakcData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>();
            return newBytes.Concat(dataBytes).ToArray<byte>();
        }

        public static byte[] PakcData(RequestCode requestCode, ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] separatorBytes = Encoding.UTF8.GetBytes("#");
            int dataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length + 1;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            return dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>()
                .Concat(actionCodeBytes).ToArray<byte>()
                .Concat(dataBytes).ToArray<byte>()
                .Concat(separatorBytes).ToArray<byte>();
        }

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }
    }
}
