﻿using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
namespace BDModel
{
    public class DataAdventure
    {
        private readonly string connectionString;
        public DataAdventure(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int GetId(DataInsights insights)
        {
            string sqlExpression = "GetId";
            int count;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlParameter countParam = new SqlParameter
                    {
                        ParameterName = "@Count",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(countParam);
                    command.ExecuteNonQuery();
                    count = (int)countParam.Value;
                    insights.AddAction("Success: GetId", DateTime.Now);
                    return count;
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                    {
                        sw.WriteLine($"Ошибка в методе DataAdventure.GetId():{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }
                    insights.AddAction("Error: GetId", DateTime.Now);
                    transaction.Rollback();
                }
            }
            return 0;
        }

        public void GetPerson(int beginNumber, int endNumber, string path, DataInsights insights)
        {
            string sqlExpression = "Information";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                try
                {
                    SqlParameter beginParam = new SqlParameter { ParameterName = "@BeginNumber", Value = beginNumber };
                    command.Parameters.Add(beginParam);
                    SqlParameter endParam = new SqlParameter { ParameterName = "@EndNumber", Value = endNumber };
                    command.Parameters.Add(endParam);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet("Person");
                    DataTable dt = new DataTable("Person");
                    ds.Tables.Add(dt);
                    adapter.Fill(ds.Tables["Person"]);
                    ds.WriteXml(path);
                    ds.WriteXmlSchema(Path.ChangeExtension(path, "xsd"));
                    insights.AddAction("Success: GetPerson", DateTime.Now);
                }
                catch (Exception e)
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                    {
                        sw.WriteLine($"Ошибка в методе DataAdventure.GetPerson():{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }
                    insights.AddAction("Error: GetPerson", DateTime.Now);
                    transaction.Rollback();
                }
            }
        }
    }
}
