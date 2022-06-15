using System.Linq;
using ConwayLib;
using ConwayWebModel;

namespace ConwayWebClient
{
    public static class BoardDetailExtensions
    {
        public static GameState ToGameState(this BoardDetail details)
        {
            return new GameState
            {
                Width = details.Info.Width,
                Height = details.Info.Height,
                SparseData = details.LiveCells.Select(x => new ConwayLib.CellCoord(x.X, x.Y)).ToArray(),
                Format = DensityOption.Sparse
            };
        }
    }
}