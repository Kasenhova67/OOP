using Microsoft.VisualStudio.TestTools.UnitTesting;
using oop;


namespace oop.testik
{
    [TestClass]
    public class AddShapeActionTests
    {
        [TestMethod]
        public void Execute_ShouldAddShapeToCanvas()
        {
            var canvas = new Canvas();
            var circle = new Circle(10, 10, 5);
            var action = new AddShapeAction(canvas, circle);

            action.Execute();

            Assert.AreEqual(1, canvas.Shapes.Count);
        }

        [TestMethod]
        public void Undo_ShouldRemoveShapeFromCanvas()
        {
            var canvas = new Canvas();
            var circle = new Circle(10, 10, 5);
            var action = new AddShapeAction(canvas, circle);
            action.Execute();

            action.Undo();

            Assert.AreEqual(0, canvas.Shapes.Count); 
        }
    }
}