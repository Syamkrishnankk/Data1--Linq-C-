// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.Json;
// public class Workout
// {
//     public string? workoutId { get;set; }
//     public string? name { get;set; }
//     public string? date { get;set; }
//     public List<Exercise> exercises { get;set; }
// }
// public class Exercise
// {
//     public string? exerciseId  { get;set; }
//     public string? name { get;set; }
//     public List<Set> sets  { get;set; }
//     public string? duration { get; set; }
//     public string? distance { get; set; }
//     public int? caloriesBurned { get; set; }
// }
// public class Set
// {
//     public int? setNumber { get;set; }
//     public int? repetitions { get;set; }
//     public string? weight { get; set; }
// }
// public class WorkoutWrapper
// {
//     public List<Workout> workouts { get; set; }
// }
// class Program
// {
//     static void Main(string[] args)
//     {
//         string jsonData = @"{
//             'workouts': [
//                 {
//                     'workoutId': 'w001',
//                     'name': 'Full Body Workout',
//                     'date': '2024-06-25',
//                     'exercises': [
//                         {
//                             'exerciseId': 'e001',
//                             'name': 'Push-ups',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 15 },
//                                 { 'setNumber': 2, 'repetitions': 17 },
//                                 { 'setNumber': 3, 'repetitions': 20 }
//                             ]
//                         },
//                         {
//                             'exerciseId': 'e002',
//                             'name': 'Squats',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 20 },
//                                 { 'setNumber': 2, 'repetitions': 20 },
//                                 { 'setNumber': 3, 'repetitions': 20 }
//                             ]
//                         },
//                         {
//                             'exerciseId': 'e003',
//                             'name': 'Pull-ups',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 10 },
//                                 { 'setNumber': 2, 'repetitions': 8 },
//                                 { 'setNumber': 3, 'repetitions': 6 }
//                             ]
//                         }
//                     ]
//                 },
//                 {
//                     'workoutId': 'w002',
//                     'name': 'Cardio Session',
//                     'date': '2024-06-26',
//                     'exercises': [
//                         {
//                             'exerciseId': 'e004',
//                             'name': 'Running',
//                             'duration': '30 minutes',
//                             'distance': '5 kilometers',
//                             'caloriesBurned': 300
//                         },
//                         {
//                             'exerciseId': 'e005',
//                             'name': 'Jump Rope',
//                             'duration': '15 minutes',
//                             'caloriesBurned': 200
//                         }
//                     ]
//                 },
//                 {
//                     'workoutId': 'w003',
//                     'name': 'Upper Body Workout',
//                     'date': '2024-06-27',
//                     'exercises': [
//                         {
//                             'exerciseId': 'e006',
//                             'name': 'Bench Press',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 12, 'weight': '50 kg' },
//                                 { 'setNumber': 2, 'repetitions': 10, 'weight': '55 kg' },
//                                 { 'setNumber': 3, 'repetitions': 8, 'weight': '60 kg' }
//                             ]
//                         },
//                         {
//                             'exerciseId': 'e007',
//                             'name': 'Dumbbell Rows',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 12, 'weight': '20 kg' },
//                                 { 'setNumber': 2, 'repetitions': 10, 'weight': '22 kg' },
//                                 { 'setNumber': 3, 'repetitions': 8, 'weight': '25 kg' }
//                             ]
//                         },
//                         {
//                             'exerciseId': 'e008',
//                             'name': 'Bicep Curls',
//                             'sets': [
//                                 { 'setNumber': 1, 'repetitions': 15, 'weight': '10 kg' },
//                                 { 'setNumber': 2, 'repetitions': 12, 'weight': '12 kg' },
//                                 { 'setNumber': 3, 'repetitions': 10, 'weight': '15 kg' }
//                             ]
//                         }
//                     ]
//                 }
//             ]
//         }";
//     var workoutsWrapper = JsonConvert.DeserializeObject<WorkoutWrapper>(jsonData);
//     var workouts = workoutsWrapper.workouts;

//     Console.WriteLine(" ");
//     //  Calculate the total calories burned across all workouts.
//     var cal = workouts.SelectMany(e => e.exercises).Sum(s => s.caloriesBurned);
//     Console.WriteLine($"Total calories burned across all workouts: {cal}");

//     Console.WriteLine(" ");
//     // For each workout, calculate the total number of sets and repetitions.
//     var wid = workouts.Select(w => w.name);
//     foreach(var id in wid){
//         var setsCount = workouts.Where(w => w.name == id).Where(s => s.exercises != null).SelectMany(e => e.exercises).Where(s => s.sets != null).SelectMany(s => s.sets).Where(s => s.setNumber != null).Select(c => c.setNumber);
//         var repetition = workouts.Where(w => w.name== id).Where(s => s.exercises != null).SelectMany(e => e.exercises).Where(s => s.sets != null).SelectMany(s => s.sets).Where(s => s.setNumber != null).Select(c => c.repetitions);
//         Console.WriteLine($"Workout  {id} : Sets {setsCount.Count()}, Repetitions {repetition.Count()}");
//     }

//     Console.WriteLine(" ");
//     // Calculate the average duration of cardio workouts.
//     var avgDuration = workouts.Where(w => w.name == "Cardio Session").SelectMany(e => e.exercises).Select(d => Duration(d.duration)).Average();
//     Console.WriteLine($"Average duration of cardio workouts : {avgDuration}");
//     static int Duration(string duration){
//         return Int32.Parse(duration.Replace(" minutes",""));
//     }
 
