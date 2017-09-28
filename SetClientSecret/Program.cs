using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetClientSecret
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("C:\\Program Files (x86)\\IIS Express\\client_secret.json"))
            {
                string sourceCreds = Path.GetFullPath("client_secret.json");
                string destCreds = "C:\\Program Files (x86)\\IIS Express\\client_secret.json";
                File.Copy(sourceCreds, destCreds);
                Console.WriteLine("file has been copied successfully!");
            }
            else
            {
                Console.WriteLine("File already exists in the directory!");
            }
        }
    }
}
