﻿using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
     public class EmployeeDB
    {
        public bool Exists(Employee employee)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Employees.Any(e => e.user == employee.user || e.mail == employee.mail);
            }
        }

        public bool IsCorrectPassword(Employee employee)
        {
            using( var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Employees.Any(e => e.user == employee.user && e.password == employee.password);
            }
        }

        public Employee? GetEmployeeData(Employee employee)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                Employee? employeeData;
                
                employeeData = context.Employees.SingleOrDefault(e => e.user == employee.user);

                if (employeeData != null)
                {
                    employeeData.password = "";
                }

                return employeeData;
            }
        }

        public int ChangePassword(string username, string newPassword)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.AccountModificator)))
            {
                var account = context.Employees.SingleOrDefault(u => u.user == username);
                int statusCode = 1;

                if(account != null)
                {
                    account.password = newPassword;
                   if(context.SaveChanges() > 0)
                    {
                        statusCode = 0;
                    }
                }
                else
                {
                    return statusCode;
                }
                return statusCode;
            }
        }

        public int Add(Employee employee)
        {
            int result = 1;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                context.Add(employee);
                result = context.SaveChanges();
            }

            if (result >= 1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
