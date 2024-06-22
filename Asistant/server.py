from flask import Flask, request, send_file
from TTS.api import TTS

app = Flask(__name__)

@app.route('/')
def index():
    return "TTS API server v0.01"

@app.route('/audio')
def get_audio():
    return send_file("assistant.wav")

@app.route('/tts', methods=['POST'])
def create_tts():
    answer = request.json["message"]

    #"{"message": "message text"}"

    tts = TTS(model_name="tts_models/en/jenny/jenny", progress_bar=False, gpu=True)
    tts.tts_to_file(text=answer, file_path="assistant.wav")

    return "SUCESS"


if __name__ == "__main__":
    app.run(debug=True)
