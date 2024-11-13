using System;

namespace ShootEmUp
{
    public sealed class Player : BaseUnit
    {
        public event Action<Player> OnHealthEmpty;

        protected override void AfterDealDamage(int damage)
        {
            if (Health <= 0) OnHealthEmpty?.Invoke(this);
        }
    }
}