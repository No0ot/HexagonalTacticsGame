using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<PlayerScriptable> profiles;
    public List<Player> players { get; private set; } = new List<Player>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        players.Clear();
        foreach(PlayerScriptable script in profiles)
        {
            script.player.InitializePlayer();
            players.Add(script.player);
            
        }
    }

    private void Start()
    {
        BattleManager.Instance.InitializeBattle();
    }
}
