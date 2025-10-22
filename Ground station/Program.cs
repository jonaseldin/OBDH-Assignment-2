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
        Console.WriteLine("display_event_log: displays the contents of the log file");
        Console.WriteLine("display_hk_pl_log");
        Console.WriteLine("display_hk_pf_log");
        Console.WriteLine("display_command_log");

    }


    static void ClearSignalFile(string signalFilePath)
    {
        File.WriteAllText(signalFilePath, ""); //i had bigger ideas for this function, but it didnt turn out. i will leave it here cuz its funny how stupid and useless it is
    }





    static void TargetPoint(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[0], sendTime, groundTime);

        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine+"\n");




    }

    static void CollectData(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[1], sendTime, groundTime);

        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine + "\n");
    }

    static void Idle(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[2], sendTime, groundTime);

        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine + "\n");
    }

    static void SafeMode(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[3], sendTime, groundTime);

        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine + "\n");
    }

    static void incorrectFormatTest(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss");
        string[] lines = File.ReadAllLines(MIBFilePath);
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        using StreamWriter sw = new StreamWriter(fs);

        string logLine = string.Join(" ", sequence, lines[4], sendTime, groundTime);
        
        sw.WriteLine(logLine);
        File.AppendAllText(commandLog, logLine + "\n");
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


        string uplinkFilePath = "uplink.txt";
        string MIBFilePath = "MIB.txt";
        string downlinkPath = "downlink.txt";
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
                List<string> lines = new List<string>();
                using (var fs = new FileStream(downlinkPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (line == null) break;
                        lines.Add(line);
                    }
                }

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
                        
                        log3.Add(line + " " + groundTime);
                        confirmPrinted = true; 

                    }
                }

                if (log1.Count > 0) File.AppendAllLines(HKLogPL, log1);
                if (log2.Count > 0) File.AppendAllLines(HKLogPF, log2);
                if (log3.Count > 0) File.AppendAllLines(EventLog, log3);

                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        using (var tf = new FileStream(downlinkPath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite)) { }
                        break; // cleared successfully
                    }
                    catch (IOException)
                    {
                        await Task.Delay(50);
                    }
                }
               

                await Task.Delay(1000);
            }
        }
        );


        //this is the main loop for accepting inputted telecommands
        bool running = true;
        while (running)
        {

            Console.Write("Input command: ");
            string cmnd = Console.ReadLine();
            string[] command = cmnd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string seqString;

        


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
                    CollectData(uplinkFilePath, MIBFilePath, commandLog, seqString, command[1]);
                    break;
                case "idle":
                    sequence++;
                    seqString = sequence.ToString();
                    Idle(uplinkFilePath, MIBFilePath, commandLog, seqString, command[1]);
                    break;
                case "safe_mode":
                    sequence++;
                    seqString = sequence.ToString();
                    SafeMode(uplinkFilePath, MIBFilePath, commandLog, seqString, command[1]);
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
                case "test_function":
                    sequence++;
                    seqString = sequence.ToString();
                    incorrectFormatTest(uplinkFilePath, MIBFilePath, commandLog, seqString, command[1]);
                    break;
                default:
                    Console.WriteLine("Error: unknown command, type 'list' for a list of commands");
                    continue;
            }


        }

    }
}
