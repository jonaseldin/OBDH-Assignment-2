import time 
import random 
import os


InFile = "to_payload.txt"  # OBDH -> Payload
OutFile = "payload_to_OBDH.txt"  # Payload -> OBDH

while True:

    try:
        with open(InFile, "r", encoding = "utf-8") as payload_state:
            command = payload_state.read().strip()

    except FileNotFoundError:
        command = ""  # Empty string if file does not exist

    # If there is nothing in teh file, wait for 2 seconds and try again
    if not command:
        time.sleep(2)
        continue

    # Comparing text in file with commands
    if command == "PAYLOAD ON":
        message = "Payload is ON"
    elif command == "PAYLOAD OFF":
        message = "Payload is OFF"
    elif command == "PAYLOAD IDLE":
        message = "Payload is IDLE"
    # If the command is not valid
    else: 
        message = "Command is invalid"

    # Write a confirmation that the comman was received
    with open(OutFile, "w", encoding="utf-8") as payload_state:
        payload_state.write(message + "\n")

    # Check every other second
    time.sleep(2)
   
