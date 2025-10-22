import time 
import random 


# Files with data
InFile = r"C:\Users\wolinn-2\source\repos\Ground station\Ground station\bin\Debug\net8.0\PF_to_PL.txt"  # Platform -> Payload
OutFile = r"C:\Users\wolinn-2\source\repos\Ground station\Ground station\bin\Debug\net8.0\PL_to_PF.txt"  # Payload -> Platform

# Sequence starting at 0
seq_counter = 0

# Begin with the payload being turned off 
state = "OFF"


# Housekeeping Telemetry

# Onboard time
def OBT():
    return time.strftime("%H:%M:%S", time.localtime()) # Wall clock time of the computer 

# Sequence
def next_seq():
    global seq_counter
    seq_counter += 1
    return seq_counter # Sequence counter för TM/sequence verify

# Simulated HK
def generate_payload_data():

    temperature_C = round(random.uniform(8.0, 42.0), 2)
    voltage_V = round(random.uniform(4.8, 5.1), 2)
    signal_strength = round(random.uniform(50.0, 100.0), 2)

    return temperature_C, voltage_V, signal_strength

# Read incomming file and clear it after copying data
def read_and_clear_cmd():
    try:
        with open(InFile, "r+", encoding = "utf-8") as InF:
            cmd = InF.read().strip()
           
            # Cut everything out in the file starting from position 0
            InF.seek(0)
            InF.truncate()
            return cmd
        
    except FileNotFoundError:
        return ""
    
# Function to write back to PF
def write_to_PF(s):
    with open(OutFile, "a", encoding = "utf-8") as OutF:
        OutF.write(s + "\n")




# Payload States

# Counter for taking images
last_img_time = 0

# Main loop
while True:

    # Calling on function to read and clear the incomming file
    command = read_and_clear_cmd()

    # Checking which state the payload should be in ackordning to the TC
    if command:
        if command == "payload idle":
            state = "IDLE"
        elif command == "payload off":
            state = "OFF"
        elif command == "collect data":
            state = "COLLECTING DATA" 
        # If the command is none of the above
        else: 
            seq = next_seq()
            write_to_PF(f"{seq} Command is invalid {OBT()}")

    # Save current time as t
    t = OBT()

    # What should happen in each state
    if state == "OFF":
        # When payload is off, send confirmation that payload is turned off along with sequence count
        seq = next_seq()
        write_to_PF(f"{seq} Payload is OFF {t}")

    # If payload is not turned off
    else: 
        # Get generated HK
        temp, volt, sig = generate_payload_data()
        # Send HK to PF along with sequence count
        seq = next_seq()
        write_to_PF(f"{seq} {temp}°C {volt}V {t} {sig}%")

        # If state is "collecting data", image should be taken every 5 seconds
        if state == "COLLECTING DATA":
            # Finding current time
            now = time.time()
            # Taking image of it has been 5 or more seconds
            if now - last_img_time >= 5:
                # Send message simulating image taken
                seq = next_seq()
                write_to_PF(f"{seq} Image taken {t}")
                last_img_time = now

        # Send confirmation to PF that payload is either idle or collecting data
        if state == "IDLE":
            message = "Payload is IDLE"
        else:
            message = "Payload is COLLECTING DATA"
        seq = next_seq()
        write_to_PF(f"{seq} {message} {t}")

    # Repeat loop every second
    time.sleep(1)
