using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameOver gameOver;
    public CinemachineTargetGroup targetGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;

        PlayerSpawn();
    }

    private void PlayerSpawn()
    {
        GameObject characterPrefab = SelectionController.instance.selectedPlayer.prefab;
        GameObject characterTmp = Instantiate(characterPrefab, SelectionController.instance.selectedPlayer.selectionLocation, Quaternion.identity);
        PlayerControl playerScript = FindObjectOfType<PlayerControl>();
        if (playerScript != null)
        {
            playerScript.enabled = true;
            playerScript.healthBar = FindObjectOfType<HealthBar>();

            GameObject playerUI = GameObject.Find("PlayerUI");
            playerScript.ammo = GameObject.FindWithTag("NumberAmo");

            CinemachineTargetGroup.Target newTarget = new CinemachineTargetGroup.Target
            {
                target = characterTmp.transform,
                weight = 1f,
                radius = 3f
            };

            List<CinemachineTargetGroup.Target> targets = new List<CinemachineTargetGroup.Target>(targetGroup.m_Targets);
            targets.Add(newTarget);
            targetGroup.m_Targets = targets.ToArray();
        }
    }

    private void Update()
    {
        if(FindObjectOfType<PlayerControl>() == null)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        gameOver.Setup();
        if (gameOverScreen.activeInHierarchy)
        {
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        

    }
}
