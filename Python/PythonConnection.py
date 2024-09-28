import socket

import whisper
import os

# Create a TCP/IP socket
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_address = ('localhost', 65432)  # Bind to localhost and a port
server_socket.bind(server_address)

# Listen for incoming connections
server_socket.listen(1)
print("Waiting for a connection...")

while True:
    # Wait for a connection
    connection, client_address = server_socket.accept()
    try:
        print(f"Connected to {client_address}")

        # Receive message from Unity
        data = connection.recv(1024).decode()
        print(f"Received message from Unity: {data}")

        # Send a response back to Unity
        response = "Hello from Python!"
        connection.sendall(response.encode())

    finally:
        # Clean up the connection
        connection.close()
