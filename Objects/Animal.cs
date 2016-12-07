using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Shelter
{
  public class Animal
  {
    private int _id;
    private string _name;
    private string _gender;
    private DateTime _admittance;
    private string _breed;

    public Animal(string Name, string Gender, DateTime Admittance, string Breed, int Id = 0)
    {
      _id = Id;
      _name = Name;
      _gender = Gender;
      _admittance = Admittance;
      _breed = Breed;
    }

    public override bool Equals(System.Object otherAnimal)
    {
      if (!(otherAnimal is Animal))
      {
        return false;
      }
      else
      {
        Animal newAnimal = (Animal) otherAnimal;
        bool idEquality =(this.GetId() == newAnimal.GetId());
        bool nameEquality = (this.GetName() == newAnimal.GetName());
        bool genderEquality = (this.GetGender() == newAnimal.GetGender());
        bool admittanceEquality = (this.GetAdmittance() == newAnimal.GetAdmittance());
        bool breedEquality = (this.GetBreed() == newAnimal.GetBreed());
        return (idEquality && nameEquality && genderEquality && admittanceEquality && breedEquality);
      }
    }

    public string GetName()
    {
      return _name;
    }
    public string GetGender()
    {
      return _gender;
    }
    public DateTime GetAdmittance()
    {
      return _admittance;
    }
    public string GetBreed()
    {
      return _breed;
    }
    public int GetId()
    {
      return _id;
    }

  public static List<Animal> GetAll()
  {
    List<Animal> allAnimals = new List<Animal>{};

    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM animals ORDER BY admittance;", conn);
    SqlDataReader rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int animalId = rdr.GetInt32(0);
      string animalName = rdr.GetString(1);
      string animalGender = rdr.GetString(2);
      DateTime animalAdmittance = rdr.GetDateTime(3);
      string animalBreed = rdr.GetString(4);
      Animal newAnimal = new Animal(animalName, animalGender, animalAdmittance, animalBreed, animalId);
      allAnimals.Add(newAnimal);
    }
    if(rdr != null)
    {
      rdr.Close();
    }
    if(conn != null)
    {
      conn.Close();
    }
    return allAnimals;
  }

  public void Save()
  {
    SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name,gender,admittance,breed) OUTPUT INSERTED.id VALUES (@AnimalName, @AnimalGender, @animalAdmittance, @AnimalBreed);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@AnimalName";
      nameParameter.Value = this.GetName();

      SqlParameter genderParameter = new SqlParameter();
      genderParameter.ParameterName = "@AnimalGender";
      genderParameter.Value = this.GetGender();

      SqlParameter admittanceParameter = new SqlParameter();
      admittanceParameter.ParameterName = "@AnimalAdmittance";
      admittanceParameter.Value = this.GetAdmittance();

      SqlParameter breedParameter = new SqlParameter();
      breedParameter.ParameterName = "@AnimalBreed";
      breedParameter.Value = this.GetBreed();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(genderParameter);
      cmd.Parameters.Add(admittanceParameter);
      cmd.Parameters.Add(breedParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Animal Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE id = @AnimalId;", conn);
      SqlParameter animalIdParameter = new SqlParameter();
      animalIdParameter.ParameterName = "@AnimalId";
      animalIdParameter.Value = id.ToString();
      cmd.Parameters.Add(animalIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAnimalId = 0;
      string foundAnimalName = null;
      string foundAnimalGender = null;
      DateTime foundAnimalAdmittance = DateTime.Today;
      string foundAnimalBreed = null;
      while(rdr.Read())
      {
        foundAnimalId = rdr.GetInt32(0);
        foundAnimalName = rdr.GetString(1);
        foundAnimalGender = rdr.GetString(2);
        foundAnimalAdmittance = rdr.GetDateTime(3);
        foundAnimalBreed = rdr.GetString(4);
      }
      Animal foundAnimal = new Animal(foundAnimalName, foundAnimalGender, foundAnimalAdmittance, foundAnimalBreed, foundAnimalId);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return foundAnimal;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
