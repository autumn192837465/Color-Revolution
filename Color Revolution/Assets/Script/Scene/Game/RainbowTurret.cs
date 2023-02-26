using System.Collections;
using System.Collections.Generic;
using CR.Game;
using Kinopi.Enums;
using Kinopi.Extensions;
using UnityEngine;

namespace CR.Game
{
    public class RainbowTurret : SupportTurret
    {
        public override TurretType TurretType => TurretType.Support;
        public override void Initialize()
        {
            SupportTurretData = DataManager.Instance.GetSupportTurretData(SupportTurretType.RainbowTurret);
            mSupportTurret = SupportTurretData.MSupportTurret.DeepClone();
            turretCost = SupportTurretData.Cost;
        }
    }    
}
