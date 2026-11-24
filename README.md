<h1 align="center">Embodied Empathy</h1>

<p align="center">
  <b>A Multimodal AR and LLM-Powered System for Self-Attachment Psychotherapy with Self-Initiated Humour</b>
</p>

<p align="center">
  Xinyan Ye, Gwyneth Phang, Anandha Gopalan, Abbas Edalat<br>
  Imperial College London
</p>

<p align="center">
  <i> International Conference on Multimodal Interaction (ICMI Companion '26) · Napoli, Italy</i>
</p>

<p align="center">
  <a href="https://doi.org/10.1145/3776591.3838179"><img src="https://img.shields.io/badge/ACM-10.1145%2F3776591.3838179-blue" alt="ACM DOI"></a>
  <a href="https://arxiv.org/abs/2608.02283"><img src="https://img.shields.io/badge/arXiv-2608.02283-b31b1b" alt="arXiv"></a>
  <img src="https://img.shields.io/badge/Unity-2022.3%20LTS-black?logo=unity" alt="Unity 2022.3">
  <img src="https://img.shields.io/badge/platform-iOS%20(ARKit)-lightgrey?logo=apple" alt="iOS">
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-green" alt="MIT License"></a>
</p>

<p align="center">
  <img src="docs/figures/teaser.jpg" alt="Left: a childhood photo becomes a 3D child avatar, and the user picks a therapist avatar. Middle: the virtual therapy room with the chat above both avatars. Right: AR mode, with the child avatar standing in the user's living room." width="90%">
</p>

<p align="center"><sub>Child avatar from a photo and therapist choice · Virtual therapy room · AR mode</sub></p>

## Demo video

<p align="center">
  <a href="https://github.com/xy2119/embodied-empathy/releases/download/v1.0.0/embodied-empathy-demo.mp4">
    <img src="docs/figures/demo_preview.gif" alt="Preview of the app demo at 2x speed: avatar creation, therapist selection, chatting with emotion mirroring, and AR mode" width="240">
  </a>
  <br>
  <sub>▶ <a href="https://github.com/xy2119/embodied-empathy/releases/download/v1.0.0/embodied-empathy-demo.mp4"><b>Watch the full demo with sound (3 min, MP4)</b></a> · the preview above plays at 2x speed</sub>
</p>

The demo walks through the full flow: creating the child avatar in MetaPerson Creator, choosing a therapist, chatting in the virtual therapy room while both avatars mirror the detected emotion, and placing the child avatar in the room with AR.

---

## Overview

**Embodied Empathy** is an iOS app that delivers the two-stage **Self-Attachment Technique (SAT)** and **Self-Initiated Humour Protocol (SIHP)** in a single, embodied experience. Earlier SAT/SIHP pilots split the experience across a Google Cardboard VR avatar and a separate rule-based web chatbot. This system brings everything into one app:

- 🧒 **Personalised childhood-self avatar.** Users upload a childhood photo, and [MetaPerson](https://metaperson.avatarsdk.com/) turns it into a customisable 3D child avatar. Avatars are stored as GLB and can be reloaded with an 8-digit share code.
- 🧑‍⚕️ **Choice of therapist avatar.** Users pick a virtual therapist from a diverse gallery.
- 🤖 **LLM-driven therapist.** Google **Gemini** is prompted to act as a supportive SAT/SIHP facilitator. It validates, reiterates and labels the user's feelings (mirroring and containment) and recommends exercises.
- 🎭 **Automated emotion mirroring.** A fine-tuned **RoBERTa** classifier reads each user message and assigns it to one of **7 emotion categories** (happy, sad, angry, fearful, disgusted, surprised, neutral). The child avatar mirrors the detected emotion through FACS-style blendshapes and Mixamo body animations, and the therapist shows a calm, regulated empathic response.
- 🗣️ **Voice in and out.** Users can press and hold the mic to speak (Google Speech-to-Text). Every therapist reply is read aloud (Google Text-to-Speech) in a voice matched to the therapist's persona.
- 📱 **On-demand AR.** Using AR Foundation and ARKit, users can place their childhood self in their own room.

## System architecture

### User flow

<p align="center">
  <img src="docs/figures/user_flow.png" alt="User flow: Start, create childhood avatar, select therapist avatar, share your feelings with the chatbot, child and therapist avatar mirror user emotions, chatbot recommends an appropriate SIHP exercise, then either loop back to sharing feelings or End." width="95%">
</p>

### System components

<p align="center">
  <img src="docs/figures/system_architecture.png" alt="System components: inside Unity are Avatar Display, Chatbot, Emotion Rendering and AR for Childhood Avatar; Avatar Display feeds Emotion Rendering and AR. The Chatbot sends user text to Emotion Recognition on Google Cloud, which returns the emotion to Emotion Rendering." width="70%">
</p>

The app runs in **Unity**, and emotion recognition runs on **Google Cloud**. Each user message goes to Gemini for the therapist's reply, which is also read aloud with Google TTS. At the same time, the message goes to the cloud-hosted RoBERTa classifier, and the emotion it detects drives the expressions and animations on both avatars.

<p align="center">
  <img src="docs/figures/emotion_mirroring.jpg" alt="Three app screenshots: the therapist greets the user; both avatars look happy after a positive message; both look sad after a negative one." width="70%">
  <br><sub>Emotion mirroring: session greeting · positive message · negative message</sub>
</p>

### Emotion categories and label mapping

The avatars express **7 emotion categories**. The classifier (double fine-tuned RoBERTa; 94.96% accuracy and 95.10% macro-F1 in the paper) predicts 12 fine-grained labels. The app then groups these labels into the 7 categories, and each category has its own facial-expression preset and body animation.

The child avatar mirrors the user's emotion directly. The therapist mirrors positive emotions and answers every negative emotion with the same calm empathic expression, as a real therapist would. The mapping is set in the Inspector on the two `FacialSwitcher` components in `ChatbotInteract/ChatbotScene.unity`. Body animations are chosen in `AnimationManager.cs`, and `ActionController.cs` sends each label to both avatars. To change how labels are grouped, reassign the expression assets (`EmotionDisplay/Expressions/*.asset`) on those components.

## Repository structure

```
embodied-empathy/
├── ar-app/                  # Unity 2022.3 LTS project (iOS, AR Foundation + ARKit)
│   ├── Assets/
│   │   ├── AvatarSelector/  # child-avatar generation (MetaPerson), therapist selection, share codes
│   │   ├── ChatbotInteract/ # Gemini chat, STT/TTS, emotion requests, expression + animation control
│   │   ├── EmotionDisplay/  # facial-expression presets and animator controllers
│   │   ├── ARDisplay/       # AR placement of the child avatar
│   │   └── EnvLoader.cs     # loads API keys from Assets/Resources/env.txt at startup
│   ├── env.example.txt      # template for Assets/Resources/env.txt
│   └── README.md
├── emotion_classifier/      # Flask server for the RoBERTa emotion model (Docker, Cloud Run)
│   ├── app.py
│   ├── Dockerfile
│   ├── tokenizer/
│   └── README.md
└── docs/
    ├── THIRD_PARTY.md       # third-party assets you need to download yourself
    └── figures/
```

## Getting started

### Prerequisites

- **Unity 2022.3 LTS** (developed on 2022.3.62f2) with the iOS Build Support module
- **Xcode** and an ARKit-capable iPhone
- A **Google Cloud** project with these APIs enabled: Generative Language (Gemini), Cloud Speech-to-Text and Cloud Text-to-Speech
- A **MetaPerson / AvatarSDK** developer account (client ID and secret)
- **Python 3.10+** for the emotion classifier

### 1. Start the emotion classifier

```bash
cd emotion_classifier
pip install -r requirements.txt
# Download RoBERTa_12_emotions.pt (see emotion_classifier/README.md) into this folder
python app.py          # serves POST /evaluate on http://0.0.0.0:5001
```

Quick test:

```bash
curl -X POST -H "Content-Type: application/json" -d '{"text": "im happy"}' http://localhost:5001/evaluate
```

To deploy it on Google Cloud Run, see [`emotion_classifier/README.md`](emotion_classifier/README.md).

### 2. Configure and build the Unity app

1. Download the third-party assets listed in [`docs/THIRD_PARTY.md`](docs/THIRD_PARTY.md) and place them at the paths given there.
2. Copy `ar-app/env.example.txt` to `ar-app/Assets/Resources/env.txt` and fill in your keys. This file is git-ignored.
3. Open `ar-app/` in Unity Hub.
4. In `ChatbotInteract/ChatbotScene.unity`, select the object with the `EmotionClassifier` component and set **Emotion Classifier Url** to your classifier endpoint (`http(s)://<host>/evaluate`). A phone cannot reach `localhost` on your computer, so use your machine's LAN IP or the Cloud Run URL.
5. Go to **File → Build Settings → iOS → Build**, then open the generated Xcode project, set up signing and run it on your device.

See [`ar-app/README.md`](ar-app/README.md) for more details.

## User study

We ran an eight-day remote study with **16 non-clinical adults** who used their own iPhones for 20–45 minutes a day. Days 1–2 focused on self-attachment with the child avatar. Days 3–8 were SIHP exercises with the LLM therapist, in the virtual room or in AR.

| Finding | Evidence (mean score, 0–100) |
|---|---|
| Personalised avatars anchor the bond, but realism lags behind | Therapist display 67.2 · Child avatar satisfaction 64.1 · Child avatar realism 48.4 |
| Emotion mirroring engages users when it is accurate, but misfires hurt | Emotion clarity 57.8 · Perceived empathy 59.4 |
| Voice output helps, and voice input divides users | TTS usefulness 64.1 (SD 27.3) · Voice input usefulness 54.7 (SD 37.9) |
| Users want a proactive facilitator, not a reactive responder | 87.5% endorsed the app for emotional self-regulation · Exercise engagement 68.8 · Error recovery 50.0 |

**Design implications:** (1) personalise the avatar first, and use AR in short, guided episodes; (2) moderate the intensity of mirroring and let users correct a wrong emotion; (3) prioritise the therapist's voice, while keeping typing as a first-class input; (4) make the therapist proactive and recover gracefully from errors.

> ⚠️ **Disclaimer.** This is a research prototype. It is not a medical device and is not a substitute for professional mental-health care. The general-purpose LLM it uses has not been clinically validated.

## Citation

If you find this work useful, please cite:

```bibtex
@inproceedings{ye2026embodied,
  title     = {Embodied Empathy: A Multimodal AR and LLM-Powered System for Self-Attachment Psychotherapy with Self-Initiated Humour},
  author    = {Ye, Xinyan and Phang, Gwyneth and Gopalan, Anandha and Edalat, Abbas},
  booktitle = {Companion Proceedings of the International Conference on Multimodal Interaction (ICMI Companion '26)},
  year      = {2026},
  address   = {Napoli, Italy},
  publisher = {Association for Computing Machinery},
  doi       = {10.1145/3776591.3838179}
}
```

arXiv version:

```bibtex
@misc{ye2026embodiedarxiv,
  title         = {Embodied Empathy: A Multimodal AR and LLM-Powered System for Self-Attachment Psychotherapy with Self-Initiated Humour},
  author        = {Ye, Xinyan and Phang, Gwyneth and Gopalan, Anandha and Edalat, Abbas},
  year          = {2026},
  eprint        = {2608.02283},
  archivePrefix = {arXiv},
  primaryClass  = {cs.HC}
}
```

## Acknowledgements

The app was originally implemented by Gwyneth Phang in [gphang/ar-psychotherapy](https://github.com/gphang/ar-psychotherapy). This repository is the companion code release for the paper.

We thank [AvatarSDK](https://avatarsdk.com/) for MetaPerson, the Empowered Human Foundation, and all study participants. The work was carried out with the Algorithmic Human Development Group at Imperial College London.

## License

The code in this repository is released under the [MIT License](LICENSE). This covers only the code and the files in this repository. Third-party assets and services, such as MetaPerson/AvatarSDK, Mixamo, UniWebView, Google Cloud APIs and the fonts and models listed in [`docs/THIRD_PARTY.md`](docs/THIRD_PARTY.md), are governed by their own licences and terms. The paper is published under CC BY 4.0.

## Contact

Xinyan Ye: xinyan.ye19@imperial.ac.uk
