using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Run, Wait, Busy, Boss }
public class GameController : MonoBehaviour
{
    [SerializeField] GameObject result;
    [SerializeField] First First;
    [SerializeField] Button Restart;
    [SerializeField] Button Exit;
    [SerializeField] List<GameObject> dices;
    
    [SerializeField] DiceSpawner diceSpawner;
    [SerializeField] DiceUpgrade[] upgrades;
    List<GameObject> myDice = new List<GameObject>();

    GameState state;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public static GameController i { get; set; }
    public DiceSpawner DiceSpawner { get => diceSpawner; }
    public List<GameObject> Dices { get => dices; }
    public List<GameObject> MyDice { get => myDice; set => myDice = value; }

    private void Awake()
    {
        i = this;
        Restart.onClick.AddListener(RestartGame);
        Exit.onClick.AddListener(ExitGame);
    }
    private void Start()
    {
        GameManager.i.Init();
        UpgradeInfo.i.Init();
        GameInit();
    }
    public event Action OnHeartChanged;
    public void DiscountHeart()
    {
        GameManager.i.Heart--;
        GameManager.i.Heart = Mathf.Clamp(GameManager.i.Heart, 0, 3);
        OnHeartChanged?.Invoke();
        if(GameManager.i.Heart == 0)
        {
            GameOver();
        }
    }
    void GameInit()
    {
        MyDice = new List<GameObject>();
        Time.timeScale = 0f;
        First.gameObject.SetActive(true);
        var rnd = new System.Random();
        dices = dices.OrderBy(x => rnd.Next()).ToList();
        First.Init();
    }
    public void GameStart()
    {
        First.gameObject.SetActive(false);
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].SetDice(MyDice[i].GetComponent<Dice>());
        }
        myDice = myDice.ConvertAll(x => x);
        OnGameStart?.Invoke();
        Time.timeScale = 1f;
    }
    public void GameOver()
    {
        OnGameOver?.Invoke();
        Time.timeScale = 0f;
        result.SetActive(true);
    }
    void RestartGame()
    {
        UpgradeInfo.i.Init();
        GameManager.i.Init();
        EnemyList.i.Init();
        AIEnemyList.i.Init();
        AIController.i.Init();
        OnHeartChanged?.Invoke();
        result.SetActive(false);
        First.gameObject.SetActive(true);
        GameInit();
    }
    void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exit Cilcked");
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
        Application.Quit();
#else
        Application.Quit();
#endif
    }
}
