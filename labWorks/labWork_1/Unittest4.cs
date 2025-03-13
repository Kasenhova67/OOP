using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop;


namespace oop.testik
{
    [TestClass]
    public class TriangleTests
    {
        [TestMethod]
        public void Draw_ShouldDrawTriangleOnCanvas()
        {
            var canvas = new Canvas();
            var triangle = new Triangle(10, 10, 5, 5, 5);

            triangle.Draw(canvas);

            Assert.AreEqual(Canvas.FillChar, canvas.CanvasData[10][10]); 
        }

       
        [TestMethod]
        public void Move_ShouldMoveTriangleToNewPosition()
        {
            var triangle = new Triangle(10, 10, 5, 5, 5);

            triangle.Move(5, 5);

            Assert.AreEqual(15, triangle.X);
            Assert.AreEqual(15, triangle.Y);
        }

        
    }
}
