namespace Game.Platform
{
    public class PlatformDeath : _PlatformBase
    {
        public override void OnEnter(PlayerController player)
        {
            base.OnEnter(player);
            player.Destroy();
        }
    }
}