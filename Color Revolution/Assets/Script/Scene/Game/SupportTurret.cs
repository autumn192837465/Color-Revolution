using System;
using System.Linq;
using CB.Model;
using CR.Model;
using Kinopi.Enums;
using Kinopi.Extensions;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace CR.Game
{
   
    
    public abstract class SupportTurret : Turret
    {
        public MSupportTurret MSupportTurret => mSupportTurret;
        protected MSupportTurret mSupportTurret;
        protected virtual void Awake()
        {
            Initialize();
        }

        public SupportTurretData SupportTurretData { get; protected set; }
        

    }
}
