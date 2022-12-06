using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BirthdayWishApplication
{
    public class Function1
    {
        private static readonly string ConnectionString = "Data Source=DESKTOP-3JLQIVV;Initial Catalog=Student;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [FunctionName("Function1")]
        public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var students = new List<Student>();
            string sql = @"Select * from Student where CAST(BirthDate as date) = Cast(GETDATE() as date)";
            try
            {
                var con = new SqlConnection(ConnectionString);
                using (var command = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            students.Add(new Student { Id = reader.GetInt32(0), Name = reader.GetString(1), BirthDate = reader.GetDateTime(2), Email = reader.GetString(3) });
                    }
                    con.Close();
                }
                foreach(var student in students)
                {
                    log.LogInformation($"Student Name: {student.Name}, Birthdate: {student.BirthDate}, Email: {student.Email}. Birthday wise an email sent sucessfully.");
                }
                log.LogInformation($"C# Timer trigger function executed at sucessfully: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.LogInformation($"C# Timer trigger function executed but have error at: {DateTime.Now} : " +
                    $"error Message : {ex.Message} : error StackTrace : {ex.StackTrace}");
                throw ex;
            }
        }
    }
}
