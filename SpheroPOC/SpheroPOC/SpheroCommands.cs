using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpheroPOC
{
    public class SpheroCommands
    {
        byte SOP = 0xff;
        byte SOP2 = 0xff;

        /// <summary>
        /// Generate a byte packet to change the color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public byte[] CreateColorChangePacket(byte r, byte g, byte b)
        {
            byte[] color = new byte[4];
            color[0] = (byte)r;
            color[1] = (byte)g;
            color[2] = (byte)b;
            color[3] = 0x01;
            var msg = GetBasePacket(0x02, 0x20, color);
            return msg;
        }

       
        private byte[] GetBasePacket(byte DID, byte command, byte[] data)
        {
            byte[] msg = new byte[7+data.Length];

            msg[0] = SOP;  //SOP
            msg[1] = SOP2; //SOP2
            msg[2] = DID; //DID
            msg[3] = command; //Command
            msg[4] = 0x00; //Seq
            msg[5] = (byte)(data.Length+1); //DLEN
            
            //data
            for (int i = 0; i < data.Length; i++)
            {
                msg[6+i] = data[i]; //data
            }
            
            //Calculate checksum
            msg[msg.Length-1] = CheckSum(msg); //checksum

            return msg;
        }

        private byte CheckSum(byte[] msg)
        {
            int added = 0;

            //Add up the bytes
            for(int i=2;i<msg.Length-1;i++)
            {
                added = added + (int)msg[i];
            }

            //Now modulo 0xff
            int modulo = added % 256;

            //bit invert
            byte checksum = (byte)~modulo;

            return checksum;
        }
    }
}
