using System;
using Habanero.BO;
using Habanero.Testability;
using TestHabanero.BO;

namespace TestHabanero.Tests.Commons
{
    public class CarPartBuilder
    {
        private readonly CarPart _carPart;
        public CarPartBuilder()
        {
            _carPart = BuildValid();
            _carPart.Quantity = RandomValueGen.GetRandomInt();
            _carPart.CarId = RandomValueGen.GetRandomGuid();
            _carPart.PartId = RandomValueGen.GetRandomGuid();
        }

        private BOTestFactory<T> CreateFactory<T>() where T : BusinessObject
        {
            BOBroker.LoadClassDefs();
            return new BOTestFactory<T>();
        }


        private CarPart BuildValid()
        {
            var car = CreateFactory<CarPart>()
                .SetValueFor(c => c.Quantity)
                .CreateValidBusinessObject();
            return car;
        }

        public CarPartBuilder WithNewId()
        {
            _carPart.CarId = Guid.NewGuid();
            return this;
        }

        public CarPart BuildSaved()
        {
            _carPart.Save();
            return _carPart;
        }

        public CarPart Build()
        {

            return _carPart;
        }
    }
}