using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class OfficeData
{
    public List<Employees>? Employees { get; set; }
    public List<Departments>? Departments { get; set; }

}
public class Employees
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
    public int? DepartmentId { get; set; }
    public string? Email { get; set; }
    public List<Projects>? Projects { get; set; }
}
public class Projects
{
    public string? Name { get; set; }
    public int? HoursWorked { get; set; }
}
public class Departments
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}


class Program
{
    static void Main(string[] args)
    {
        string jsonData = @"{
  'employees': [
    {
      'id': 1,
      'name': 'Alice Johnson',
      'age': 30,
      'departmentId': 1,
      'email': 'alice.johnson@example.com',
      'projects': [
        {'name': 'Project A', 'hoursWorked': 120},
        {'name': 'Project B', 'hoursWorked': 100}
      ]
    },
    {
      'id': 2,
      'name': 'Bob Smith',
      'age': 42,
      'departmentId': 2,
      'email': 'bob.smith@example.com',
      'projects': [
        {'name': 'Project A', 'hoursWorked': 200},
        {'name': 'Project C', 'hoursWorked': 150}
      ]
    },
    {
      'id': 3,
      'name': 'Carol White',
      'age': 25,
      'departmentId': 1,
      'email': 'carol.white@example.com',
      'projects': [
        {'name': 'Project B', 'hoursWorked': 90},
        {'name': 'Project C', 'hoursWorked': 110}
      ]
    }
  ],
  'departments': [
    {'id': 1, 'name': 'Development'},
    {'id': 2, 'name': 'Marketing'}
  ]
}
";
    var officeData = JsonConvert.DeserializeObject<OfficeData>(jsonData);
    var employeesData = officeData?.Employees;
    var departmentsData = officeData?.Departments;
    // foreach(var data in departmentsData){
    //    Console.WriteLine(data.Id);
    // }
   
    // 1. Find all employees who are older than 30 and work in the "Development" department.
    Console.WriteLine("Find all employees who are older than 30 and work in the 'Development' department.");
    var emp = officeData.Employees.Join(
      officeData.Departments,
      employee => employee.DepartmentId,
      department => department.Id,
      (employee,department) => new {
        Name = employee.Name,
        Age = employee.Age,
        Department = department.Name
      }
    );
    var age = emp.Where(e => e.Age > 30 && e.Department == "Development").Select(e => e.Name);
   
    if(age.Count() == 0){
      Console.WriteLine("-> No Employees");
    }
    else{
        foreach(var data in age){
          Console.WriteLine($"-> {data}");
    }
    }
    Console.WriteLine("");

    // 2. Retrieve the names of employees along with their department names.
    Console.WriteLine("Retrieve the names of employees along with their department names.");
    foreach(var data in emp){
      Console.WriteLine($"-> {data.Name} : {data.Department}");
    }
    Console.WriteLine("");

    // 3. List the names of employees and the total hours they have worked across all projects.
    Console.WriteLine("List the names of employees and the total hours they have worked across all projects.");
    var hour = employeesData.Where(s => s.Projects != null).Select(s => new{
        Name = s.Name,
        Hour = s.Projects.Sum(e => e.HoursWorked)
    } );
    foreach(var data in hour){
      Console.WriteLine($"-> {data.Name} : {data.Hour}");
    }
    Console.WriteLine("");

    // 4. Group employees by department and calculate the average age of employees in each department.
    Console.WriteLine("Group employees by department and calculate the average age of employees in each department.");
    var avgEmp = emp.GroupBy(e => e.Department).Select(g => new{
      Name = g.Key,
      Avg = g.Average(e => e.Age)
    });
    foreach(var data in avgEmp){
      Console.WriteLine($"-> {data.Name} : {String.Format("{0:0}",data.Avg)}");
      
    }
    Console.WriteLine("");

    // 5. Find the name of the employee who worked the most hours on "Project A".
    Console.WriteLine("Find the name of the employee who worked the most hours on 'Project A'.");
    var mostHour = employeesData.Where(s => s.Projects != null).SelectMany(s => s.Projects, (s,p) => new{ Employee = s, Project = p}).Where(d => d.Project.Name == "Project A")
      .OrderByDescending(d => d.Project.HoursWorked).FirstOrDefault();
    
    Console.WriteLine($"-> {mostHour.Employee.Name}");
    Console.WriteLine("");

    // 6. Extract a list of employees with their names and the names of projects they have worked on.
    Console.WriteLine("Extract a list of employees with their names and the names of projects they have worked on.");
    var empList = employeesData.Select(s => new{
      Name = s.Name,
      Projects = s.Projects.Select(a => a.Name)
    });
    foreach(var data in empList){
      Console.WriteLine($"{data.Name}");
      foreach(var d in data.Projects){
        Console.WriteLine($"-> {d}");
      }
    }
    Console.WriteLine("");

    // 7. List the names of employees who have worked more than 100 hours on any single project.
    Console.WriteLine("List the names of employees who have worked more than 100 hours on any single project.");
    var empHundred = employeesData.Where(s => s.Projects.Any(e => e.HoursWorked > 100)).Select(s => s.Name);
    foreach(var data in empHundred){
      Console.WriteLine($"-> {data}");
    }
    Console.WriteLine("");

    // 8. Check if there is any employee who has worked on "Project D".
    Console.WriteLine("Check if there is any employee who has worked on 'Project D'");
    var ProjectD = employeesData.SelectMany(s => s.Projects).Any(e => e.Name == "Project D");
    if(ProjectD){
      Console.WriteLine($"-> {ProjectD}");
    }
    else{
      Console.WriteLine($"-> Not Worked");
    }
    Console.WriteLine("");

    // 9. List all unique project names across all employees.
    Console.WriteLine("List all unique project names across all employees.");
    var uniqueProject = employeesData.SelectMany(s => s.Projects).Select(e => e.Name).Distinct();
    foreach(var data in uniqueProject){
      Console.WriteLine($"-> {data}");
    }
    Console.WriteLine("");

    // 10. Calculate the total number of hours worked by all employees combined.
    Console.WriteLine("Calculate the total number of hours worked by all employees combined.");
    var totalHours = employeesData.SelectMany(s => s.Projects).Sum(e => e.HoursWorked);
    Console.WriteLine($"-> {totalHours}");
    Console.WriteLine("");




// .Select(d => new{
//       Name = d.Name
//     })
// .Select(a => new{
//       Name = s.Name,
//       Hour = s.Max(s => s.HoursWorked)
//     })
// var empHundred = employeesData.SelectMany(s => s.Projects, (s, e) => new{ Employee = s, Project = e}).Where(a => a.Project.HoursWorked > 100).Select(e => e.Employee.Name).Distinct();  
    
    
     // var emp = employeesData.Where(e => e.Age > 20 && departmentsData.Where(d => d.DepId == e.DepartmentId).Any(d => d.DepName == "Development")).Select(emp => new 
    // {
    //     Name = emp.Name,
    //     Age = emp.Age
    // });
    }
}