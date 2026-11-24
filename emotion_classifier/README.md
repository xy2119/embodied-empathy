# Emotion classifier

A small Flask server that wraps a fine-tuned **RoBERTa-base** emotion classifier. The Unity app sends each user message to `POST /evaluate`. The predicted label then drives the facial expressions and body animations of both avatars.

## API

```
POST /evaluate
Content-Type: application/json

{"text": "I miss my childhood friend"}
→ {"emotion": "sadness"}
```

The server returns one of 12 labels: `fear`, `love`, `instability`, `disgust`, `disappointment`, `shame`, `anger`, `jealous`, `sadness`, `envy`, `joy`, `guilt`. The Unity app maps each label to a facial-expression preset (`ar-app/Assets/EmotionDisplay/Expressions/`) and a Mixamo animation.

## Model weights

The weights are too large for git. Download them and put the file in this folder:

- **RoBERTa (used by `app.py`)**: [`RoBERTa_12_emotions.pt`](https://drive.google.com/file/d/1cycrYd0S3Go7j3W2A50bJKQBn-oCdgZs/view?usp=sharing)
- **T5-small (smaller alternative, used by `classifier.py`)**: [`T5_small_emotion.pt`](https://drive.google.com/drive/folders/19uh2aIyxqzS-4LiagBouu6YuPYiOvaDz)

The tokenizer files are in `tokenizer/`. The base `roberta-base` model is downloaded from Hugging Face on first run.

## Run locally

```bash
pip install -r requirements.txt
python app.py      # http://0.0.0.0:5001
curl -X POST -H "Content-Type: application/json" -d '{"text": "im happy"}' http://localhost:5001/evaluate
```

## Deploy on Google Cloud Run

After you set up the gcloud CLI, build the Docker image, push it and deploy it to Cloud Run:

```bash
gcloud builds submit --tag gcr.io/$PROJECT_ID/$IMAGE_NAME
gcloud run deploy $IMAGE_NAME \
  --image gcr.io/$PROJECT_ID/$IMAGE_NAME \
  --platform managed \
  --region $REGION \
  --memory 2Gi \
  --allow-unauthenticated
```

`--allow-unauthenticated` makes the endpoint public, and anyone with the URL can call it and use up your quota. For anything beyond a short study, add authentication or set a maximum instance count.

## Files

| File | Purpose |
|---|---|
| `app.py` | Flask server with the RoBERTa classifier (used in the study) |
| `classifier.py` | Alternative T5-small classifier |
| `Dockerfile` | Container for Cloud Run (gunicorn on port 8080) |
| `evaluate_model/` | `EmpatheticPersonas.csv` and an evaluation script stub |
| `tokenizer/` | Byte-level BPE tokenizer files for RoBERTa |
