using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	public Sound[] zombieNoises;
	
	public Sound[] m_PistolSounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

		foreach (Sound s in zombieNoises)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
		
		foreach (Sound s in m_PistolSounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
	
	public void PlayZombieNoise()
	{
		var s = zombieNoises[Random.Range(0, zombieNoises.Length)];
		PlaySound(s);
	}

	public void PlayPistolSound()
	{
		var s = m_PistolSounds[Random.Range(0, m_PistolSounds.Length)];
		PlaySound(s);
	}

	private void PlaySound(Sound s)
	{
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		
		s.source.volume = s.volume * (1f + Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
	
	

}
