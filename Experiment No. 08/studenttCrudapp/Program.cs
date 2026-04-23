using System;
using System.Linq;
using studenttCrudapp.Data;
using studenttCrudapp.Models;

class Program
{
    static void Main()
    {
        using (var context = new AppDbContext())
        {
            // CREATE
            var student = new Student
            {
                Name = "Shriprasad",
                Age = 20,
                Course = "IT"
            };
            context.Students.Add(student);
            context.SaveChanges();

            Console.WriteLine("Student Added!");

            // READ
            var students = context.Students.ToList();
            foreach (var s in students)
            {
                Console.WriteLine($"{s.Id} {s.Name}");
            }

            // UPDATE
            var firstStudent = context.Students.FirstOrDefault();
            if (firstStudent != null)
            {
                firstStudent.Name = "Updated Shivani";
                context.SaveChanges();
                Console.WriteLine("Student Updated!");
            }

            // DELETE
            var deleteStudent = context.Students.FirstOrDefault();
            if (deleteStudent != null)
            {
                context.Students.Remove(deleteStudent);
                context.SaveChanges();
                Console.WriteLine("Student Deleted!");
            }
        }
    }
}
