using System;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace PixivDL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //C:\Users\dsg\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup
            //string startup =  Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            //Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            //C:\Users\dsg\AppData\Roaming
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            var ain =  Task.Run<int>(DLPic);
    
            Console.WriteLine("await!!");
            ain.Wait();
            Console.WriteLine(ain.Result);
            Console.WriteLine();
        }

        private async static Task<int> DLPic()
        {
            byte[] temp;

            HttpClient hc = new HttpClient() { Timeout = TimeSpan.FromSeconds(20) };

            string picdl = "http://hnd-jp-ping.vultr.com/vultr.com.100MB.bin";

            //hc.DefaultRequestHeaders.Referrer = new Uri("https://www.pixiv.net");
            //string picdl = "https://i.pximg.net/img-original/img/2017/06/21/00/00/01/63485483_p0.jpg";

            var resop =await hc.GetAsync(picdl);
            //var resop = hc.SendAsync(new HttpRequestMessage(HttpMethod.Head,picdl)).Result;
            long? maxlen = resop.Content.Headers.ContentLength;

            string filename = picdl.Split('/').Last();

            Stream task1 =await resop.Content.ReadAsStreamAsync();
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
   
            FileStream fs = new FileStream(filename, FileMode.CreateNew);
            BinaryReader br = new BinaryReader(task1);
            
            while (true)
            {
                //一开始  byte[] temp;
                temp = br.ReadBytes(2048);
                fs.Write(temp, 0, temp.Length);
                fs.Flush();
                Console.WriteLine(fs.Length);
                if (maxlen == fs.Length)
                {
                    // 可以刷新 进度条UI
                    break;
                }
            }
            Console.WriteLine("Successful1");
            br.Dispose();
            fs.Dispose();

            return 0;
        }

        /*
 * using System.Linq;
 * using System.Net.Http;
 * using System.IO;
 * using System.Threading.Tasks;
 */
        private static int DLPic2()
        {
            HttpClient hc = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) };
            hc.DefaultRequestHeaders.Referrer = new Uri("https://www.pixiv.net");
            string picdl = "https://i.pximg.net/img-original/img/2017/06/21/00/00/01/63485483_p0.jpg";
            Task<byte[]> task1 = hc.GetByteArrayAsync(picdl);
            string filename = picdl.Split('/').Last();
            task1.Wait();
            File.WriteAllBytesAsync(filename, task1.Result);
            Console.WriteLine("Successful2");
            return 0;
        }
    }
}
