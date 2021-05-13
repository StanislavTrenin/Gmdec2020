using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Buff
    {
        NO,
        FAIL,
        WIN
    }
    
    [SerializeField] private Controller controller;
    [SerializeField] private UiManagerGame uiManagerGame;

    private static List<LevelInfo> levels = new List<LevelInfo>();
    private static int currentLevel = 0;
    public static Buff currentBuff = Buff.NO;

    public static LevelInfo GetCurrentLevelInfo()
    {
        return levels[currentLevel];
    }

    static GameManager()
    {
        LevelInfo levelInfo = new LevelInfo();
        levelInfo.mapName = "map1.txt";
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 3,
            y = 8,
            characterClass = CharacterClass.TANK,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 3,
            y = 10,
            characterClass = CharacterClass.MELEE_FIGHTER,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 2,
            y = 12,
            characterClass = CharacterClass.RANGED_FIGHTER,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 1,
            y = 9,
            characterClass = CharacterClass.SUPPORT,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 8,
            y = 8,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 8,
            y = 12,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 16,
            y = 5,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 14,
            y = 14,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 1
        });
        levels.Add(levelInfo);
        levelInfo = new LevelInfo();
        levelInfo.mapName = "map2.txt";
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 3,
            y = 8,
            characterClass = CharacterClass.TANK,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 3,
            y = 10,
            characterClass = CharacterClass.MELEE_FIGHTER,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 1,
            y = 7,
            characterClass = CharacterClass.RANGED_FIGHTER,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = true,
            x = 1,
            y = 9,
            characterClass = CharacterClass.SUPPORT,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 8,
            y = 8,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 8,
            y = 12,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 3
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 16,
            y = 5,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 1
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 14,
            y = 14,
            characterClass = CharacterClass.OOZE_MELEE,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 14,
            y = 10,
            characterClass = CharacterClass.OOZE_RANGED,
            level = 2
        });
        levelInfo.spawnPoints.Add(new SpawnPoint
        {
            isPlayer = false,
            x = 11,
            y = 12,
            characterClass = CharacterClass.OOZE_RANGED,
            level = 2
        });
        levels.Add(levelInfo);
    }

    private void Start()
    {
        controller.onLose += RestartRound;
        controller.onWin += NextRound;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        controller.onLose -= RestartRound;
        controller.onWin -= NextRound;
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void RestartRound()
    {
        uiManagerGame.ShowPanel(UiPanelNames.RestartPanel);
    }

    private void NextRound()
    {
        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            currentLevel = 0;
            uiManagerGame.ShowPanel(UiPanelNames.EndGamePanel);
        }
        else
        {
            uiManagerGame.ShowPanel(UiPanelNames.EndRoundPanel);
        }
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name != "Game")
        {
            currentLevel = 0;
            currentBuff = Buff.NO;
        }
    }
}
