using ConwayLib;
using System;

namespace ConwayMultiSim;

public class Simulation : IDisposable
{
    private static readonly Random mRandom = new();
    public int Seed {get; }
    public bool Finished { get; private set; } = false;
    public int Generation { get { return mGame.Generation; } }
    private readonly Game mGame;
    public IReadableBoard InitialBoard {get;}

    public Simulation(int width, int height, double density, IEvolution evolution, int? seed = null)
    {
        if (!seed.HasValue)
        {
            Seed = mRandom.Next();
        }
        else
        {
            Seed = seed.Value;
        }
        InitialBoard = new Board(width, height).Randomise(new Random(Seed), density);
        mGame = new Game(InitialBoard.MutableCopy(), evolution);
    }
    public bool Turn()
    {
        if (!Finished)
        {
            mGame.Turn(out bool isFinished);
            Finished = isFinished;
            return isFinished;
        }
        else
            return true;
    }

    public void Dispose()
    {
        mGame.Dispose();
    }
}
