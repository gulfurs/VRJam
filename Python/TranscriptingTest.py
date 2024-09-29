import pyaudio
import wave
import whisper
import warnings

warnings.filterwarnings("ignore", category=UserWarning)  # Suppress the FP16 warning

# Set up parameters for recording
FORMAT = pyaudio.paInt16
CHANNELS = 1
RATE = 16000
CHUNK = 1024
RECORD_SECONDS = 10  # Shorten for quick tests
OUTPUT_FILE = "output.wav"

def record_audio(file_path):
    audio = pyaudio.PyAudio()

    print("Recording...")

    stream = audio.open(format=FORMAT, channels=CHANNELS,
                        rate=RATE, input=True,
                        frames_per_buffer=CHUNK)

    frames = []

    for i in range(0, int(RATE / CHUNK * RECORD_SECONDS)):
        data = stream.read(CHUNK)
        frames.append(data)

    print("Finished recording.")

    # Stop and close the stream
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
    # Load the Whisper model
    model = whisper.load_model("tiny")

    # Force the language to English
    print("Transcribing audio in English...")
    result = model.transcribe(file_path, language="en")  # Force transcription in English
    return result['text']

# Record audio and save to output.wav
record_audio(OUTPUT_FILE)

# Transcribe the recorded audio in English
transcription = transcribe_audio(OUTPUT_FILE)
print(f"Transcription: {transcription}")
