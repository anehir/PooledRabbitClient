using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PooledRabbitClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private IRabbitManager _rabbitClient;

        public QueueController(IRabbitManager rabbitClient)
        {
            _rabbitClient = rabbitClient;
        }
        
        // GET: api/<QueueController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<QueueController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _rabbitClient.Publish<string>(id.ToString(), "", "hello");
            return $"{id} sent to rabbitmq...";
        }

        // POST api/<QueueController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _rabbitClient.Publish<string>(value, "", "hello");
        }

        // PUT api/<QueueController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QueueController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}