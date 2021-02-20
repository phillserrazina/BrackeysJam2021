using UnityEngine.Audio;
using System;
using UnityEngine;

namespace Lucerna.Audio
{
	public class AudioManager : MonoBehaviour
	{
		// VARIABLES
		public static AudioManager instance;

		public AudioMixer audioMixer;
		public AudioMixerGroup mixerGroup;

		public Sound[] sounds;

		// EXECUTION FUNCTIONS
		private void Awake() {
			if (instance != null && instance != this) {
				Destroy(gameObject);
			}
			if (instance == null) {
				instance = this;
				DontDestroyOnLoad(gameObject);
			}

			InitSounds();
		}

		// METHODS
		private void InitSounds() {
			foreach (Sound s in sounds) {
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.loop = s.loop;

				s.source.outputAudioMixerGroup = s.mixerGroup;
			}
		}

		/// <summary>
		/// Plays target sound.
		/// If index > 0, a sound at random is played with the same name.
		/// Example: "Shoot 3", will choose a random sound if there are 3 sounds called "Shoot 1", "Shoot 2" and "Shoot 3"
		/// </summary>
		/// <param name="sound"></param>
		/// <param name="index"></param>
		public void Play(string sound, int index=-1)
		{
			// Choose random sound based on index
			if (index > 0) sound += " " + UnityEngine.Random.Range(1, index+1);

			// Find sound in sound array
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null) {
				UnityEngine.Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			// Set volume and pitch
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			// Play it
			s.source.Play();
		}

		public bool IsPlaying(string sound, int index=-1, bool checkAll=false) {
			Sound s = null;

			if (index > 0 && checkAll) {
				for (int i = 0; i < index; i++) {
					string currSound = sound + " " + (i+1);
					s = Array.Find(sounds, item => item.name == currSound);
					if (s.source.isPlaying) return true;
				}

				return false;
			}
			
			if (index > 0) sound = sound + " " + index;
			s = Array.Find(sounds, item => item.name == sound);
			return s.source.isPlaying;
		}

		public void Stop(string sound, int index=-1, bool stopAll=false) {
			Sound s = null;

			if (index > 0 && stopAll) {
				for (int i = 0; i < index; i++) {
					string currSound = sound + " " + (i+1);
					s = Array.Find(sounds, item => item.name == currSound);
					if (s.source.isPlaying) s.source.Stop();
				}

				return;
			}

			if (index > 0) sound = sound + " " + index;
			s = Array.Find(sounds, item => item.name == sound);
			s.source.Stop();
		}

		public void StopAll() { foreach (Sound s in sounds) s.source.Stop(); }

		public void StopAllInMixer(string mixerName) {
			Sound[] ms = Array.FindAll(sounds, item => item.mixerGroup.name == mixerName);
			foreach (Sound s in ms) s.source.Stop();
		}
	}
}

