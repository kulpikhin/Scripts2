using UnityEngine;

public class AbilitiesIcon : MonoBehaviour
{
    [SerializeField] private IconAbility[] _AbilitiesSlots;

    [SerializeField] private Sprite _emptySlot;

    private int _countSpells = 0;

    public void SetIcon(int index, Sprite sprite)
    {
        _AbilitiesSlots[index].SpellIcon.sprite = sprite;
        _AbilitiesSlots[index].EmptyIcon.sprite = sprite;

        _countSpells++;
    }

    public void FillEmptySlots()
    {
        for (int i = _AbilitiesSlots.Length - 1; i >= _countSpells; i--)
        {
            _AbilitiesSlots[i].SpellIcon.sprite = _emptySlot;
        }
    }

    public IconAbility GetAbility(int index)
    { 
        return _AbilitiesSlots[index];
    }
}
