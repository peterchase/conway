using ConwayLib;
using System;

namespace ConwayMultiSim;

public class Simulation
{
    private static readonly Random mRandom = new();
    public int Seed {get; }
    public bool Finished { get; private set; } = false;
    public int Generation { get { return mGame.Generation; } }
    private readonly Game mGame;

    public Simulation(int width, int height, double density, IEvolution evolution, int? seed = null)
    {
        IReadableBoard initialBoard;
        if (!seed.HasValue)
        {
            Seed = mRandom.Next();
        }
        else
        {
            Seed = seed.Value;
        }
        initialBoard = new Board(width, height).Randomise(new Random(Seed), density);
        mGame = new Game(initialBoard, evolution);
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
}
