using Xunit;
using System;
using System.Collections.Generic;
using Shelter.Objects;

namespace  Shelter
{
  public class AnimalTest : IDisposable
  {
    public AnimalTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=shelter_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange
      //Act
      int result = Animal.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Animal firstAnimal = new Animal("Fido", "Male", DateTime.Today, "Dog");
      Animal secondAnimal = new Animal("Fido", "Male", DateTime.Today, "Dog");

      Assert.Equal(firstAnimal, secondAnimal);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Animal testAnimal = new Animal("Fido", "Male", DateTime.Today, "Dog");

      testAnimal.Save();
      List<Animal> result = Animal.GetAll();
      List<Animal> testList = new List<Animal>{testAnimal};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Animal testAnimal = new Animal("Fido", "Male", DateTime.Today, "Dog");

      testAnimal.Save();
      Animal savedAnimal = Animal.GetAll()[0];

      int result = savedAnimal.GetId();
      int testId = testAnimal.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsAnimalInDatabase()
    {
      Animal testAnimal = new Animal("Fido", "Male", DateTime.Today, "Dog");
      testAnimal.Save();
      Animal foundAnimal = Animal.Find(testAnimal.GetId());

      Assert.Equal(testAnimal, foundAnimal);
    }

    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
