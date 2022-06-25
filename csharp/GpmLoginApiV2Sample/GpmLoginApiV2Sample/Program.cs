﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GpmLoginApiV2Sample.Libs;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;

namespace GpmLoginApiV2Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GPMLoginAPI api = new GPMLoginAPI("http://127.0.0.1:49806");

            // Print list off profiles in GPMLogin -------------------------
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PROFILES ----------------------------");
            Console.ForegroundColor = ConsoleColor.White;

            List<JObject> profiles = api.GetProfiles();
            if (profiles != null)
            {
                foreach (JObject profile in profiles)
                {
                    string name = Convert.ToString(profile["name"]);
                    string id = Convert.ToString(profile["id"]);
                    Console.WriteLine($"ID: {id} | Name: {name}");
                }
            }


            // Create profile and print id -------------------------
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("CREATE PROFILE ------------------");
            Console.ForegroundColor = ConsoleColor.White;

            JObject createdResult = api.Create("giaiphapmmo.net");
            string createdProfileId = null;

            if (createdResult != null)
            {
                bool status = Convert.ToBoolean(createdResult["status"]);
                if (status)
                    createdProfileId = Convert.ToString(createdResult["profile_id"]);
            }

            Console.WriteLine("Created profile ID: " + createdProfileId);

            // Start profile ----------------------------------------
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("START PROFILE ------------------");
            Console.ForegroundColor = ConsoleColor.White;

            JObject startedResult = api.Start(createdProfileId);
            if (startedResult != null)
            {
                bool status = Convert.ToBoolean(createdResult["status"]);
                if (status)
                {
                    string browserLocation = Convert.ToString(startedResult["browser_location"]);
                    string seleniumRemoteDebugAddress = Convert.ToString(startedResult["selenium_remote_debug_address"]);
                    string gpmDriverPath = Convert.ToString(startedResult["selenium_driver_location"]);

                    // Init selenium
                    FileInfo gpmDriverFileInfo = new FileInfo(gpmDriverPath);

                    ChromeDriverService service = ChromeDriverService.CreateDefaultService(gpmDriverFileInfo.DirectoryName, gpmDriverFileInfo.Name);
                    ChromeOptions options = new ChromeOptions();
                    options.BinaryLocation = browserLocation;
                    options.DebuggerAddress = seleniumRemoteDebugAddress;

                    UndetectChromeDriver driver = new UndetectChromeDriver(service, options);

                    driver.Get("https://fingerprint.com/products/bot-detection/");


                    // Delay 10 s, close and delete profile
                    Thread.Sleep(10000);
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                }
            }

            // Start profile ----------------------------------------
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("DELETE PROFILE ------------------");
            Console.ForegroundColor = ConsoleColor.White;

            api.Delete(createdProfileId);
            Console.WriteLine("Deleted : "+createdProfileId + "\n\n");


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ALL DONE, PRESS ENTER TO EXIT");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
    }
}
