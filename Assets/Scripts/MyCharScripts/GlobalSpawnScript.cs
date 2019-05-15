using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpawnScript : MonoBehaviour
{
    public List<AmmoSpawn> AmmoPlatformList;
    public int timerLimit = 1800;
    public int timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1;
        if (timer == timerLimit)
        {
            AmmoPlatformList[Random.Range(0, AmmoPlatformList.Count)].CreateAmmoBox();
            timer = 0;
        }
    }
}