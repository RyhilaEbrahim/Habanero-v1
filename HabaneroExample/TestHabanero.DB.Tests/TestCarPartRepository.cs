﻿using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TestHabanero.BO;
using TestHabanero.BO.Tests.Util;
using TestHabanero.Tests.Commons;
using TestHabenaro.Db.Interfaces;

namespace TestHabanero.DB.Tests
{
    public class TestCarPartRepository
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            TestUtils.SetupFixture();
        }

        [Test]
        public void GetParts_GivenOnePart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var car = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            var cars = new List<CarPart> { car };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.GetCarPart().Returns(cars);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cars.Count);
            var actual = cars.First();
            Assert.AreSame(car, actual);
        }

        [Test]
        public void GetParts_GivenTwoPart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var car1 = new CarPartBuilder().WithNewId().Build();
            var car2 = new CarPartBuilder().WithNewId().Build();

            var userRepository = Substitute.For<ICarPartRepository>();
            var cars = new List<CarPart> { car1, car2 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.GetCarPart().Returns(cars);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cars.Count);

        }

        [Test]
        public void GetParts_GivenThreePart_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var part1 = new CarPartBuilder().WithNewId().Build();
            var part2 = new CarPartBuilder().WithNewId().Build();
            var part3 = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            var cars = new List<CarPart> { part1, part2, part3 };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.GetCarPart().Returns(cars);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cars.Count);

        }
        
        [Test]
        public void GetPartBy_GivenPartId_ShouldReturnPart()
        {
            //---------------Set up test pack-------------------
            var part = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            var parts = new List<CarPart> { part };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.GetCarPartBy(part.PartId).Returns(part);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, parts.Count);
            var actual = parts.First();
            Assert.AreSame(part, actual);
        }

        [Test]
        public void Save_GivenNewPart_ShouldSave()
        {
            //---------------Set up test pack-------------------
            var car = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Save(car);
            //---------------Test Result -----------------------
            userRepository.Received().Save(car);
        }

        [Test]
        public void Update_GivenExistingPart_ShouldUpdateAndSave()
        {
            //---------------Set up test pack-------------------
            var existingPart = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            userRepository.GetCarPartBy(existingPart.PartId).Returns(existingPart);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Update(existingPart, existingPart);
            //---------------Test Result -----------------------
            userRepository.Received().Update(existingPart, existingPart);
        }

        [Test]
        public void Delete_GivenExistingPart_ShouldDeleteAndSave()
        {
            //---------------Set up test pack-------------------
            var part = new CarPartBuilder().WithNewId().Build();
            var userRepository = Substitute.For<ICarPartRepository>();
            userRepository.GetCarPartBy(part.PartId).Returns(part);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            userRepository.Delete(part);
            //---------------Test Result -----------------------
            userRepository.Received().Delete(part);
        }
    }
}