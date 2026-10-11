using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
	public static float volume = soundEffect.volume;
	[Range(0, 1)]
	public float glassVolumePerc = 0.6f;

	public AudioClip[] glassAudios;
	public AudioClip[] crateHardAudios;
	public AudioClip[] crateSoftAudios;
	float lastCollision;
	float delay = 0.5f;

	private void Start()
    {
		lastCollision = 0;
    }

    void OnCollisionEnter(Collision other)
	{
        if (!PickUpController.beingHeld)
        {
			if (other.gameObject.name != "PlayerCollider" && other.gameObject.name != "Example player prefab")
			{
				if (lastCollision + delay <= Time.time)
				{
					lastCollision = Time.time;
					if (other.gameObject.tag == "Glass")
					{
						AudioClip randomGlassAudio = glassAudios[Random.Range(0, glassAudios.Length)];
						AudioSource.PlayClipAtPoint(randomGlassAudio, transform.position, volume * glassVolumePerc);
					}
					if (PickUpController.playHard)
					{
						AudioClip randomHardAudio = crateHardAudios[Random.Range(0, crateHardAudios.Length)];
						AudioSource.PlayClipAtPoint(randomHardAudio, transform.position, volume);
					}
					else
					{
						AudioClip randomSoftAudio = crateSoftAudios[Random.Range(0, crateSoftAudios.Length)];
						AudioSource.PlayClipAtPoint(randomSoftAudio, transform.position, volume);
					}
				}
			}
        }
		

	}
}
