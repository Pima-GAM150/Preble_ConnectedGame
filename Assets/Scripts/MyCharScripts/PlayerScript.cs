using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Fields

    private int playerNumber;
    private int ammoCount;

    private string moveInputAxis = "Horizontal";
    private string jumpInputAxis = "Jump";

    private bool IsFiring;

    private Vector2 shotVector;
    private Vector2 bulletPoint;

    #endregion

    #region Public Fields

    public bool isJumping = false;
    public float playerSpeed = 15f;
    public float jumpStrength = 15f;
    public float Health = 1f;
    public static float myBulletDirection;

    public GameObject playerPrefab;
    public Rigidbody2D playerRB;
    public GameObject _bullet;
    public Transform playerBullet;
    public Transform myBullet;

    public static GameObject LocalPlayerInstance;
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isJumping);
            stream.SendNext(playerPrefab.transform.position);
        }
        else
        {
            this.isJumping = (bool)stream.ReceiveNext();
            this.playerPrefab.transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        ammoCount = 0;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float moveAxis = Input.GetAxis(moveInputAxis);
            float jumpAxis = Input.GetAxis(jumpInputAxis);
            if (moveAxis > 0)
            {
                myBulletDirection = 1;
            }
            if (moveAxis < 0)
            {
                myBulletDirection = -1;
            }
            bulletPoint = new Vector2(transform.position.x + 2 * myBulletDirection, transform.position.y);

            ApplyInput(moveAxis, jumpAxis);
        }

        if (Health <= 0f)
        {
            GameManager.Instance.LeaveRoom();
        }

        if (_bullet != null && IsFiring != _bullet.activeSelf)
        {
            _bullet.SetActive(IsFiring);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ammo" && ammoCount < 3)
        {
            ammoCount += 1;
            PhotonNetwork.Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!other.name.Contains("Bullet"))
        {
            return;
        }
        Health -= 0.1f;
    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!other.name.Contains("Bullet"))
        {
            return;
        }
        Health -= 0.1f * Time.deltaTime;
    }

    #region MovementInput

    void ApplyInput(float moveInput, float jumpInput)
    {
        Move(moveInput);
        if (playerRB.velocity.y == 0)
        {
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            isJumping = true;
            Jump(jumpInput);
        }
        if (Input.GetKeyDown(KeyCode.E) && ammoCount > 0)
        {
            Shoot();
        }
    }

    private void Move(float input)
    {
        transform.Translate(Vector2.right * input * playerSpeed);
        //Debug.Log("Moving the character either left or right.");
    }

    private void Jump(float input)
    {
        playerRB.AddForce(Vector2.up * input * jumpStrength);
        Debug.Log("Jumping!");
    }

    private void Shoot()
    {
        PhotonNetwork.Instantiate(_bullet.name, bulletPoint, Quaternion.identity, 0);
        ammoCount -= 1;
    }

    #endregion
}
