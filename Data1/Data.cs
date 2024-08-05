using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class Data
{
    public List<Employees>? Employees { get; set; }
    public List<Departments>? Departments { get; set; }
    public List<DataProjects>? DataProjects { get; set; }
}
public class Employees
{ 
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
    public int? DepartmentId { get; set; }
    public string? Email { get; set; }
    public List<string>? Skills { get; set; }
    public List<Projects>? Projects { get; set; }
    public List<Evaluations>? Evaluations { get; set; }
}
public class Departments
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}
public class DataProjects
{
    public string? Name { get; set; }
    public int? Budget { get; set; }
    public string? Deadline { get; set; }
}
public class Projects{
    public string? Name { get; set; }
    public int? HoursWorked { get; set; }
    public string? Role { get; set; }
}
public class Evaluations
{
    public int? Year { get; set; }
    public decimal? Score { get; set; }
}


class Program
{
    static void Main(string[] args)
    {
        string filePath = "Data.json";
        try
        {
            using(StreamReader reader = new StreamReader(filePath)){
                
                string json = reader.ReadToEnd();
                JObject jobject = JObject.Parse(json);

                JArray employeesArray = (JArray)jobject["employees"];
                List<Employees> employees = new List<Employees>();
                foreach(var item in employeesArray){
                    Employees employee = item.ToObject<Employees>();
                    employees.Add(employee);
                }
                
                JArray departmentsArray = (JArray)jobject["departments"];
                List<Departments> departments = new List<Departments>();
                foreach(var item in departmentsArray){
                    Departments department = item.ToObject<Departments>();
                    departments.Add(department);
                }
         
                JArray projectsArray = (JArray)jobject["projects"];
                List<DataProjects> dataprojects = new List<DataProjects>();
                foreach(var item in projectsArray){
                    DataProjects project = item.ToObject<DataProjects>();
                    dataprojects.Add(project);
                }
            Console.WriteLine("");
            // 1. Find all employees who are proficient in "SQL" and sort them by age in descending order. 
            Console.WriteLine("Find all employees who are proficient in \"SQL\" and sort them by age in descending order.");
            var ageSql = employees.Where(s => s.Skills.Contains("SQL")).OrderByDescending(s => s.Age).Select(s => s.Name);
            foreach(var data in ageSql){
                Console.WriteLine($"-> {data}");
            }
            Console.WriteLine("");

            // 2. Retrieve a list of employees along with the names of their departments and the total budget of the projects they are working on.
            Console.WriteLine("Retrieve a list of employees along with the names of their departments and the total budget of the projects they are working on.");

            var empList = employees.Join(departments,
                            emp => emp.DepartmentId,
                            dept => dept.Id,
                            (emp, dept) => new { emp, dept }
            ).SelectMany(
                e => e.emp.Projects.Select(p => new { e.emp, e.dept, p.Name }),
                (e, p) => new { empName = e.emp.Name, deptName = e.dept.Name, projName = p.Name }
            ).Join(dataprojects,
                empproj => empproj.projName,
                proj => proj.Name,
                (empproj, proj) => new { empproj , proj }
            ).GroupBy(
                a => new {
                    a.empproj.empName,
                    a.empproj.deptName
                },
                a => a.proj.Budget,
                (emp, budget) => new{
                    Name = emp.empName,
                    Department = emp.deptName,
                    TotalBudget = budget.Sum()
                }
            );

            foreach(var data in empList){
                Console.WriteLine($"-> {data.Name} : {data.Department} : {data.TotalBudget}");
            }
            Console.WriteLine("");      

            // 3. List the names of employees and the total number of hours they have worked on all projects.
            Console.WriteLine("List the names of employees and the total number of hours they have worked on all projects.");
            var empHours = employees.Select(s => new{
                Name = s.Name,
                TotalHours = s.Projects.Sum(a => a.HoursWorked)
            });
            foreach(var data in empHours){
                Console.WriteLine($"-> {data.Name} : {data.TotalHours}");
            }
            Console.WriteLine("");

            // 4. Group employees by the department and list the average evaluation score for each department for the year 2023.
            Console.WriteLine("Group employees by the department and list the average evaluation score for each department for the year 2023.");
            var avgList = employees.SelectMany(e => e.Evaluations.Where(eval => eval.Year == 2023).Select(p => new { e.DepartmentId , p.Score })
            )
            .Join(departments,
                            emp => emp.DepartmentId,
                            dept => dept.Id,
                            (emp, dept) => new {  dept.Name, emp.Score}
            ).GroupBy(
                a => a.Name,
                a => a.Score,
                (e, p) => new{
                    Department = e,
                    Avg = p.Average()
                }
                
            );

            foreach(var data in avgList){
                Console.WriteLine($"-> {data.Department} : {data.Avg}");
            }
            Console.WriteLine("");

            // 5. Find all projects that have a deadline within the next 6 months and list the employees involved in those projects.
            Console.WriteLine("Find all projects that have a deadline within the next 6 months and list the employees involved in those projects.");
            static bool DLine(string date){
                DateTime now = DateTime.Now;
                int day = Int32.Parse(now.ToString("dd"));
                int month = Int32.Parse(now.ToString("MM"));
                int year = Int32.Parse(now.ToString("yyyy"));
                int inDay = Int32.Parse(date.Substring(8,2));
                int inMonth = Int32.Parse(date.Substring(5,2));
                int inYear = Int32.Parse(date.Substring(0,4));
                if(inYear >= year){
                    if(inMonth > month && (inMonth-month) <= 6){
                        return true;
                    }
                }
                return false;
                
            }
            var sixMList = employees.SelectMany(e => e.Projects.Select(a => new{
                Name = e.Name, Project = a.Name
            })).Join(dataprojects,
                emp => emp.Project,
                proj => proj.Name,
                (emp, proj) => new { emp, proj }
            ).Where(a => DLine(a.proj.Deadline)).GroupBy(a => a.emp.Project);

            foreach(var data in sixMList){
                Console.WriteLine($"- {data.Key}");
                foreach(var d in data){
                    Console.WriteLine($"---> {d.emp.Name}");
                }
            }
            Console.WriteLine("");

            // 6. List employees who have worked on more than one project in a managerial role (e.g., "Lead" or "Manager").
            Console.WriteLine("List employees who have worked on more than one project in a managerial role (e.g., "+"Lead"+" or "+"Manager"+").");
            string[] managerList = {"Lead","Manager"};
            var empLead = employees.Select(e => new{
                Name = e.Name,
                leadCount = e.Projects.Count(p => managerList.Contains(p.Role))
            }).Where(a => a.leadCount > 1);
            if(empLead == null){
                Console.WriteLine("No Employees");
            }
            else{
                foreach(var data in empLead){
                    Console.WriteLine(data.Name); 
                }
            }

            Console.WriteLine("");

            // 7. List all unique skills possessed by employees, ordered alphabetically.
            Console.WriteLine("List all unique skills possessed by employees, ordered alphabetically.");
            var uniqueSkills = employees.SelectMany(s => s.Skills.Select(s => s)).Distinct().OrderBy(s => s);
            foreach(var data in uniqueSkills){
                Console.WriteLine($"-> {data}");
            }
            Console.WriteLine("");

            // 8. Calculate the total budget for all projects and the average project budget.
            Console.WriteLine("Calculate the total budget for all projects and the average project budget.");
            var totBudget = employees.SelectMany(s => s.Projects.Select(a => new{
                Project = a.Name
                })).Join(dataprojects,
                emp  => emp.Project,
                proj => proj.Name,
                (emp, proj) => new { emp, proj }
            ).Select(
                p => p.proj.Budget
            ).Sum(s => s);
            var avgBudget = employees.SelectMany(s => s.Projects.Select(a => new{
                Project = a.Name
                })).Join(dataprojects,
                emp  => emp.Project,
                proj => proj.Name,
                (emp, proj) => new { emp, proj }
            ).Select(
                p => p.proj.Budget
            ).Average(s => s);
            Console.WriteLine($"-> Total Budget : {totBudget}");
            Console.WriteLine($"-> Average Budget : {avgBudget}");
            Console.WriteLine("");

            // var totBudget = dataprojects.Sum(s => s.Budget);
            // var avgBudget = dataprojects.Average(s => s.Budget);
            // Console.WriteLine(totBudget);
            // Console.WriteLine(avgBudget);

            // 9. Check if there are any employees who have a perfect evaluation score (5.0) for any year.
            Console.WriteLine("Check if there are any employees who have a perfect evaluation score (5.0) for any year.");
            var evScore = employees.SelectMany(s => s.Evaluations.Select(a => a.Score)).Any(a => a.ToString() == "5.0");
            // foreach(var data in evScore){
            //      Console.WriteLine(data);
            // }
            if(evScore)
            Console.WriteLine("-> Employee have perfect evaluation score 5.0");
            else
            Console.WriteLine("-> No Employee have perfect evaluation score 5.0");
            Console.WriteLine("");

            // 10. Create a summary report listing each department's name, total number of employees, total hours worked on projects, and average evaluation score.
            Console.WriteLine("Create a summary report listing each department's name, total number of employees, total hours worked on projects, and average evaluation score.");
            var summaryReport = employees.Join(departments,
                emp => emp.DepartmentId,
                dept => dept.Id,
                (emp, dept) => new { 
                    deptName = dept.Name,
                    totHours = emp.Projects.Sum(a => a.HoursWorked),
                    Scores = emp.Evaluations.Select(a => a.Score)

                 }
                )
                .GroupBy(
                    e => e.deptName
                    ).Select(s => new{
                        DeptName = s.Key,
                        EmpCount = s.Count(),
                        TotHours = s.Sum(x => x.totHours),
                        AvgScore = s.SelectMany(x => x.Scores).Average()
                    });
            foreach(var data in summaryReport){
            Console.WriteLine($"-> {data.DeptName} : {data.EmpCount} : {data.TotHours} : {data.AvgScore} ");
            }

            // 11. For each project, list the names of the employees involved, their roles, and the total hours worked on that project
            Console.WriteLine("For each project, list the names of the employees involved, their roles, and the total hours worked on that project");
            var summaryProject = employees.SelectMany(e => e.Projects.Select(p => new { Name = e.Name, projName = p.Name, Hours = p.HoursWorked, Role = p.Role  }))
                                // .GroupBy(emp => 
                                //     emp.projName);
                                //     ).Select(
                                //     s => new {
                                //         ProjName = s.Key,
                                //         empName = s.Select(a => a.Name),
                                //         role = s.Select(a => a.Role),
                                //         totHours = s.Select(a => a.Hours)
                                //     }
                                // );
                                .GroupBy(emp => emp.projName).Select(
                                    s => new {
                                        ProjName = s.Key,
                                        emp = s.Select(a => new{
                                            Name = a.Name,
                                            Role = a.Role,
                                            Hours = a.Hours
                                        }),
                                        
                                    }
                                );
            // foreach(var data in summaryProject){
            //     Console.WriteLine($"- {data.Key}");

                
            //     foreach(var d in data){
            //         Console.WriteLine($"-> {d.Name} : {d.Role} : {d.Hours}");
            //     }
            // }

            foreach(var data in summaryProject){
                Console.WriteLine($"- {data.ProjName}");

                
                foreach(var d in data.emp){
                    Console.WriteLine($"-> {d.Name} : {d.Role} : {d.Hours}");
                }
            }   
            Console.WriteLine("");              
     
            // 12. Generate a flat list of employee-project pairs showing the employee's name, project name, role, and hours worked.
            Console.WriteLine("Generate a flat list of employee-project pairs showing the employee's name, project name, role, and hours worked.");
            var empProj = employees.SelectMany(e => e.Projects.Select(s => new {
                Name = e.Name,
                Proj = s.Name,
                Role = s.Role,
                Hours = s.HoursWorked
            }));
            foreach(var data in empProj){
                Console.WriteLine($"-> {data.Name} : {data.Proj} : {data.Role} : {data.Hours}");
            }
            Console.WriteLine(""); 


            // var summaryReport = employees.GroupBy(e => e.DepartmentId).SelectMany(
            //     a => new{
            //         deptName = departments.First(d => d.Id == a.Key).Name,
            //         empCount = a.Count(),
            //         totHours = a.SelectMany(e => e.Projects).Sum(p => p.HoursWorked),
            //         avgScore = a.SelectMany(e => e.Evaluations).Average(p => p.Score)

            //     }
            // );
            // foreach(var data in summaryReport){
            //     Console.WriteLine(data.deptName);
            //     Console.WriteLine(data.empCount);
            //     Console.WriteLine(data.totHours);
            //     Console.WriteLine(data.avgScore);

            // }
// .SelectMany(
//                     e => e.Projects.Select(p => new { e.Name, e.DepartmentId, e.Evaluations, p.HoursWorked })
//                 ).
// .Where(a => a.Year == 2023).Select(ed => new{
//                 Department = ed.Department,
//                 Avg = ed.Average(s => s.Score)
//             })
            // var empty = employees.SelectMany(s => s.Projects.Select(a => a.Name));
            // var empty = employees.SelectMany(s => s.Projects);
            // foreach(var data in empty){
            //     Console.WriteLine(data.);
            // }

            // var empList = employees.Join(departments,
            //                 emp => emp.DepartmentId,
            //                 dept => dept.Id,
            //                 (emp, dept) => new { emp, dept }
            // ).Join<dynamic, Projects, string, dynamic>(dataprojects,
            //     emp2 => emp2.emp.SelectMany(s => s.Projects.Select(a => a.Name)),
            //     proj => proj.Name,
            //     (emp2, proj) => new {emp2, proj}
            
            // ).Select(e => new{
            //     Name = e.emp2.emp.Name,
            //     Department = e.emp2.dept.Name,
            //     TotalBudget = e.proj.Sum(b => b.Budget)
            // });
            }
        }
        catch(Exception ex){
            Console.WriteLine("Error reading JSON data: {0}", ex.Message);
        }







    }
}

        // string filePath = @"C:\Users\descp\OneDrive\Desktop\C#\latest\Data.json";
        // string jsonData = File.ReadAllText(filePath);
        // using(StreamReader r = new StreamReader(Data.json)){
        //     string jsonData = r.ReadToEnd();
        //     var data = JsonConvert.DeserializeObject<Data>(jsonData);
        // }
        // JObject empData = JObject.Parse(File.ReadAllText("C:\Users\descp\OneDrive\Desktop\C#\latest\Data.json"));
        // var empData = JsonConvert.DeserializeObject<Data>(empData);
        // var employeesData = empData.Employees;
        // foreach(var data in employeesData){
        //     Console.WriteLine(data.Id);
        // }
                // using StreamReader reader = new("Data.json");
        // var json = reader.ReadToEnd();
        // var jarray = JArray.Parse(json);
        // List <Data> data = new();

        // foreach(var item in jarray){
        //     Data datas = item.ToObject<Data>();
        //     data.Add(datas);
        // }
        // Console.WriteLine(data);