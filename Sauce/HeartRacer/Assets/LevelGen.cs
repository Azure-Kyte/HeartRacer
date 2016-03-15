using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelGen : MonoBehaviour{

    class song
    {
        public float bpm { get; set; }
        public float offset {get; set; }
        public float totalTime { get; set; }
    }

    song testSong = new song();
    int interval;
    float time;
    public float timeperBeat;
    public float totalbeats;
    public GameObject[] g = new GameObject[1];

	// Use this for initialization
	void Start () {
        testSong.bpm = 126f;
        testSong.offset = 1.9f;
        testSong.totalTime = 268;
        timeperBeat = 60 / testSong.bpm;
        totalbeats = testSong.totalTime / timeperBeat;
        float offsetbeats = testSong.offset / timeperBeat;

        string[] items = File.ReadAllLines(@"./Assets/items.txt");
        int[] items2 = new int[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items2[i] = int.Parse(items[i]);
        }

        foreach (int iss in items2)
        {
            Object spawner;
            string n = "Object_" + iss.ToString();
            int r = Random.Range(0, 2);
            if (r == 0)
            {
                spawner = GameObject.Instantiate(g[0], new Vector3(0, 0.75f, 9.375f + offsetbeats + (2.38095f * iss)), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                spawner = GameObject.Instantiate(g[1], new Vector3(0, 2f, 9.375f + offsetbeats + (2.38095f * iss)), Quaternion.Euler(0, 0, 0));
            }
                spawner.name = n;
        }

        /*THIS CODE ENABLES RANDOM GENERATION IN A bar->bar>halfbar>bar format.
         * 
         * for (int i = 0; i < totalbeats - 16; i++)
        {
            if (i % 4 == 0 || i % 6 == 0)
            {
                
                //Velocity = 5, create level so that it syncs with that velocity;
                //1 second = 5.
                //1 beat is 0.47619 of a second.
                //Velocity = 2.34375 per beat. //2.38095
                Object spawner;
                string n = "Object_" + i.ToString();
                int r = Random.Range(0, 2);
                if (r == 0)
                    spawner = GameObject.Instantiate(g[0], new Vector3(0, 0.75f, 9.375f + offsetbeats + 2.34375f * i), Quaternion.Euler(0, 0, 0));
                else
                    spawner = GameObject.Instantiate(g[1], new Vector3(0, 2f, 9.375f + offsetbeats + 2.34375f * i), Quaternion.Euler(0, 0, 0));
                spawner.name = n;
            }
        }*/

    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > testSong.offset)
        {
            float ti = time - testSong.offset;
            if (ti > (timeperBeat) * interval)
            {
                interval++;
            }
        }

        
	}
}
