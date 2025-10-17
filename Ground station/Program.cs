// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Threading;


class Program
{


    static void DisplayHelp()
    {
        Console.WriteLine("\nhk_pf: display incoming platform housekeeping data");
        Console.WriteLine("hk_pl: displays incoming payload housekeeping data");
        Console.WriteLine("exit: exit the program");
        Console.WriteLine("target_point: Point sattelite towards target");
        Console.WriteLine("collect_data: sattelite will start collecting data, telemetry will be displayed, requires target point executed");
        Console.WriteLine("idle: sets the payload to its idle mode");
        Console.WriteLine("safe_mode: sets the sattelite into safe mode");
        Console.WriteLine("display_event_log: displays the contents of the log file\n");
        Console.WriteLine("display_hk_pl_log");
        Console.WriteLine("display_hk_pf_log");

    }


    static void ClearSignalFile(string signalFilePath)
    {
        File.WriteAllText(signalFilePath, "");
    }

    static void DisplayPfHk()
    {

    }

    static void DisplayPlHk()
    {

    }

    static void TargetPoint(string signalFilePath, string MIBFilePath, string downlinkPath)
    {
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using StreamWriter sw = new StreamWriter(fs);

        sw.WriteLine(lines[0]);
        

    }

    static void CollectData(string signalFilePath, string MIBFilePath, string downlinkPath)
    {
      
    }

    static void Idle(string signalFilePath, string MIBFilePath, string downlinkPath)
    {
      
    }

    static void SafeMode(string signalFilePath, string MIBFilePath, string downlinkPath)
    {
    
    }

    static void DisplayEventLog()
    {

    }

    static void DisplayPlHkLog()
    {

    }

    static void DisplayPfHkLog()
    {

    }

    static void Main()
    {
        string uplinkFilePath = "Uplink.txt";
        string MIBFilePath = "MIB.txt";
        string downlinkPath = "C:\\Users\\wolinn-2\\source\\repos\\test\\test\\bin\\Debug\\net8.0\\Downlink.txt";
        string LogPath = "Log.txt";

        File.WriteAllText(LogPath, ""); //startup clear the file path, just for assignment
        ClearSignalFile(uplinkFilePath); //startup clear the file path, just for assignment

        bool running = true;
        while (running)
        {
            Console.WriteLine("type 'list' for a list of commands");
            Console.Write("Input command: ");
            string command = Console.ReadLine();


            string lastTelemetry = File.ReadLines(LogPath).Last();

            switch (command)
            {
                case "list":
                    DisplayHelp();
                    break;
                case "exit":
                    running = false;
                    break;
                case "hk_pf":
                    DisplayPfHk();
                    break;
                case "hk_pl":
                    DisplayPlHk();
                    break;
                case "target_point":
                    TargetPoint(uplinkFilePath, MIBFilePath, downlinkPath);
                    break;
                case "collect_data":
                    CollectData(uplinkFilePath, MIBFilePath, downlinkPath);
                    break;
                case "idle":
                    Idle(uplinkFilePath, MIBFilePath, downlinkPath);
                    break;
                case "safe_mode":
                    SafeMode(uplinkFilePath, MIBFilePath, downlinkPath);
                    break;
                case "display_event_log":
                    DisplayEventLog();
                    break;
                case "display_hk_pl_log":
                    DisplayPlHkLog();
                    break;
                case "display_hk_pf_log":
                    DisplayPfHkLog();
                    break;
         






            }

        }

    }
}
