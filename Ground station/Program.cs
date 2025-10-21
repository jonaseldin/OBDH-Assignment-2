// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


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
        Console.WriteLine("display_command_log");

    }


    static void ClearSignalFile(string signalFilePath)
    {
        File.WriteAllText(signalFilePath, "");
    }





    static void TargetPoint(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[0], sendTime, groundTime);

        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine+"\n");




    }

    static void CollectData(string signalFilePath, string MIBFilePath)
    {
      
    }

    static void Idle(string signalFilePath, string MIBFilePath)
    {
      
    }

    static void SafeMode(string signalFilePath, string MIBFilePath)
    {
    
    }

    static void DisplayEventLog(string EventLog)
    {
        string[] log = File.ReadAllLines(EventLog);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        } 
            
    }



    static void DisplayPfHkLog(string HKLogPF)
    {
        string[] log = File.ReadAllLines(HKLogPF);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }

    static void DisplayPlHkLog(string HKLogPL)
    {
        string[] log = File.ReadAllLines(HKLogPL);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }

    static void DisplayCommandLog(string commandLog)
    {
        string[] log = File.ReadAllLines(commandLog);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }

    static void Main()
    {
        string uplinkFilePath = "Uplink.txt";
        string MIBFilePath = "MIB.txt";
        string downlinkPath = "C:\\Users\\wolinn-2\\source\\repos\\test\\test\\bin\\Debug\\net8.0\\Downlink.txt";
        string EventLog = "EventLog.txt";
        string HKLogPF = "HKlogPF.txt";
        string HKLogPL = "HKLogPL.txt";
        string commandLog = "CommandLog.txt";

        File.WriteAllText(EventLog, ""); //startup clear the file path, just for assignment
        File.WriteAllText(HKLogPF, ""); //startup clear the file path, just for assignment
        File.WriteAllText(HKLogPL, ""); //startup clear the file path, just for assignment
        File.WriteAllText(commandLog, ""); //startup clear the file path, just for assignment


        ClearSignalFile(uplinkFilePath); //startup clear the file path, just for assignment


        int sequence = 0;



        //this is a parallell task that cyclycally reads the downlink file to look for incoming telemetry and writes it all down to the log file with an appended receival timestamp
        Task.Run(async () =>
        {
            while (true)
            {
                bool confirmPrinted = false;
                string groundTime = DateTime.Now.ToString("HH:mm:ss");
                string[] lines = File.ReadAllLines(downlinkPath);

                var log1 = new List<string>();
                var log2 = new List<string>();
                var log3 = new List<string>();
               
                

                foreach (var line in lines)
                {
                    if (line.Length == 0) continue;

                    if (line[0] == '1')
                        log1.Add(line + " " + groundTime);
                    else if (line[0] == '2')
                        log2.Add(line + " " + groundTime);
                    else if (line[0] == '3')
                        log3.Add(line + " " + groundTime);
                    else if (line[0] == '4' && !confirmPrinted)
                    {
                        Console.WriteLine("Line starting with 4 detected: " + line);
                        log3.Add(line + " " + groundTime);
                        confirmPrinted = true; 

                    }
                }

                if (log1.Count > 0) File.AppendAllLines(HKLogPL, log1);
                if (log2.Count > 0) File.AppendAllLines(HKLogPF, log2);
                if (log3.Count > 0) File.AppendAllLines(EventLog, log3);

                File.WriteAllText(downlinkPath, "");

                await Task.Delay(1000);
            }
        }
        );


        //this is the main loop for accepting inputted telecommands
        bool running = true;
        while (running)
        {
            Console.WriteLine("type 'list' for a list of commands");
            Console.Write("Input command: ");
            string cmnd = Console.ReadLine();
            string[] command = cmnd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string seqString;

            if (command.Length < 2)
            {
                Console.WriteLine("Error: You must provide a time or type 'inst' for instant execution.");
                continue;
            }


            Thread.Sleep(1000);




            switch (command[0])
            {
                case "list":
                    DisplayHelp();
                    break;
                case "exit":
                    running = false;
                    break;
                case "target_point":
                    sequence++;
                    seqString = sequence.ToString();
                    TargetPoint(uplinkFilePath, MIBFilePath, commandLog, seqString, command[1]);
                    break;
                case "collect_data":
                    sequence++;
                    seqString = sequence.ToString();
                    CollectData(uplinkFilePath, MIBFilePath);
                    break;
                case "idle":
                    sequence++;
                    seqString = sequence.ToString();
                    Idle(uplinkFilePath, MIBFilePath);
                    break;
                case "safe_mode":
                    sequence++;
                    seqString = sequence.ToString();
                    SafeMode(uplinkFilePath, MIBFilePath);
                    break;
                case "display_event_log":
                    DisplayEventLog(EventLog);
                    break;
                case "display_hk_pf_log":
                    DisplayPfHkLog(HKLogPF);
                    break;
                case "display_hk_pl_log":
                    DisplayPlHkLog(HKLogPL);
                    break;
                case "display_command_log":
                    DisplayCommandLog(commandLog);
                    break;
                default:
                    Console.WriteLine("unknown command, type 'list' for a list of commands");
                    break;
                 
         






            }

        }

    }
}
