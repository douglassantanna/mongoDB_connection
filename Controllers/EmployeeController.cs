using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace wcaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IMongoCollection<dynamic> _employees;
        public EmployeeController(IMongoClient client)
        {
            var database = client.GetDatabase("employeeDB");
            _employees = database.GetCollection<dynamic>("employee");
        }

        [HttpGet]
        public ActionResult<dynamic> Get(string name)
        {
            if(name is null)
                return  _employees.Find<dynamic>(new BsonDocument()).Limit(10).ToList();

            var filtrar = Builders<dynamic>.Filter.Eq("name", new BsonRegularExpression($".*{name}*.", "i"));
            return _employees.Find(filtrar).Limit(10).ToList();
        }
    }
}

