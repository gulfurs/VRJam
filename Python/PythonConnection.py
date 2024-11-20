import pyaudio
import numpy as np
import whisper
import socket
import io
import wave

# Socket setup
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind(('127.0.0.1', 5000))
server_socket.listen(1)
conn, _ = server_socket.accept()

# Audio recording parameters
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 16000
CHUNK = 512  # Smaller chunk size for faster processing
RECORD_SECONDS = 5

# Initialize PyAudio and Whisper model once
audio = pyaudio.PyAudio()
model = whisper.load_model("base")

def record_audio():
    stream = audio.open(format=FORMAT, channels=CHANNELS, rate=RATE, 
                       input=True, frames_per_buffer=CHUNK)
    frames = []
    
    for _ in range(0, int(RATE / CHUNK * RECORD_SECONDS)):
        frames.append(stream.read(CHUNK))
    
    stream.stop_stream()
    stream.close()
    
    # Convert audio to in-memory WAV
    wav_buffer = io.BytesIO()
    with wave.open(wav_buffer, 'wb') as wf:
        wf.setnchannels(CHANNELS)
        wf.setsampwidth(audio.get_sample_size(FORMAT))
        wf.setframerate(RATE)
        wf.writeframes(b''.join(frames))
    
    return wav_buffer.getvalue()

def transcribe_audio(audio_data):
    # Save audio data to a temporary file in memory
    with open('temp.wav', 'wb') as f:
        f.write(audio_data)
    
    # Transcribe using the pre-loaded model
    result = model.transcribe('temp.wav', language="en")
    return result['text']

try:
    while True:
        if conn.recv(1024).decode('utf-8') == "start":
            audio_data = record_audio()
            transcription = transcribe_audio(audio_data)
            conn.sendall(transcription.encode('utf-8'))
finally:
    conn.close()
    server_socket.close()
    audio.terminate()