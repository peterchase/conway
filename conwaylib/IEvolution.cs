namespace ConwayLib
{
    public interface IEvolution
    {
        bool GetNextState(bool curState, int neighbours);
    }
}