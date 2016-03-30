using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using TestHabanero.Bootstrap.Mappings;
using TestHabanero.BO;
using TestHabanero.Controllers;
using TestHabanero.Models;
using TestHabanero.Tests.Commons;
using TestHabenaro.Db.Interfaces;
using Part = System.Web.UI.WebControls.WebParts.Part;

namespace HabeneroTests
{
    public class TestCarPartController
    {
        private CarPartControllerBuilder CreateBuilder()
        {
            return new CarPartControllerBuilder();
        }

        [Test]
        public void Construct()
        {
            Assert.DoesNotThrow(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
        }

        [Test]
        public void Construct_GivenICarPartRepositoryIsNull_ShouldThrow()
        {
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(null, Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("carPartRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), Substitute.For<IPartRepository>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Construct_GivenICarRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), null, Substitute.For<IPartRepository>(), Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("carRepository", ex.ParamName);
        }


        [Test]
        public void Construct_GivenIPartRepositoryIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex =
                Assert.Throws<ArgumentNullException>(() => new CarPartsController(Substitute.For<ICarPartRepository>(), Substitute.For<ICarRepository>(), null, Substitute.For<IMappingEngine>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("partRepository", ex.ParamName);
        }

     

        [Test]
        public void Create_POST_GivenModelStateIsValid_ShouldCallSave()
        {
            //---------------Set up test pack-------------------
            var viewModel = Substitute.For<CarPartsViewModel>();

            var repository = Substitute.For<ICarPartRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            var car = new CarPartBuilder().WithNewId().Build();
            mapperEngine.Map<CarPart>(viewModel).Returns(car);
            var carController = CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mapperEngine)
                .Build();
            repository.Save(car);
            //---------------Assert Precondition----------------
            Assert.IsTrue(carController.ModelState.IsValid);
            //---------------Execute Test ----------------------
            var result = carController.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            repository.Received().Save(car);
        }

        [Test]
        public void Create_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var partRepository = Substitute.For<IPartRepository>();
            var carRepository = Substitute.For<ICarRepository>();
            var mapperEngine = Substitute.For<IMappingEngine>();
            
            var part = new PartBuilder().Build();
            var parts = new List<TestHabanero.BO.Part> { part };
            var car = new CarBuilder().Build();
            var cars = new List<Car> { car };
            partRepository.GetParts().Returns(parts);
            carRepository.GetCars().Returns(cars);
            var partController = CreateBuilder()
                .WithMappingEngine(mapperEngine)
                .WithPartRepository(partRepository)
                .WithCarRepository(carRepository)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_GivenAllUsersReturnedFromRepository_ShouldReturnViewModel()
        {
            //---------------Set up test pack-------------------
            var car = new CarBuilder().WithNewId().Build();
            var part=new PartBuilder().WithNewId().Build();
            var carPart = new CarPartBuilder().WithNewId().WithCar(car).WithPart(part).Build();
            var carParts = new List<CarPart> { carPart };
            var repository = Substitute.For<ICarPartRepository>();
            var mappingEngine = ResolveMapper();

            repository.GetCarPart().Returns(carParts);

            var partController = CreateBuilder()
                .WithCarPartRepository(repository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var model = result.Model as List<CarPartsIndexViewModel>;
            Assert.IsInstanceOf<List<CarPartsIndexViewModel>>(model);
            Assert.AreEqual(carPart.CarPartId, model.FirstOrDefault().CarPartId);
            Assert.AreEqual(carPart.CarId, model.FirstOrDefault().CarId);
            Assert.AreEqual(carPart.PartId, model.FirstOrDefault().PartId);
            Assert.AreEqual(carPart.Quantity, model.FirstOrDefault().Quantity);
        }

        [Test]
        public void Index_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var part = new CarPartBuilder().WithNewId().Build();
            var parts = new List<CarPart> { part };
            var partRepository = Substitute.For<ICarPartRepository>();
            partRepository.GetCarPart().Returns(parts);
            var mappingEngine = Substitute.For<IMappingEngine>();

            var partController = CreateBuilder()
                .WithCarPartRepository(partRepository)
                .WithMappingEngine(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var results = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<CarPart>, List<CarPartsIndexViewModel>>(parts);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var partController = CreateBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = partController.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        private static IMappingEngine ResolveMapper()
        {
            return ResolveMappingWith(
                new CarPartMapping()
                );
        }

        public static IMappingEngine ResolveMappingWith(params Profile[] profiles)
        {
            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return Mapper.Engine;
        }
    }
}