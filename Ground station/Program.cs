// See https://aka.ms/new-console-template for more information

using System;
using System.IO;


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


    static void DisplayPfHk()
    {

    }

    static void DisplayPlHk()
    {

    }

    static void TargetPoint()
    {
        string[] lines = File.ReadAllLines("MIB.txt");
        File.WriteAllText("Signal.txt", lines[0]);
    }

    static void CollectData()
    {

    }

    static void Idle()
    {

    }

    static void SafeMode()
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


        bool running = true;
        while (running)
        {
            Console.WriteLine("type 'list' for a list of commands");
            Console.Write("Input command: ");
            string command = Console.ReadLine();

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
                    TargetPoint();
                    break;
                case "collect_data":
                    CollectData();
                    break;
                case "idle":
                    Idle();
                    break;
                case "safe_mode":
                    SafeMode();
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
