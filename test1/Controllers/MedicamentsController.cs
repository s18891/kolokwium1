using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test1.Models;

namespace test1.Controllers
{
    [Route("api/medicaments")]
    [ApiController]
    public class MedicamentsController : ControllerBase
    {

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18891;Integrated Security=True";

        [HttpGet("{IdMedicament}")]
        public IActionResult GetMedicament(String IdMedicament)
        {

            var list = new List<Prescription>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "select Prescription.IdPrescription, Prescription.Date, Prescription.DueDate, Prescription.IdPatient, Prescription.IdDoctor from Prescription_Medicament INNER JOIN Prescription ON Prescription_Medicament.IdPrescription= Prescription.IdPrescription WHERE Prescription_Medicament.IdMedicament=@idMedicament ORDER BY Date DESC;";

                    com.Parameters.AddWithValue("idMedicament", IdMedicament);


                    con.Open();


                    SqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {

                        var pe = new Prescription();
                        pe.IdPrescription = (int)dr["IdPrescription"];
                        pe.Date = DateTime.Parse(dr["Date"].ToString());
                        pe.DueDate = DateTime.Parse(dr["DueDate"].ToString());
                        pe.IdPatient = (int)dr["IdPatient"];
                        pe.IdDoctor = (int)dr["IdDoctor"];

                        list.Add(pe);


                    }
                }
            }
            catch (SqlException exc)
            {
                throw new Exception(exc.Message);
            }

            return Ok(list);
        }
    }
}