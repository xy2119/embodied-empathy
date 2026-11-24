# AR app (Unity)

The Unity project for the Embodied Empathy iOS app. It was developed with **Unity 2022.3.62f2** (2022.3 LTS) using AR Foundation 5.2 and ARKit 5.2.

## Scenes and scripts

| Folder | What it does |
|---|---|
| `Assets/AvatarSelector/` | Home screen. Users generate a child avatar from a photo through MetaPerson Creator (`MobileUnitySampleHandler.cs`), get an 8-digit share code and choose a therapist avatar (`TherapistAvatarSelect.cs`). |
| `Assets/ChatbotInteract/` | Virtual therapy room. `ChatbotController.cs` sends messages to Gemini (`gemini-2.5-flash`) and handles Google Speech-to-Text and Text-to-Speech. `EmotionClassifier.cs` posts each user message to the emotion classifier. `ActionController.cs`, `FacialSwitcher.cs` and `AnimationManager.cs` apply the matching facial expression and body animation to both avatars. |
| `Assets/EmotionDisplay/` | Facial-expression presets (`Expressions/*.asset`) and animator controllers. |
| `Assets/ARDisplay/` | AR mode: places the child avatar on a detected plane (`ARAvatarPlacement.cs`). |
| `Assets/EnvLoader.cs` | Loads `Assets/Resources/env.txt` into environment variables before the first scene loads. |

## Setup

1. Download the third-party assets in [`../docs/THIRD_PARTY.md`](../docs/THIRD_PARTY.md).
2. Copy `env.example.txt` to `Assets/Resources/env.txt` and fill in:
   - `GEMINI_API_KEY`: a Google Cloud API key with the **Generative Language API**, **Cloud Speech-to-Text API** and **Cloud Text-to-Speech API** enabled
   - `METAPERSON_CLIENT_ID` / `METAPERSON_CLIENT_SECRET`: your AvatarSDK developer credentials
3. Open this folder in Unity Hub with Unity 2022.3 LTS.
4. Open `Assets/ChatbotInteract/ChatbotScene.unity`, select the object with the `EmotionClassifier` component, and set **Emotion Classifier Url** to your deployed classifier (`https://<your-service>/evaluate`). See [`../emotion_classifier`](../emotion_classifier).

> `env.txt` is git-ignored. Never commit API keys. Anything in `Resources/` is bundled into the app build, so use keys restricted to these APIs and to your app.

## Building for iOS

1. Go to **File → Build Settings**, choose **iOS** and click **Switch Platform**.
2. Make sure **ARKit** is enabled under **Project Settings → XR Plug-in Management → iOS**.
3. Click **Build**, open the generated Xcode project, set up signing and your provisioning profile, and run it on an ARKit-capable iPhone.

Android is not supported yet. AR Foundation does support ARCore, but this project has only been configured and tested on iOS.

## Optional: local LLM with Ollama

`ChatbotController.cs` contains a commented-out Ollama variant (an OpenAI-compatible `/v1/completions` endpoint, model `llama3.2`). To use a local model instead of Gemini:

```bash
OLLAMA_HOST="0.0.0.0" ollama serve
ngrok http 11434     # exposes the local server to the phone
```

Then uncomment the Ollama section and set `ollamaUrl` to `<ngrok_url>/v1/completions`.
