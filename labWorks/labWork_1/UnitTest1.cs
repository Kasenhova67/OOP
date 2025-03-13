using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop;

namespace oop.testik
{
    [TestClass]
    public class CircleTests
    {
      

        [TestMethod]
        public void Fill_ShouldFillCircleOnCanvas()
        {
            var canvas = new Canvas();
            var circle = new Circle(10, 10, 5);

            circle.Fill(canvas);

            Assert.AreEqual(Canvas.FillChar, canvas.CanvasData[10][10]);
        }

        [TestMethod]
        public void Move_ShouldMoveCircleToNewPosition()
        {
            
            var canvas = new Canvas();
            var circle = new Circle(10, 10, 5);

            circle.Move(5, 5);

            Assert.AreEqual(15, circle.X);
            Assert.AreEqual(15, circle.Y);
        }

        [TestMethod]
        public void Erase_ShouldEraseCircleFromCanvas()
        {
            var canvas = new Canvas();
            var circle = new Circle(10, 10, 5);
            circle.Draw(canvas);

            circle.Erase(canvas);

            Assert.AreEqual(' ', canvas.CanvasData[10][10]);
        }
    }
}