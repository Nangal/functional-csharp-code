using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using Examples.Domain;

namespace Examples.Chapter7
{
   public interface IEmployeeRepository
   {
      Option<Employee> Get(Guid id);
      IEnumerable<Employee> GetByLastName(string lastName);
   }

   public class EmployeeRepository : IEmployeeRepository
   {
      Func<object, IEnumerable<Employee>> getById;
      Func<object, IEnumerable<Employee>> getByName;

      //public EmployeeRepository(ConnectionString conn)
      //{
      //   var select = "SELECT * FROM EMPLOYEES";
      //   var query = conn.Query<Employee>().Curry();

      //   getById = query($"{select} WHERE ID = @Id");
      //   getByName = query($"{select} WHERE LASTNAME = @LastName");
      //}

      public EmployeeRepository(ConnectionString conn)
      {
         SqlTemplate select = "SELECT * FROM EMPLOYEES"
            , sqlById = $"{select} WHERE ID = @Id"
            , sqlByName = $"{select} WHERE LASTNAME = @LastName";

         var query = conn.Query<Employee>();

         getById = query.Apply(sqlById);
         getByName = query.Apply(sqlByName);
      }

      public Option<Employee> Get(Guid id)
         => getById(new { Id = id }).FirstOrDefault();

      public IEnumerable<Employee> GetByLastName(string lastName)
         => getByName(new { LastName = lastName });
   }
}
