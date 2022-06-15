using ConwayLib;
using System;

namespace ConwayMultiSim
{
    partial class Program
    {
        public class Simulation
        {
            public bool Finished {get; private set; } = false;
            public int Generation {get {return mGame.Generation;}}
            private readonly Game mGame;

            public Simulation(int width, int height, double density, IEvolution evolution, int? seed = null)
            {
                IReadableBoard initialBoard;
                if (!seed.HasValue)
                {
                    initialBoard = new Board(width, height).Randomise(density);
                }
                else
                {
                    initialBoard = new Board(width, height).Randomise(new Random(seed.Value), density);
                }
                mGame = new Game(initialBoard, evolution);
            }
            public Simulation(GameState state, IEvolution evolution)
            {
                mGame = new Game(new Board(state), evolution);
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
    }
}
