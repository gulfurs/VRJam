import pyaudio
import wave
import whisper
import socket
import warnings



warnings.filterwarnings("ignore", category=UserWarning)  # Suppress the FP16 warning

# Socket setup
HOST = '127.0.0.1'  # Localhost
PORT = 5000  # Port to listen on
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((HOST, PORT))
server_socket.listen(1)
print("Waiting for Unity connection...")

conn, addr = server_socket.accept()
print(f"Connected by {addr}")

# Audio recording parameters
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 16000
CHUNK = 1024
RECORD_SECONDS = 5
OUTPUT_FILE = "output.wav"

def record_audio(file_path):
    audio = pyaudio.PyAudio()
    print("Recording...")
    stream = audio.open(format=FORMAT, channels=CHANNELS, rate=RATE, input=True, frames_per_buffer=CHUNK)
    frames = []
    for i in range(0, int(RATE / CHUNK * RECORD_SECONDS)):
        data = stream.read(CHUNK)
        frames.append(data)
    print("Finished recording.")
    stream.stop_stream()
    stream.close()
    audio.terminate()
    
    # Save the recorded audio to a WAV file
    wf = wave.open(file_path, 'wb')
    wf.setnchannels(CHANNELS)
    wf.setsampwidth(audio.get_sample_size(FORMAT))
    wf.setframerate(RATE)
    wf.writeframes(b''.join(frames))
    wf.close()

def transcribe_audio(file_path):
    model = whisper.load_model("tiny")
    print("Transcribing audio in English...")
    result = model.transcribe(file_path, language="en")
    return result['text']

# Main loop to listen for triggers from Unity
try:
    while True:
        data = conn.recv(1024).decode('utf-8')
        if not data:
            break

        if data == "start":
            print("Start recording triggered by Unity.")
            record_audio(OUTPUT_FILE)
            transcription = transcribe_audio(OUTPUT_FILE)
            print(f"Transcription: {transcription}")

            # Send the transcription back to Unity
            conn.sendall(transcription.encode('utf-8'))

finally:
    conn.close()
    server_socket.close()