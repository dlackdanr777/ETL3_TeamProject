using TMPro;
using UnityEngine;

public class UIBoss : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIBar _healthBar;
    [SerializeField] private TextMeshProUGUI _nameText;
    
    private BossController _boss;

    public void Init(BossController boss)
    {
        _boss = boss;
        _nameText.text = _boss.Name;
        _healthBar.SetBarAfterNoAnime(_boss.MaxHp, _boss.Hp);
        _boss.OnHpChanged += OnHpChanged;
    }

    private void OnDestroy()
    {
        _boss.OnHpChanged -= OnHpChanged;
    }


    private void OnHpChanged(object subject, float value)
    {
        _healthBar.SetBar(_boss.MaxHp, _boss.Hp);
    }


}