//     Console.WriteLine(" ");
//     // Determine the heaviest weight lifted in any bench press set.
//     var heavy = workouts.Where(e => e.exercises != null).SelectMany(e => e.exercises).Where(e => e.sets != null && e.name == "Bench Press").SelectMany(e => e.sets).Select(w => Weight(w.weight)).Max();
//     static int Weight(string weight){
//         return Int32.Parse(weight.Replace(" kg",""));
//     }
//     Console.WriteLine($"Heaviest weight lifted in any bench press set : {heavy}");

//     Console.WriteLine(" ");
//     // Calculate the percentage increase in repetitions for push-ups between the first and last workout.
//     float Fper = (float) workouts.Where(e => e.exercises != null).SelectMany(e => e.exercises).Where(e => e.name == "Push-ups" && e.sets != null).SelectMany(s => s.sets).Select(r => r.repetitions).First();
//     float Lper = (float) workouts.Where(e => e.exercises != null).SelectMany(e => e.exercises).Where(e => e.name == "Push-ups" && e.sets != null).SelectMany(s => s.sets).Select(r => r.repetitions).Last();
//     float per = (Lper-Fper)/Fper*100;
//     Console.WriteLine($"Percentage increase in repetitions for push-ups between the first and last workout : {String.Format("{0:0.00}", per)}%");

//     Console.WriteLine(" ");
//     // For each exercise, calculate the average number of repetitions per set.
//     var avgExercise = workouts.Where(s => s.exercises != null).SelectMany(e => e.exercises).Select(s => s.name);
//     Console.WriteLine("Average number of repetitions per set :");
//     foreach(var exe in avgExercise){
//         var avgRep = workouts.Where(s => s.exercises != null).SelectMany(e => e.exercises).Where(s => s.name == exe).Where(s => s.sets != null).SelectMany(e => e.sets).Select(s => s.repetitions).Average() ?? 0;
//         Console.WriteLine($"-> {exe} : {String.Format("{0:0}", avgRep)} ");
//     }

//     Console.WriteLine(" ");
//     // Calculate the total number of repetitions for each exercise in the "Full Body Workout".
//     var totalRep = workouts.Where(e => e.name == "Full Body Workout").SelectMany(w => w.exercises).Select(e => new{
//         exeName = e.name,
//         totalrep = e.sets.Sum(s => s.repetitions)
//     });
//     Console.WriteLine("Total number of repetitions for each exercise in the Full Body Workout");
//     foreach(var tot in totalRep){
//         Console.WriteLine($"-> {tot.exeName} : {tot.totalrep}");
//     }

//     Console.WriteLine(" ");
//     // List all exercises with a duration greater than 20 minutes.
//     var exeDur = workouts.Where(w => w.exercises != null).SelectMany(w => w.exercises).Where(e => e.duration != null && Duration(e.duration) > 20).Select(e =>e.name);
//     Console.WriteLine("List all exercises with a duration greater than 20 minutes.");
//     foreach(var exe in exeDur){
//         Console.WriteLine($"-> {exe}");
//     }
    
//     Console.WriteLine(" ");
//     // var setsCount1 = workouts.Where(w => w.workoutId == "w001").SelectMany(e => e.exercises).Where(s => s.sets != null).SelectMany(s => s.sets).Where(s => s.setNumber != null).Select(c => c.setNumber);
//     // var setsCount2 = workouts.Where(w => w.workoutId == "w002").SelectMany(e => e.exercises).Where(s => s.sets != null).SelectMany(s => s.sets).Where(s => s.setNumber != null).Select(c => c.setNumber);
//     // foreach(var set in setsCount2){
//     //     Console.WriteLine(set); 
//     // }
//     // Console.WriteLine(setsCount2.Count());




//     // var exercises = workouts.exercises;
//     // foreach(var e in workouts){
//     //     Console.WriteLine(e.name);
//     // }

//     // string targetWorkoutId = "w002";

//     // var workout = workouts.FirstOrDefault(w => w.workoutId == targetWorkoutId).exercises.Where(w => w.exerciseId == "e008");

//     // var workout = workouts.FirstOrDefault(w => w.workoutId == targetWorkoutId).exercises.FirstOrDefault(w => w.exerciseId == "e008").sets.Select(s => s.setNumber);
//     // var workout = workouts.First(w => w.workoutId == targetWorkoutId).exercises.First(w => w.exerciseId == "e004").distance;
//     // Console.WriteLine(workout);
//     // var workoutList = exec.Select(w => w.name).ToList();
//     // foreach(var work in workout){

//     //     Console.WriteLine(work);
    
//     // }
//     // var workoutList = workouts.Select(w => w.name).ToList();
//     // var workoutList001 = workouts.First(w => w.workoutId == "w001").Select(w => w.name).ToList();
//     // var workoutNames = workouts.Select(w => w.date).ToList();
//     // foreach(var work in workoutList001){

//     //     Console.WriteLine(work);
    
//     // }
//     // Assuming workoutId is known (e.g., "w001")
// // string targetWorkoutId = "w001";

// // var workout = workouts.FirstOrDefault(w => w.workoutId == targetWorkoutId);

// // if (workout != null)
// // {
// //     foreach(var c in workout.exercises){
// //         Console.WriteLine(c);
// //     }
    
// // }

//     //  foreach (var workout in workouts)
//     // {
//     //     Console.WriteLine($"Workout: {workout.name}");
//     //     foreach (var exercise in workout.exercises)
//     //     {
//     //         Console.WriteLine(workout.name);
            
//     //     }
//     // }
  
//     }
// }