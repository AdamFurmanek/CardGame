using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    public List<GameObject> sounds;

    public void PlaySound(int index)
    {
        sounds[index].GetComponent<AudioSource>().Play();
    }

    public void PlaySound(string name)
    {
        foreach(GameObject sound in sounds)
        {
            if (sound.GetComponent<AudioSource>().clip.name.CompareTo(name) == 0)
                sound.GetComponent<AudioSource>().Play();
        }
    }
}
