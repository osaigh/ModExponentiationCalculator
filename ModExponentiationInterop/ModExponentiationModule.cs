using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ModExponentiationInterop
{
    /// <summary>
    /// A COM component that provides inter-process communication. 
    /// Can be consumed in an Excel spreadsheet
    /// </summary>
    [Guid("7887862B-75B0-4169-8648-CE9433DA1AE2")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ModExponentiationModule
    {

        public int Calculate_ModExpo(int b, int n, int m)
        {
            TcpClient client = new TcpClient("127.0.0.1", 2000);
            int result = 0;

            try
            {
                NetworkStream strm = client.GetStream();
                BinaryReader rd = new BinaryReader(strm);
                BinaryWriter wr = new BinaryWriter(strm);

                wr.Write(b);
                wr.Write(n);
                wr.Write(m);
                //wr.Flush();

                result = rd.ReadInt32();
                strm.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {
                client.Close();
            }
            return result;
        }
    }
}
