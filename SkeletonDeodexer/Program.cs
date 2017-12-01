//SkeletonDeodexer - deodexes files using smali/baksmali and 7zip
//Copyright (C) 2014  foxox - foxox.jd@live.com

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

namespace SkeletonDeodex
{
    class Program
    {
        static string convertDirWin2Cyg(string filefullname)
        {
            filefullname = filefullname.Replace("C:\\", "/cygdrive/c/");
            filefullname = filefullname.Replace("c:\\", "/cygdrive/c/");
            filefullname = filefullname.Replace('\\', '/');
            return filefullname;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("SkeletonDeodexer  Copyright (C) 2014  foxox - foxox.jd@live.com\nThis program comes with ABSOLUTELY NO WARRANTY.\nThis is free software, and you are welcome to redistribute it\nunder certain conditions. See COPYING.md for details.");

            string indir = "";
            if (args.Length < 1)
                indir = System.IO.Directory.GetCurrentDirectory();
            else
                indir = args[0];

            string outdir = "";
            if (args.Length < 2)
                outdir = System.IO.Directory.GetCurrentDirectory() + "/deodexed";
            else
                outdir = args[1];

            string frameworkdir = "C:/android/framework/";

            //indir = convertDirWin2Cyg(indir);
            //outdir = convertDirWin2Cyg(outdir);
            //frameworkdir = convertDirWin2Cyg(frameworkdir);

            //make temp dir
            System.IO.Directory.CreateDirectory("temp");
            //make deodexed output 
            System.IO.Directory.CreateDirectory(outdir);

            //CWD
                //temp
            //indir
            //outdir

            //Get list of files here
            foreach (string file in System.IO.Directory.EnumerateFiles(indir))
            {
                if (!file.EndsWith("odex"))
                    continue;

                //java -jar ../jar/baksmali.jar -d "C:/android/framework/" -x AccuweatherDaemon.odex
                //java -jar ../jar/smali.jar out -o classes.dex
                //mv classes.dex ../temp
                //cp AccuweatherDaemon.apk ../temp
                //"jar/7z.exe" a -tzip "temp/AccuweatherDaemon.apk" "temp/classes.dex"
                //mv "temp/AccuweatherDaemon.apk" deodexed

                string filefullname = file;
                //filefullname = convertDirWin2Cyg(filefullname);

                System.IO.FileInfo fileinfo = new System.IO.FileInfo(filefullname);
                string filename = fileinfo.Name;
                System.IO.DirectoryInfo filedirinfo = fileinfo.Directory;
                string filedir = filedirinfo.FullName;

                string filefullnameapk = filefullname.Replace(".odex", ".apk");
                string filenameapk = filename.Replace(".odex", ".apk");

                System.IO.FileInfo fileinfoapk = new System.IO.FileInfo(filefullnameapk);
                if (!fileinfoapk.Exists)
                    continue;   //skip framework files for now... no JAR deodexing
                //else
                    //Console.WriteLine("aoeu");

                System.Console.WriteLine("Processing " + filename);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;

                string test = "";

                Process running;

                //java -jar ../jar/baksmali.jar -d "C:/android/framework/" -x AccuweatherDaemon.odex
                psi.FileName = "java"; psi.Arguments = "-jar baksmali.jar -d \"" + frameworkdir + "\" -x \"" + filefullname + "\"";
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();

                //java -jar ../jar/smali.jar out -o classes.dex
                psi.FileName = "java"; psi.Arguments = "-jar smali.jar out -o temp\\classes.dex";
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();

                ////mv classes.dex ../temp
                //psi.FileName = "mv"; psi.Arguments = "classes.dex temp";
                //test = psi.FileName + " " + psi.Arguments;
                //running = Process.Start(psi);
                //running.WaitForExit();

                //cp AccuweatherDaemon.apk ../temp
                psi.FileName = "cp"; psi.Arguments = convertDirWin2Cyg(filefullnameapk) + " temp";
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();

                //"jar/7z.exe" a -tzip "temp/AccuweatherDaemon.apk" "temp/classes.dex"
                string temp = psi.WorkingDirectory; psi.WorkingDirectory = "temp";
                psi.FileName = "7za.exe"; psi.Arguments = "a -tzip \"" + filenameapk + "\" \"classes.dex\"";
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();
                psi.WorkingDirectory = temp;    //restore CWD
                
                //mv "temp/AccuweatherDaemon.apk" deodexed
                psi.FileName = "mv"; psi.Arguments = "\"temp/" + filenameapk + "\" " + convertDirWin2Cyg(outdir);
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();

                //rm out dir
                psi.FileName = "rm"; psi.Arguments = "-r out/* temp/*";
                test = psi.FileName + " " + psi.Arguments;
                running = Process.Start(psi);
                running.WaitForExit();
                
            }

            //END

        }//End Main
    }//End Program
}//End Namespace for Project
