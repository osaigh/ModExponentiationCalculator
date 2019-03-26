using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace ModExponentiationCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void ModExponentiationDelegate(int b, int n, int m);
        private int modExpResult = 0;
        NumberTheoryAlgorithms numberTheoryAlgorithms;
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            this.numberTheoryAlgorithms = new NumberTheoryAlgorithms();
            //RunServer();
            thread = new Thread(RunServer);
            thread.Start();
        }

        public void ModModExponentiationExecute(int b, int n, int m)
        {
            this.bValue.Content = b;
            this.nValue.Content = n;
            this.mValue.Content = m;
            var mdExp = numberTheoryAlgorithms.ModExponentiation(b, n, m);
            this.result.Content = mdExp;
            modExpResult = mdExp;

        }

       

        public void RunServer()
        {
            System.Windows.Threading.Dispatcher dp = Application.Current.Dispatcher;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            var listen = new TcpListener(localAddr, 2000);
            Socket soc;

            int b, n, m;

            var modExponentiation = new ModExponentiationDelegate(ModModExponentiationExecute);

            listen.Start();

            while (true)
            {
                soc = listen.AcceptSocket();

                try
                {
                    NetworkStream strm = new NetworkStream(soc);
                    BinaryReader br = new BinaryReader(strm);
                    BinaryWriter bw = new BinaryWriter(strm);

                    b = br.ReadInt32();
                    n = br.ReadInt32();
                    m = br.ReadInt32();

                    dp.Invoke(modExponentiation,b,n,m);

                    bw.Write(modExpResult);

                    strm.Close();
                }catch(Exception e)
                {

                }
            }
        }
    }

    
}
