using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class AmmoSpawn : MonoBehaviourPunCallbacks
{
    public static int ammoBoxCount;

    public GameObject _ammoBox;
    public Vector2 ammoSpawn;

    // Start is called before the first frame update
    void Start()
    {
        ammoBoxCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateAmmoBox()
    {
        PhotonNetwork.Instantiate(this._ammoBox.name, new Vector3(transform.position.x, transform.position.y + 1, -10.39756f), Quaternion.identity, 0);
        ammoBoxCount += 1;
    }
}
