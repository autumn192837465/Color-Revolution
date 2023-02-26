using CB.Model;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CR.Game
{
    public class TurretInfoUI : MonoBehaviour
    {
        [Header("Offensive Turret")] 
        [SerializeField] private GameObject offensiveTurretInfoRoot;
        [SerializeField] private TextMeshProUGUI redDamageText;
        [SerializeField] private TextMeshProUGUI greenDamageText;
        [SerializeField] private TextMeshProUGUI blueDamageText;
        [SerializeField] private TextMeshProUGUI attackRangeDamageText;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI operatingTimeText;
        [SerializeField] private TextMeshProUGUI coolDownTimeText;


        [Header("Support Turret")] 
        [SerializeField] private GameObject supportTurretInfoRoot;

        

        public void InitializeUI(MOffensiveTurret mOffensiveTurret)
        {
            offensiveTurretInfoRoot.SetActive(true);
            supportTurretInfoRoot.SetActive(false);
            
            redDamageText.text = mOffensiveTurret.AttackDamage.RedValue.ToString();
            greenDamageText.text = mOffensiveTurret.AttackDamage.GreenValue.ToString();
            blueDamageText.text = mOffensiveTurret.AttackDamage.BlueValue.ToString();

            attackRangeDamageText.text = mOffensiveTurret.AttackRange.ToString("0.0");
            attackSpeedText.text = mOffensiveTurret.BulletPerSecond.ToString("0.0");
            operatingTimeText.text = $"{mOffensiveTurret.OperatingTime} sec";
            coolDownTimeText.text = $"{mOffensiveTurret.CooldownTime} sec";
        }
        
        
        public void InitializeUI(MSupportTurret mSupportTurret)
        {
            offensiveTurretInfoRoot.SetActive(false);
            supportTurretInfoRoot.SetActive(true);
        }
    }    
}
