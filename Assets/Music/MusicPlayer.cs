using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<GameObject> songs;

    void Start()
    {
        StartCoroutine(PlaySong());
    }

    IEnumerator PlaySong()
    {
        while (true)
        {
            int randomSong = Random.Range(0, songs.Count);
            for (int i = randomSong; i < songs.Count + randomSong; i++)
            {
                int j = i;
                if (j >= songs.Count)
                    j -= songs.Count;
                songs[j].GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(songs[j].GetComponent<AudioSource>().clip.length);
            }
        }
        

    }

}
