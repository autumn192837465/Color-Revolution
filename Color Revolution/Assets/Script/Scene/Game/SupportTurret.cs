using CB.Model;

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
