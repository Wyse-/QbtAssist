﻿using System;
using System.Diagnostics;
using System.Threading;

namespace qBtAssist
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"   ____  _     _                    _     _          __  __ ");            //
            Console.WriteLine(@"  / __ \| |   | |     /\           (_)   | |        /_ |/_ |");            //
            Console.WriteLine(@" | |  | | |__ | |_   /  \   ___ ___ _ ___| |_  __   _| | | |");            // ASCII art.
            Console.WriteLine(@" | |  | | '_ \| __| / /\ \ / __/ __| / __| __| \ \ / / | | |");            //
            Console.WriteLine(@" | |__| | |_) | |_ / ____ \\__ \__ \ \__ \ |_   \ V /| |_| |");            //
            Console.WriteLine(@"  \___\_\_.__/ \__/_/    \_\___/___/_|___/\__|   \_/ |_(_)_|");            //
            Console.WriteLine();                                                                           //
            Console.WriteLine(@"````````````````````````````````````````````````````````````");            //

            bool isLoggedIn = false;
            string[] xmlData;                                                                              //
            string host = "";                                                                              //
            string username = "";                                                                          //
            string password = "";                                                                          // Variable initialization, the names should make this pretty straight forward.
            string[] processNames;                                                                         //
            long uploadLimit = 0;                                                                          //
            long downloadLimit = 0;                                                                        //

            XML.LoadXML("config.xml");                                                                     //
            xmlData = XML.ParseXML();                                                                      //
            host = xmlData[0];                                                                             //
            username = xmlData[1];                                                                         // Load and parse XML file, then assign the values of the XML contents to their respective variables.
            password = xmlData[2];                                                                         //
            uploadLimit = long.Parse(xmlData[3]);                                                          //
            downloadLimit = long.Parse(xmlData[4]);                                                        //
            processNames = new string[xmlData.Length - 5];                                                 //

            for (int i = 5; i < xmlData.Length; i++)                                                       //
            {                                                                                              // Populate processNames string array with the content of the <Process> nodes in the XML file.
                processNames[i - 5] = xmlData[i];                                                          //
            }                                                                                              //

            while (true)                                                                                   // Infinite loop.
            {
                if (IsQbittorrentRunning())                                                                // Check if Qbittorrent is running; if it is proceed by attempting login to WebUI, otherwise display an error message and exit with code -1.
                {
                    API.Initialize(host, 100);                                                             //
                    if (isLoggedIn = (bool)API.Login(username, password).Result)                           //
                    {                                                                                      // Initialize Qbittorrent WebUI connection and attempt login with supplied info.
                        Console.WriteLine("                           ");                                  //
                        Console.SetCursorPosition(0, Console.CursorTop - 1);                               //
                        Console.WriteLine("Login successful.");                                            //
                        Console.SetCursorPosition(0, Console.CursorTop - 1);                               //
                    }                                                                                      //
                    else
                    {                                                                                      //
                        Console.WriteLine("Login failed, correct login info and retry.");                  // Display error message, wait for 5 seconds and exit with code -1.
                        Thread.Sleep(5000);                                                                //
                        Environment.Exit(-1);                                                              //
                    }
                }
                else
                {
                    Console.WriteLine("Qbittorrent is not running.");                                      //
                    Console.SetCursorPosition(0, Console.CursorTop - 1);                                   // Write over previous line.
                }

                Thread.Sleep(5000);                                                                        // Sleep for 5 seconds.

                while (IsQbittorrentRunning() && isLoggedIn)                                               //
                {                                                                                          //
                    if (IsProcessRunning(processNames))                                                    //
                    {                                                                                      //
                        Console.WriteLine("Process running, capping upload & download.");                  // Check if a process from the processNames array is running while Qbittorrent is running: if there's at least one process running set upload and download limit to supplied values.
                        API.SetGlobalUploadSpeedLimit(uploadLimit);                                        // If Qbittorrent process ends (e.g. it crashes or is terminated by the user) attempt connection again.
                        API.SetGlobalDownloadSpeedLimit(downloadLimit);                                    //
                    }                                                                                      //
                    else
                    {                                                                                      // 
                        Console.WriteLine("No process running, uncapping upload & download.");             // 
                        API.SetGlobalUploadSpeedLimit(0);                                                  // If no process is running uncap upload and download.
                        API.SetGlobalDownloadSpeedLimit(0);                                                // 
                    }                                                                                      // 

                    Thread.Sleep(5000);                                                                    // Sleep for 5 seconds.

                    Console.SetCursorPosition(0, Console.CursorTop - 1);                                   //
                    Console.WriteLine("                                                  ");               // Clear line.
                    Console.SetCursorPosition(0, Console.CursorTop - 1);                                   //
                }
            }
        }

        /// <summary>
        /// Determine if a process is running.
        /// </summary>
        /// <param name="processNames">Array of all process names.</param>
        /// <returns>True if a process is running, false if not.</returns>
        static bool IsProcessRunning(string[] processNames)
        {

            for(int i = 0; i < processNames.Length; i++)
            {
                Process[] p = Process.GetProcessesByName(processNames[i]);
                if (p.Length > 0)
                {
                    return true;
                }
            }
            return false;
            
        }

        /// <summary>
        /// Determine if Qbittorrent is running.
        /// </summary>
        /// <returns>True if Qbittorrent is running, false if not.</returns>
        static bool IsQbittorrentRunning()
        {
            if(Process.GetProcessesByName("qbittorrent").Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
