using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSave : Singleton<DataSave>
{

    public List<Player> players = new List<Player>();
    public List<Enemy> enemies = new List<Enemy>();
    public List<Character> characters = new List<Character>();
    public Character character;
    public Dictionary<string, Player> currentPlayers = new Dictionary<string, Player>();
    public Player currentPlayer;
    public GameObject currentObj, targetObj;
    /*public void PlayerDataIn(List<Player> players)
    {
        this.players = players; 
    }

    public void EnemyDataIn(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }*/
    public void SaveData()
    {    
        characters.Clear();
        for (int i = 0; i < players.Count; i++)
        {          
            for (int f = 0; f < characters.Count; f++)
            {
                if (players[i].speed > characters[f].speed)
                {
                    if (f == characters.Count - 1)
                    {
                        characters.Add(players[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    characters.Insert(f, players[i]);
                    break;
                }
            }
            if (characters.Count == 0)
            {
                characters.Add(players[i]);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            
            for (int f = 0; f < characters.Count; f++)
            {
                if (enemies[i].speed > characters[f].speed)
                {
                    if (f == characters.Count - 1)
                    {
                        characters.Add(enemies[i]);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    characters.Insert(f, enemies[i]);
                    break;
                }
            }
            if (characters.Count == 0)
            {
                characters.Add(enemies[i]);
            }
        }
        characters.Reverse();
    }
}
