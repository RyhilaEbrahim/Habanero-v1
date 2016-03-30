using Habanero.BO;

namespace TestHabanero.BO.Tests.Util
{
    class TestUtils
    {
        public static void SetupFixture()
        {

            Habanero.BO.BORegistry.DataAccessor = new DataAccessorInMemory();
            BOBroker.LoadClassDefs();
        }
    }
}
