import azure.cognitiveservices.speech as speechsdk
from azure.cognitiveservices.speech.audio import AudioConfig, AudioOutputConfig

speech_key, speech_region = "f68d0b98d82f4ef1895f3f541548cd47", "centralindia"
audioConfig = AudioOutputConfig(filename="hello.wav")
speechConfig = speechsdk.SpeechConfig(subscription=speech_key, region=speech_region)
synthesizer = speechsdk.SpeechSynthesizer(speech_config=speechConfig, audio_config=audioConfig)

ssmlString = "<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>" + "<voice name = 'en-US-GuyNeural'>" + "Welcome to this factory facility, guest." + "</voice>" +"</speak>"

result = synthesizer.speak_ssml_async(ssmlString)