// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


class Program
{


    static void DisplayHelp()
    {
        Console.WriteLine("\nhk_pf: display incoming platform housekeeping data");
       
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



    static void DisplayPfHkLog()
    {

    }
     
    static void Main()
    {
        string uplinkFilePath = "Uplink.txt";
        string MIBFilePath = "MIB.txt";
        string downlinkPath = "C:\\Users\\wolinn-2\\source\\repos\\test\\test\\bin\\Debug\\net8.0\\Downlink.txt";
        string EventLog= "EventLog.txt";
        string HKLogPF = "HKlogPF.txt";
        string HKLogPL = "HKLogPL.txt";
        string commandLog = "CommandLog";

        File.WriteAllText(EventLog, ""); //startup clear the file path, just for assignment
        File.WriteAllText(HKLogPF, ""); //startup clear the file path, just for assignment
        File.WriteAllText(HKLogPL, ""); //startup clear the file path, just for assignment
        File.WriteAllText(commandLog, ""); //startup clear the file path, just for assignment


        ClearSignalFile(uplinkFilePath); //startup clear the file path, just for assignment



        //this is a parallell task that cyclycally reads the downlink file to look for incoming telemetry and writes it all down to the log file with an appended receival timestamp
        Task.Run(async () =>
        {
            
            
            while (true)
            {
                string groundTime = DateTime.Now.ToString("HH:mm:ss");
                using var rfs = new FileStream(downlinkPath, FileMode.OpenOrCreate, FileAccess.Read,FileShare.ReadWrite | FileShare.Delete);

                using var sr = new StreamReader(rfs);
                string[] lines = (await sr.ReadToEndAsync()).Split('\n', StringSplitOptions.RemoveEmptyEntries);

                string text = lines.Length > 0 ? lines[^1].Trim() : string.Empty;


                if (text[0] == '1')
                    File.AppendAllText(HKLogPL, text + groundTime + "\n");
                else if (text[0] == '2')
                    File.AppendAllText(HKLogPF, text + groundTime + "\n");
                else if (text[0] == '3')
                    File.AppendAllText(HKLogPL, text + groundTime + "\n");
                else if (text[0] == '4')
                    File.AppendAllText(commandLog, text + groundTime + "\n");



                await Task.Delay(1000);
            }
        });


        //this is the main loop for accepting inputted telecommands
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
                case "display_hk_pf_log":
                    DisplayPfHkLog();
                    break;
         






            }

        }

    }
}
