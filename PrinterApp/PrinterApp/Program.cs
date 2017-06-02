using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrinterApp
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string fileName = @"D:\Document\Dummy\dummyPdf.pdf";
            //Pdf.PrintPDFs(fileName);
                Pdf.PrintAllDocument(fileName);
            //Pdf.JustPrint(fileName);


        }
        public class Pdf
        {
            public static Boolean PrintPDFs(string pdfFileName)
            {
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.Verb = "PrintTo";
                    
                    //Define location of adobe reader/command line
                    //switches to launch adobe in "print" mode
                    proc.StartInfo.FileName = @"C:\Program Files (x86)\Adobe\Reader 10.0\Reader\AcroRd32.exe";
                    proc.StartInfo.Arguments = String.Format(@"/p /h {0}", pdfFileName);
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;

                    proc.Start();
                  
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    if (proc.HasExited == false)
                    {
                        proc.WaitForExit(10000);
                    }

                    proc.EnableRaisingEvents = true;

                    proc.Close();
                    KillAdobe("AcroRd32");
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            //For whatever reason, sometimes adobe likes to be a stage 5 clinger.
            //So here we kill it with fire.
            private static bool KillAdobe(string name)
            {
                foreach (Process clsProcess in Process.GetProcesses().Where(
                             clsProcess => clsProcess.ProcessName.StartsWith(name)))
                {
                    clsProcess.Kill();
                    return true;
                }
                return false;
            }


            public static void PrintAllDocument(string filename)
            {
                // Send it to the selected printer
                using (PrintDialog printDialog1 = new PrintDialog())
                {
                    printDialog1.PrinterSettings.PrinterName = "HP LaserJet Pro MFP M126nw";
                    //if (printDialog1.ShowDialog() == DialogResult.OK)
                    //{
                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(filename);
                        info.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                        info.CreateNoWindow = true;
                        info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        info.UseShellExecute = true;
                        info.Verb = "PrintTo";
                        System.Diagnostics.Process.Start(info);
                        KillAdobe("AcroRd32");
                    //}
                }
            }

            public static void JustPrint(string filename)
            {
               var pNewProcess = new Process();

                string sPrinter = "HP LaserJet Pro MFP M126nw";
                pNewProcess.StartInfo.FileName = filename;

                pNewProcess.StartInfo.Verb = "printto";

                pNewProcess.StartInfo.CreateNoWindow = true;

                pNewProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                pNewProcess.StartInfo.Arguments = "\"" + sPrinter + "\"";

                pNewProcess.StartInfo.UseShellExecute = true;

               // pNewProcess.StartInfo.WorkingDirectory = sDocPath;

                pNewProcess.Start();
            }
        }//END Class

     
    }
}
