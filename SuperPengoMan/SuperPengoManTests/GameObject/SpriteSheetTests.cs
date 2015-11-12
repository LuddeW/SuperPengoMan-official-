using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPengoMan.GameObject;
using SuperPengoMan;

namespace Util.Tests
{
    [TestClass()]
    public class SpriteSheetTests : Game
    {
        [TestMethod()]
        public void SpriteSheetTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void SrcRectTest()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            graphics.CreateDevice();
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, 500, 500);
            SpriteSheet spriteSheet = new SpriteSheet(5, 5, texture);
            Rectangle rect = spriteSheet.SrcRect(0);
            Rectangle testRect = new Rectangle(0, 0, 100, 100);
            Assert.AreEqual(true, testRect.Equals(rect));
            rect = spriteSheet.SrcRect(4);
            testRect = new Rectangle(400, 0, 500, 100);
            Assert.AreEqual(true, testRect.Equals(rect));
            rect = spriteSheet.SrcRect(12);
            testRect = new Rectangle(200, 200, 300, 300);
            Assert.AreEqual(true, testRect.Equals(rect));
            rect = spriteSheet.SrcRect(20);
            testRect = new Rectangle(0, 400, 100, 500);
            Assert.AreEqual(true, testRect.Equals(rect));
            rect = spriteSheet.SrcRect(24);
            testRect = new Rectangle(400, 400, 500, 500);
            Assert.AreEqual(true, testRect.Equals(rect));
        }

        [TestMethod()]
        public void RowsTest()
        {
            //Assert.Fail();
        }
    }
}

