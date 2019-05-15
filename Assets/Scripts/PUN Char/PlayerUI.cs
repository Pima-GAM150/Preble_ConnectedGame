using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;


    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    private PlayerManager target;

    float characterControllerHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    Vector3 targetPosition;


    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Update()
    {
        if(playerHealthSlider != null)
        {
            playerHealthSlider.value = target.Health;
        }

        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void LateUpdate()
    {
        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (targetRenderer != null)
        {
            this.gameObject.SetActive(targetRenderer.isVisible);
        }


        // #Critical
        // Follow the Target GameObject on screen.
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        target = _target;

        targetTransform = this.target.GetComponent<Transform>();
        targetRenderer = this.target.GetComponent<Renderer>();
        CharacterController characterController = _target.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }

        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }
}