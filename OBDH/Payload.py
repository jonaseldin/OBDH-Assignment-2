import time 
import random 
import os



InFile = "PF_to_PL.txt"  # Platform -> Payload
OutFile = "PL_to_PF.txt"  # Payload -> Platform

seq_counter = 0
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

# Read incommin file and clear it efter copying data
def read_and_clear_cmd():
    try:
        with open(InFile, "r+", encoding = "utf-8") as InF:
            cmd = InF.read().strip()
            # Erase everything in the file
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

last_img_time = 0

while True:

    command = read_and_clear_cmd()

    if command:
        if command == "PAYLOAD IDLE":
            state = "IDLE"
        elif command == "PAYLOAD OFF":
            state = "OFF"
        elif command == "PAYLOAD COLLECT DATA":
            state = "COLLECTING DATA" 
        else: 
            seq = next_seq()
            write_to_PF(f"{seq} State command is invalid {OBT()}")

    t = OBT()

    if state == "OFF":

        seq = next_seq()
        write_to_PF(f"{seq} Payload is OFF {t}")

    else: 
        temp, volt, sig = generate_payload_data()
        seq = next_seq
        write_to_PF(f"{seq} {temp}°C {volt}V {t} {sig}%")

        if state == "COLLECTING DATA":
            now = time.time()
            if now - last_img_time >= 5:
                seq = next_seq()
                write_to_PF(f"{seq} Image taken {t}")
                last_img_time = now

        if state == "IDLE":
            message = "Payload is IDLE"
        else:
            message = "Payload is COLLECTING DATA"
        write_to_PF(f"{seq} {message} {t}")

    time.sleep(1)

   




