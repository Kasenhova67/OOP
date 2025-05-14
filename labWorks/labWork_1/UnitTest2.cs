using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop;


namespace oop.testik
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void Draw_ShouldDrawRectangleOnCanvas()
        {
            var canvas = new Canvas();
            var rectangle = new Rectangle(10, 10, 5, 5);

            rectangle.Draw(canvas);

            Assert.AreEqual(Canvas.FillChar, canvas.CanvasData[10][10]);
        }

        [TestMethod]
        public void Fill_ShouldFillRectangleOnCanvas()
        {
            var canvas = new Canvas();
            var rectangle = new Rectangle(10, 10, 5, 5);

            rectangle.Fill(canvas);

            Assert.AreEqual(Canvas.FillChar, canvas.CanvasData[12][12]);
        }

        [TestMethod]
        public void Move_ShouldMoveRectangleToNewPosition()
        {
            var rectangle = new Rectangle(10, 10, 5, 5);

            rectangle.Move(5, 5);

            Assert.AreEqual(15, rectangle.X);
            Assert.AreEqual(15, rectangle.Y);
        }

        [TestMethod]
        public void Erase_ShouldEraseRectangleFromCanvas()
        {
            var canvas = new Canvas();
            var rectangle = new Rectangle(10, 10, 5, 5);
            rectangle.Draw(canvas);

            rectangle.Erase(canvas);

            Assert.AreEqual(' ', canvas.CanvasData[10][10]);
        }
    }
}