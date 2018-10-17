using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2DTest.Physics
{
    [TestClass]
    public class PhysicsEngineTest
    {
        [TestMethod]
        public void Test1()
        {
            var obj1 = new MovableObjectDummy
            {
                Position = new IntVector(0, -5000, 0),
                Velocity = new IntVector(0, 1000, 0)
            };
            var obj2 = new MovableObjectDummy
            {
                Position = new IntVector(0, -4000, 0),
                Velocity = new IntVector(0, 1000, 0)
            };
            var objs = new List<IMovableObject> { obj1, obj2 };
            
            var world = new Mock<IPhysicsEngineContext>();
            world.Setup(w => w.GetCollidableObjectsWithin(It.IsAny<IntRectangle>())).Returns(objs);
            world.Setup(w => w.GetMovableObjects()).Returns(objs);
            world.Setup(w => w.Gravity).Returns(IntVector.Down * 1000);

            var physics = new PhysicsEngine(CoordinateSystem.Create());

            Assert.AreEqual(0, obj1.TouchedObjects.Count);
            Assert.AreEqual(0, obj2.TouchedObjects.Count);

            physics.Initialize(world.Object);

            Assert.AreEqual(1, obj1.TouchedObjects.Count);
            Assert.AreEqual(1, obj2.TouchedObjects.Count);
            Assert.AreEqual(1, obj1.TouchedObjects[Side.Bottom].Count);
            Assert.AreEqual(1, obj2.TouchedObjects[Side.Top].Count);

            physics.Update(world.Object, TimeSpan.FromSeconds(1));

            Assert.AreEqual(1, obj1.TouchedObjects.Count);
            Assert.AreEqual(1, obj2.TouchedObjects.Count);
            Assert.AreEqual(1, obj1.TouchedObjects[Side.Bottom].Count);
            Assert.AreEqual(1, obj2.TouchedObjects[Side.Top].Count);
        }

        [TestMethod]
        public void Test2()
        {
            var obj1 = new MovableObjectDummy
            {
                Position = new IntVector(0, -5000, 0),
                Velocity = new IntVector(0, -1000, 0)
            };
            var obj2 = new MovableObjectDummy
            {
                Position = new IntVector(0, -4000, 0),
                Velocity = new IntVector(0, -2000, 0)
            };
            var objs = new List<IMovableObject> { obj1, obj2 };

            var world = new Mock<IPhysicsEngineContext>();
            world.Setup(w => w.GetCollidableObjectsWithin(It.IsAny<IntRectangle>())).Returns(objs);
            world.Setup(w => w.GetMovableObjects()).Returns(objs);
            world.Setup(w => w.Gravity).Returns(IntVector.Down * 1000);

            var physics = new PhysicsEngine(CoordinateSystem.Create());

            Assert.AreEqual(0, obj1.TouchedObjects.Count);
            Assert.AreEqual(0, obj2.TouchedObjects.Count);

            physics.Initialize(world.Object);

            Assert.AreEqual(1, obj1.TouchedObjects.Count);
            Assert.AreEqual(1, obj2.TouchedObjects.Count);
            Assert.AreEqual(1, obj1.TouchedObjects[Side.Bottom].Count);
            Assert.AreEqual(1, obj2.TouchedObjects[Side.Top].Count);

            physics.Update(world.Object, TimeSpan.FromSeconds(1));

            Assert.AreEqual(new IntVector(0, -6000, 0), obj1.Position);
            Assert.AreEqual(new IntVector(0, -5000, 0), obj2.Position);
            Assert.AreEqual(1, obj1.TouchedObjects.Count);
            Assert.AreEqual(1, obj2.TouchedObjects.Count);
            Assert.AreEqual(1, obj1.TouchedObjects[Side.Bottom].Count);
            Assert.AreEqual(1, obj2.TouchedObjects[Side.Top].Count);
        }
    }

    public class MovableObjectDummy : IMovableObject
    {
        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);
        public bool IsSolid => true;
        public IntVector Velocity { get; set; }
        public IntVector Position { get; set; }
        public IntVector Size => new IntVector(1000, 1000, 1000);
        public TouchedObjectCollection TouchedObjects { get; } = new TouchedObjectCollection();
    }
}
