﻿using SessionServiceReference;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic
{
    public class LoginManager
    {
        public int Login(EmployeeClass employee)
        {
            if (!employee.IsValidForLogin())
            {
                return 1;
            }

            SessionServiceClient client = new();
            ResponseWithContentOfEmployeeefOWEHwa response;

            try
            {
                response = client.Login(employee.User, employee.Password);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return 4;
                default:
                    return 0;
            }
        }

        public EmployeeClass getSessionInfo(string user)
        {
            SessionServiceClient client = new();
            ResponseWithContentOfEmployeeefOWEHwa response;

            try
            {
                response = client.GetAccountInfo(user);
            }
            catch(CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.InvalidFields);
                default:
                    EmployeeClass employeeInfo = new()
                    {
                        User = response.Data.user,
                        Role = response.Data.role,
                        Name = response.Data.name,
                        Mail = response.Data.mail,
                        Id = response.Data.id,
                        SucursalId = response.Data.sucursalId
                    };

                    return employeeInfo;
            }
        }

        public int Logout(string user)
        {
            SessionServiceClient client = new();
            Response response;
            int StatusCode = 1;
            try
            {
                response = client.Logout(user);

                switch (response.StatusCode)
                {
                    case 0:
                        StatusCode = 0;
                        return StatusCode;
                    case 1:
                        StatusCode = 1;
                        throw new Exception(ErrorMessages.BadRequest);
                }
            }catch(CommunicationException error)
            {
                StatusCode = 1;
                throw new Exception(ErrorMessages.ServerError);
            }
            return StatusCode;
        }
    }
}
