using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private List<Character> RightTeam;
    [SerializeField] private List<Character> LeftTeam;

    private List<IDamageable> AliveRightTeam;
    private List<IDamageable> AliveLeftTeam;

    private int _countLeftTeam;
    private int _countRightTeam;

    private void OnEnable()
    {
        if (!TestMode.IsTest)
        {
            AliveRightTeam = new List<IDamageable>(RightTeam);
            AliveLeftTeam = new List<IDamageable>(LeftTeam);
            SubTeam(RightTeam.Cast<IDamageable>().ToList());
            SubTeam(LeftTeam.Cast<IDamageable>().ToList());
            BattleCondition.GameContinues = true;
        }
    }

    private void OnDisable()
    {
        UnSubTeam(RightTeam.Cast<IDamageable>().ToList());
        UnSubTeam(LeftTeam.Cast<IDamageable>().ToList());
    }

    private void Start()
    {
        BattleCondition.GameContinues = true;
    }

    public List<IDamageable> GetRandomTargets(TeamSide side, int count)
    {
        List<IDamageable> targets = new List<IDamageable>();

        if (side == TeamSide.Right)
        {
            if (count >= AliveLeftTeam.Count)
            {
                return AliveLeftTeam;
            }

            targets = AliveLeftTeam.OrderBy(_ => Random.value).Take(count).ToList();
        }

        else
        {
            if (count >= AliveRightTeam.Count)
            {
                return AliveRightTeam;
            }
            targets = AliveRightTeam.OrderBy(_ => Random.value).Take(count).ToList();
        }

        return targets;
    }

    private void onPlayerDeth(IDamageable character)
    {
        character.Container.ClearAllEffects();

        if (character.GetSide() == TeamSide.Right)
        {
            _countRightTeam--;
            AliveRightTeam.Remove(character);
        }
        else if (character.GetSide() == TeamSide.Left)
        {
            _countLeftTeam--;
            AliveLeftTeam.Remove(character);
        }

        if (_countRightTeam == 0)
        {
            Final(TeamSide.Left);
        }
        else if (_countLeftTeam == 0)
        {
            Final(TeamSide.Right);
        }
    }

    private void SubTeam(List<IDamageable> characters)
    {
        foreach (Character character in characters)
        {
            character.Died += onPlayerDeth;

            if (character.Side == TeamSide.Left)
            {
                _countLeftTeam++;
            }
            else if (character.Side == TeamSide.Right)
            {
                _countRightTeam++;
            }
        }
    }

    private void UnSubTeam(List<IDamageable> characters)
    {
        foreach (Character character in characters)
        {
            character.Died -= onPlayerDeth;
        }
    }

    private void Final(TeamSide side)
    {
        Debug.Log(side.ToString() + " win");

        ClearAllEffects();
        BattleCondition.GameContinues = false;
    }

    public void ClearAllEffects()
    {
        IDamageable[] units = FindObjectsOfType<MonoBehaviour>().OfType<IDamageable>().ToArray();

        foreach (var unit in units)
        {
            unit.Container.ClearAllEffects();
        }
    }
}
