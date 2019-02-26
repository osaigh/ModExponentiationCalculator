using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModExponentiationCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void ModExponentiationDelegate(int b, int n, int m);
        private int modExpResult = 0;

        public MainWindow()
        {
            InitializeComponent();
            RunServer();
        }

        /// <summary>
        /// Efficient algorithm for calculating modulo exponentiation
        /// i.e the remainder of (b raised to power(n)) / m
        /// </summary>
        /// <param name="b"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        public void ModExponentiation(int b, int n, int m)
        {
            var expansion = Base_b_Expansion(n,2);

            int x = 1;
            var result = Div_Mod(b,m);
            int power = result.Mod;

            foreach(var i in expansion)
            {
                if(i == 1)
                {
                    result = Div_Mod(x* power, m);
                    x = result.Mod;
                }
                result = Div_Mod(power * power, m);
                power = result.Mod;
            }

            this.bValue.Content = b;
            this.nValue.Content = n;
            this.mValue.Content = m;
            this.result.Content = x;
            modExpResult = x;
        }

        /// <summary>
        /// Base b expansion of integer n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="b"></param>
        public IEnumerable<int> Base_b_Expansion(int n, int b)
        {
            int quotient = n;
            List<int> expansion = new List<int>();

            while(quotient > 0)
            {
                var result = Div_Mod(quotient,b);
                expansion.Add(result.Mod);
                quotient = result.Div;

            }

            return expansion;
        }

        /// <summary>
        /// Calsulates the result and the remainder when n is divided by m
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public DivModResult Div_Mod(int n, int m)
        {
            int d = 0;
            int r = Math.Abs(n);

            while(r >= m)
            {
                r = r - m;
                d = d + 1;
            }

            if(n < 0 && r > 0)
            {
                r = m - r;
                d = -(d + 1);
            }

            DivModResult result = new DivModResult() { Div=d,Mod = r};

            return result;
        }

        public void RunServer()
        {
            System.Windows.Threading.Dispatcher dp = Application.Current.Dispatcher;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            var listen = new TcpListener(localAddr, 2000);
            Socket soc;

            int b, n, m;

            var modExponentiation = new ModExponentiationDelegate(ModExponentiation);

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

    public class DivModResult
    {
        public int Div { get; set; }
        public int Mod { get; set; }
    }
}
