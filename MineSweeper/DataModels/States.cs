namespace MineSweeper
{
    /// <summary>
    /// 
    /// </summary>
    public enum States
    {
        Blank = 1 << 0,
        One = 1 << 1,
        Two = 1 << 2,
        Three = 1 << 3,
        Four = 1 << 4,
        Five = 1 << 5,
        Six = 1 << 6,
        Seven = 1 << 7,
        Eight = 1 << 8,
        Unopened = 1 << 9,
        FlagMark = 1 << 10,
        WrongFlag = 1 << 11,
        QuestionMark = 1 << 12,
        Mine = 1 << 13,
    }
}
