using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kong_Engine;

namespace KongEngineTests
{
    [TestClass]
    public class MainGameTests
    {
        [TestMethod]
        public void InitializeGameplay_ShouldInitializeEntities()
        {
            var game = new MainGame();
            game.InitializeGameplay();

            Assert.IsNotNull(game._playerEntity);
            Assert.IsNotNull(game._enemyEntity);
            Assert.IsTrue(game._entities.Contains(game._playerEntity));
            Assert.IsTrue(game._entities.Contains(game._enemyEntity));
        }
    }
}
