# PooledRabbitClient
An ObjectPool<IModel> addition to the RabbitMQ .Net Client library.
You basically have a pool of channels. You pick a channel when you need and return after publishing your message.


I found below article when I needed a multiple channel publisher implementation for performance concerns:

https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/

So, I gathered all the codes in the article above and added some extra to form a class library and used it in my projects.

Simple usage on a .Net WebApi project:
 
```cs
    public class Startup
    {
        ...
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRabbit(new RabbitOptions()
            {
                UserName = "devuser",
                Password = "devuser",
                HostName = "localhost",
                VHost = "/",
                Port = 5672
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var rabbitClient = app.ApplicationServices.GetService<IRabbitManager>();
            rabbitClient.QueueDeclare("hello", true, false, true, null);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private IRabbitManager _rabbitClient;

        public QueueController(IRabbitManager rabbitClient)
        {
            _rabbitClient = rabbitClient;
        }
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _rabbitClient.Publish<string>(value, "", "hello");
        }
        
        ...
    }
