using System;

namespace ShootEmUp
{
    public sealed class Player : BaseUnit
    {
        public Action<Player, int> OnHealthChanged;
        public Action<Player> OnHealthEmpty;
    }
}