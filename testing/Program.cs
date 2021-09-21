using System;
using TimHanewich.SqlHelper;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertHelper ih = new InsertHelper("People");
            ih.Add("FirstName", "Tim");
            ih.Add("LastName", "Hanewich");
            ih.Add("DateOfBirth", "12/8/1996", true);
            Console.WriteLine(ih.ToString());
        }
    }
}
