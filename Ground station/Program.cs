// See https://aka.ms/new-console-template for more information

using System;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


class Program
{
   

    // function to display all the relevant commands
    static void DisplayHelp()
    {
        Console.WriteLine("\nexit: exit the program");
        Console.WriteLine("target_point: Point sattelite towards target");
        Console.WriteLine("collect_data: sattelite will start collecting data, telemetry will be displayed, requires target point executed");
        Console.WriteLine("idle: sets the payload to its idle mode");
        Console.WriteLine("safe_mode: sets the sattelite into safe mode");
        Console.WriteLine("display_event_log: displays the contents of the log file");
        Console.WriteLine("display_hk_pl_log: display the payload housekeeping log file");
        Console.WriteLine("display_hk_pf_log: display platform housekeeping log file");
        Console.WriteLine("display_command_log: display command log file\n");

    }


    static void ClearSignalFile(string signalFilePath)
    {
        File.WriteAllText(signalFilePath, ""); //i had bigger ideas for this function, but it didnt turn out. i will leave it here cuz its funny how stupid and useless it is
    }




    //function to set platform to target pointing mode. 
    static void TargetPoint(string signalFilePath, string MIBFilePath, string commandLog, string sequence, string sendTime)
    {
        string groundTime = DateTime.Now.ToString("HH:mm:ss"); //get time in hh:mm:ss format
        string[] lines = File.ReadAllLines(MIBFilePath); //read all lines in MIB file into an array
        using FileStream fs = new FileStream(signalFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite); //open uplink file and enable append, write, and allow others to read and write while it is open here
        using StreamWriter sw = new StreamWriter(fs);//create streamwriter using the above filestream to write lines

        string logLine = string.Join(" ", sequence, lines[0], sendTime, groundTime); //create the line that will be written using specifically line 1 in the MIB, removing excess spaces and making it a uniform format

        sw.WriteLine(logLine);//write to the uplink file
        File.AppendAllText(commandLog, logLine+"\n");//write to the log file




    }
    //same as the abovew function except using lines[1] instead. in hindsight i should have made a boilerplate function to reuse but whatever
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
    
    //same as above
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

    //same as above
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

    //this is a test function which calls for an incorrectly formatted function in the MIB file. maybe an intern wrote it or smth idk. anyway its just for testing the OBDH error handling
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


    //function for displaying the event log
    static void DisplayEventLog(string EventLog)
    {
        string[] log = File.ReadAllLines(EventLog);//read all lines in the event log into and array
        foreach (string logLine in log) //for each line in the array, print that line
        {
            Console.WriteLine(logLine);
        } 
            
    }


    //same as above but for the platform housekeeping log. once again probably should have made a boilerplate function for this too
    static void DisplayPfHkLog(string HKLogPF)
    {
        string[] log = File.ReadAllLines(HKLogPF);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }

    //same as above
    static void DisplayPlHkLog(string HKLogPL)
    {
        string[] log = File.ReadAllLines(HKLogPL);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }

    //same as above
    static void DisplayCommandLog(string commandLog)
    {
        string[] log = File.ReadAllLines(commandLog);
        foreach (string logLine in log)
        {
            Console.WriteLine(logLine);
        }
    }


    //main function that runs on startup
    static void Main()
    {
        Console.Write("Initializing:"); //inform user that the program is starting up


        //define all the filepath strings
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

        //calling the stupid pathetic function that nobody asked for
        ClearSignalFile(uplinkFilePath); //startup clear the file path, just for assignment



        //initialize the sequence and sequence string
        int sequence = 0;
        string seqString;


        //initialization process to get OBDH program to work. jonas made a bunch of code for the OBDH without testing so this will have to do
        sequence++;//increment sequence
        seqString = sequence.ToString();//translate sequence int into string
        Idle(uplinkFilePath, MIBFilePath, commandLog, seqString, "inst"); //call the idle function with all relevant filepaths and strings, sending execution time as instant which in our case is just the string "inst"


        //this is a parallell task that cyclycally reads the downlink file to look for incoming telemetry and writes it all down to the log file with an appended receival timestamp
        Task.Run(async () =>
        {
            
            while (true)
            {


                bool confirmPrinted = false; //initialize confirm printed bool so it doesnt print the same thing 1 gorillion times
                string groundTime = DateTime.Now.ToString("HH:mm:ss"); //get the time in hh:mm:ss in string format
                List<string> lines = new List<string>(); //create new list
                using (var fs = new FileStream(downlinkPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)) //create new file stream to allow others to read and write while my program has the file open
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)//reads through every line in the downlink file until an empty string and adds it to the line list
                    {
                        var line = sr.ReadLine();
                        if (line == null) break;
                        lines.Add(line);
                    }
                }

                //create a list for every telemetry category
                var log1 = new List<string>();
                var log2 = new List<string>();
                var log3 = new List<string>();
               
                


                //for every line in the lines list, check the first character and add it to the appropriate category list. if it is 4 specifically, print confirmation once and set confirm printed to true
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
                        Console.WriteLine(line);
                        log3.Add(line + " " + groundTime);
                        confirmPrinted = true; 

                    }
                }

                //write contents of all the lists to appropriate log files
                if (log1.Count > 0) File.AppendAllLines(HKLogPL, log1);
                if (log2.Count > 0) File.AppendAllLines(HKLogPF, log2);
                if (log3.Count > 0) File.AppendAllLines(EventLog, log3);


                //this basically just clears the downnlink file once it has read and logged everything, this is so that it doesnt read in all the old telemetry every update
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
               
                //loop around every second
                await Task.Delay(1000);
            }
        }
        );


        //this is the main loop for accepting inputted telecommands
        bool running = true;
        while (running)
        {

            
            string cmnd = Console.ReadLine();//accept input
            string[] command = cmnd.Split(' ', StringSplitOptions.RemoveEmptyEntries);//take the input, split it at each spacebar and put each word as an array element


            //check if no execution time is given, if none is given then set the second word in the command (the input time) to instant
            if (command.Length < 2)
            {
                Array.Resize(ref command, 2);
                command[1] = "inst";
            }


            //wait one second for a confirmation of OBDH recieval
            Thread.Sleep(1000);



            //big switch that looks for the first word in the command and calls relevant functions. it also increments the sequence each time one is called. If it doesnt recognize the input, it defaults to an error and tells you to type list
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
