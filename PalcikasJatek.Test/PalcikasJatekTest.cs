using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palcikas_Jatek.Model;

namespace PalcikasJatek.Test
{
    [TestClass]
    public class PalcikasJatekTest
    {

        [TestMethod]
        public void AiChooseSquareWithOneOrZeroSelectedSide()
        {
            var gm = new GameModel(2,false);
            gm.NewGame();

            for (int i = 0; i < gm.Squares.Count; i++)
            {
                var coords = gm.Squares[i].GetFreeSideCoords(Side.LEFT);
                gm.HighLightSide(coords.X, coords.Y);
                gm.SelectSide();
            }

            gm.PlayersTurn = false;
            var comp = gm.ComputerTurn();

            Assert.IsTrue(comp.Item1.SelectedNum < 2 );

        }

        [TestMethod]
        public void AiChooseSquareWithThreeSelectedSide()
        {
            var gm = new GameModel(2, false);
            gm.NewGame();

            for (int i = 0; i < gm.Squares.Count; i++)
            {
                var coords = gm.Squares[i].GetFreeSideCoords();
                gm.HighLightSide(coords.X, coords.Y);
                gm.SelectSide();

                coords = gm.Squares[i].GetFreeSideCoords();
                gm.HighLightSide(coords.X, coords.Y);
                gm.SelectSide();
            }

            gm.PlayersTurn = false;
            var comp= gm.ComputerTurn();
             Assert.IsTrue(comp.Item1.SelectedNum == 3);
           
        }

        [TestMethod]
        public void AiScoreTwoPoints()
        {
            var gm = new GameModel(2,false);
            gm.NewGame();

            for (int i = 0; i < gm.Squares.Count; i++)
            {
                gm.PlayersTurn = true;
                var coords = gm.Squares[i].GetFreeSideCoords(Side.LEFT);
                gm.HighLightSide(coords.X, coords.Y);
                gm.SelectSide();

                gm.PlayersTurn = true;
                coords = gm.Squares[i].GetFreeSideCoords(Side.TOP);
                gm.HighLightSide(coords.X, coords.Y);
                gm.SelectSide();
            }

            gm.PlayersTurn = false;
            gm.ComputerTurn();
            gm.SelectSide();

            gm.ComputerTurn();
            gm.SelectSide();

            Assert.IsTrue(gm.BlueScore == 2 && gm.RedScore == 1);

        }


    }
}
