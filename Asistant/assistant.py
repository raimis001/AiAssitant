from openai import OpenAI
from TTS.api import TTS

import pygame
import time

client = OpenAI(base_url="http://localhost:1234/v1", api_key="lm-studio")

completion = client.chat.completions.create(
  model="crusoeai/dolphin-2.9.1-llama-3-8b-GGUF",
  messages=[
    {"role": "system", "content": "Always answer in rhymes."},
    {"role": "user", "content": "Introduce yourself."}
  ],
  temperature=0.7,
)

answer = completion.choices[0].message.content
print(answer)

tts = TTS(model_name="tts_models/en/jenny/jenny", progress_bar=False, gpu=False)
tts.tts_to_file(text=answer, file_path="assistant.wav")

pygame.mixer.init()
pygame.mixer.music.load("assistant.wav")
pygame.mixer.music.play()

while pygame.mixer.music.get_busy():
    time.sleep(1)