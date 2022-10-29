namespace Tanks
{
    public enum ETeam
    {
        A = -1,
        B = +1,
        None = 0
    }

    public static class ETeamExtensions
    {
        public static ETeam Flip(this ETeam self)
        {
            return (ETeam)((int)self * -1);
        }
    }
}
