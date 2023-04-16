namespace Tanks
{
    public enum ETeam
    {
        None = 0,
        A = -1,
        B = +1
    }

    public static class ETeamExtensions
    {
        public static ETeam Flip(this ETeam self)
        {
            return (ETeam)((int)self * -1);
        }
    }
}
