using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftDialogue {

    public class DialogueAudioManager : MonoBehaviour {

        public int audioChannelCount = 8;
        public bool allowFade = true;
        public float fadeDuration = 0.5f;
        public bool allowCrossfade = false;
        private bool isFading = false;
        private float fadeProgression = 0;

        private Queue<AudioSource> audioSources;
        private Queue<AudioSource> musicSources;

        // Use this for initialization
        void Start() {
            audioSources = new Queue<AudioSource>();
            for (int i = 0; i < audioChannelCount; ++i) {
                GameObject obj = new GameObject();
                obj.transform.SetParent(transform);
                AudioSource source = obj.AddComponent<AudioSource>();
                source.loop = false;
                source.bypassEffects = true;
                source.bypassListenerEffects = true;
                source.bypassReverbZones = true;
                audioSources.Enqueue(source);
            }
            musicSources = new Queue<AudioSource>();
            for (int i = 0; i < (allowFade ? 2 : 1); ++i) {
                GameObject obj = new GameObject();
                obj.transform.SetParent(transform);
                AudioSource source = obj.AddComponent<AudioSource>();
                source.loop = true;
                source.bypassEffects = true;
                source.bypassListenerEffects = true;
                source.bypassReverbZones = true;
                musicSources.Enqueue(source);
            }
        }

        // Update is called once per frame
        void Update() {
            if (isFading)
                FadeAdjustement(Time.deltaTime);
        }

        public void PlayAudio(AudioClip audio) {
            AudioSource source = audioSources.Dequeue();
            source.Stop();
            source.clip = audio;
            source.Play();
            audioSources.Enqueue(source);
        }

        public void PlayAudio(AudioClip audio, Vector3 position) {
            AudioSource source = audioSources.Dequeue();
            source.Stop();
            source.clip = audio;
            source.transform.position = position;
            source.Play();
            audioSources.Enqueue(source);
        }

        public void PlayMusic(AudioClip music) {
            if (allowCrossfade) {
                DoCrossfade(music);
            } else if (allowFade) {
                DoFade(music);
            } else {
                DoPlayMusic(music);
            }
        }

        public void StopAudio() {
            for (int i = 0; i < audioSources.Count; ++i) {
                AudioSource source = audioSources.Dequeue();
                source.Stop();
                source.clip = null;
                audioSources.Enqueue(source);
            }
        }

        public void StopMusic() {
            for (int i = 0; i < musicSources.Count; ++i) {
                AudioSource source = musicSources.Dequeue();
                source.Stop();
                source.clip = null;
                musicSources.Enqueue(source);
            }

        }

        private void DoPlayMusic(AudioClip music) {
            AudioSource source = musicSources.Dequeue();
            source.Stop();
            source.clip = music;
            source.Play();
            musicSources.Enqueue(source);
        }

        private void DoFade(AudioClip music) {
            AudioSource sourceA = musicSources.Dequeue();
            AudioSource sourceB = musicSources.Dequeue();
            if (sourceA.isPlaying) {
                sourceB.Stop();
                sourceB.clip = music;
                fadeProgression = 0;
                musicSources.Enqueue(sourceA); // > A
                musicSources.Enqueue(sourceB); // > B
            } else if (sourceB.isPlaying) {
                sourceA.Stop();
                sourceA.clip = music;
                fadeProgression = 0;
                musicSources.Enqueue(sourceB); // > A
                musicSources.Enqueue(sourceA); // > B
            } else {
                sourceA.Stop();
                sourceA.clip = music;
                sourceA.volume = 0;
                fadeProgression = 1;
                musicSources.Enqueue(sourceB); // > A
                musicSources.Enqueue(sourceA); // > B
            }
            isFading = true;
        }

        private void DoCrossfade(AudioClip music) {
            AudioSource sourceA = musicSources.Dequeue();
            AudioSource sourceB = musicSources.Dequeue();
            if (sourceA.isPlaying) {
                sourceB.Stop();
                sourceB.clip = music;
                sourceB.Play();
                fadeProgression = 0;
                musicSources.Enqueue(sourceA); // > A
                musicSources.Enqueue(sourceB); // > B
            } else if (sourceB.isPlaying) {
                sourceA.Stop();
                sourceA.clip = music;
                sourceA.Play();
                fadeProgression = 0;
                musicSources.Enqueue(sourceB); // > A
                musicSources.Enqueue(sourceA); // > B
            } else {
                sourceA.clip = music;
                sourceA.Play();
                fadeProgression = 0;
                musicSources.Enqueue(sourceB); // > A
                musicSources.Enqueue(sourceA); // > B
            }
            isFading = true;
        }

        private void FadeAdjustement(float deltaTime) {
            float step = deltaTime / fadeDuration;
            AudioSource sourceA = musicSources.Dequeue();
            AudioSource sourceB = musicSources.Dequeue();
            fadeProgression += step;
            if (allowCrossfade) { // do crossfade
                if (fadeProgression >= 1) {
                    sourceA.Stop();
                    sourceA.clip = null;
                    sourceB.volume = 1;
                    isFading = false; // fading is done
                } else {
                    sourceA.volume = 1 - fadeProgression;
                    sourceB.volume = fadeProgression;
                }
            } else { // do classic fade
                if (fadeProgression >= 2) {
                    sourceB.volume = 1;
                    isFading = false; // fading is done
                } else if (fadeProgression >= 1) { // fade in
                    if (!sourceB.isPlaying)
                        sourceB.Play();
                    sourceB.volume = fadeProgression - 1;
                    if (sourceA.isPlaying) {
                        sourceA.Stop();
                        sourceA.clip = null;
                    }
                } else { // fade out
                    sourceA.volume = 1 - fadeProgression;
                }
            }
            musicSources.Enqueue(sourceA);
            musicSources.Enqueue(sourceB);
        }
    }

}
