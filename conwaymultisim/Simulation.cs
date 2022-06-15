using ConwayLib;
using System;

namespace ConwayMultiSim
{
    partial class Program
    {
        public class Simulation
        {
            public bool Finished {get; private set; } = false;
            private Game mGame;
            public int Evolution {get {return mGame.Generation;}}

            public Simulation(int seed, int width, int height, double density, IEvolution evolution)
            {
                var initialBoard = new Board(width, height).Randomise(new Random(seed), density);
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
