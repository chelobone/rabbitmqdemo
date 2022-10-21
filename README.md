# Laboratorio para RabbitMQ

![RabbitMQ](https://pbs.twimg.com/profile_images/1223261138059780097/eH73w5lN_400x400.jpg)

### Este es un laboratorio para entender como funcionan los MQ (Message Queues) en RabbitMQ.

## *Queues.Rabbit.Consumer*
Microservicio que contiene los consumidores (ejecutar código) para guardar info

[Consumer.cs](Queues.Rabbit.Consumer/Consumers/Consumer.cs): Contiene el método para ejecutar la cola.

```cs
public class Consumer : IConsumer<Client>
    {
        public async Task Consume(ConsumeContext<Client> context)
        {
            var data = context.Message;

            if (data != null)
            {
                //context.
                new DAClient().ConnectionToDatabase(data);
            }
        }
    }
 ```

## *Queues.Rabbit.Learning*
Microservicio que realiza la llamada para agregar la solicitud a la cola de RabbitMQ

[CustomerController.cs](Queues.Rabbit.Learning/Controllers/CustomerController.cs): Método POST que permite enviar la información a la cola.

Dentro de este método, se crea un identificador unico de transacción, que permitirá consultar en el front si es que el registro ha sido creado:

 ```cs
// POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            if (client != null)
            {
                var offset = DateTimeOffset.Now;
                client.CreatedOn = offset.DateTime;
                client.uniqueId = Convert.ToBase64String(Encoding.ASCII.GetBytes(offset.ToUnixTimeSeconds().ToString()));
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(client);
                return Ok(new { ok = true, data = client.uniqueId });
            }
            return BadRequest(new { ok = false, data = "" });
        }
 ```

## *Queues.Rabbit.Shared*
Libreria de elementos compartidos


#
## *Frontend de carga de archivos*
El proyecto de frontend para ejecutar este laboratorio lo pueden encontrar en este link: [multi-login-v2]()

#