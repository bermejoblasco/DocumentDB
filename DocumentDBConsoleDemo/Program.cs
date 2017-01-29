

namespace DocumentDBConsoleDemo
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Microsoft.Azure.Documents;
    using System.Linq;
    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                StartDemoAsync().Wait();
            }
            catch (DocumentClientException de)
            {
                var baseException = de.GetBaseException();
                Console.WriteLine("{0} error: {1}, Mensaje: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                var baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Mensaje: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("Final de la demo, pulsa una tecla para finalizar");
                Console.ReadKey();
            }
        }

        private static async Task StartDemoAsync()
        {
            Console.WriteLine("Introduce el nombre de la BBDD a connectar:");
            var databaseId = Console.ReadLine();

            Console.WriteLine("Nombre de la colección:");
            var collectionId = Console.ReadLine();

            DocumentDBRepository<Famila>.Initialize(databaseId, collectionId);
            Console.WriteLine("BBDD y colección listas");

            await ShowMenu();
        }

        private static async Task ShowMenu()
        {
            var salir = false;
            while (!salir)
            {
                Console.WriteLine("Que deseas hacer?");
                Console.WriteLine("1- Crear nuevo documento");
                Console.WriteLine("2- Crear user defined function");
                Console.WriteLine("3- Ejecutar user defined function");
                Console.WriteLine("4- Crear store procedure");
                Console.WriteLine("5- Ejecutar store procedure");
                Console.WriteLine("6- Crear trigger");

                Console.WriteLine("0- Salir");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        await CreateDocument();
                        break;
                    case "2":
                        await CreateUserDefinedFunction();
                        break;
                    case "3":
                        await EjecutarUserDefinedFunction();
                        break;
                    case "4":
                        await CreateStoreProcedure();
                        break;
                    case "5":
                        await EjectuarStoreProcedure();
                        break;
                    case "6":
                        await CreateTrigger();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        continue;
                }
            }
        }

        private static async Task EjecutarUserDefinedFunction()
        {
            var result = DocumentDBRepository<Famila>.ExecutFunctionByName<string>("Sqrt",16);
            var kk = JsonConvert.DeserializeObject<object>(result.AsEnumerable().FirstOrDefault());
            Console.WriteLine(kk);
        }

        private static async Task EjectuarStoreProcedure()
        {
            var result = await DocumentDBRepository<Famila>.ExecuteSpByName<string>("ProcedureCATzureDocumentDBDemo");
            Console.WriteLine(result.Response);
        }

        private static async Task CreateTrigger()
        {
            var trigger = new Trigger()
            {
                Id = "TriggerDocumentDB",
                Body = @"function validateNameExists() {
                              var collection = getContext().getCollection();
                              var request = getContext().getRequest();
                              var docToCreate = request.getBody();
  
                              // Reject documents that do not have a name property by throwing an exception.
                              if (!docToCreate.name) {
                                throw new Error('El documento debe de tenr la propiedad nombre');
                                        }}",
                TriggerOperation = TriggerOperation.All,
                TriggerType = TriggerType.Pre
            };
         
            await DocumentDBRepository<Famila>.CreateDocumentTrigger(trigger);
        }

        private static async Task CreateStoreProcedure()
        {
            var storeProcedure = new StoredProcedure
            {
                Id = "ProcedureCATzureDocumentDBDemo",
                Body = @"function HelloWorld() {
                            var context = getContext();
                            var response = context.getResponse();
                            response.setBody('Hello, World');
                        }"
            };
            await DocumentDBRepository<Famila>.CreateStoreProcedureAsync(storeProcedure);
        }

        private static async Task CreateUserDefinedFunction()
        {
            var function = new UserDefinedFunction
            {
                Id = "Sqrt",
                Body = @"function Sqrt(number) 
                        var context = getContext();
                            var response = context.getResponse();
                            response.setBody(Math.sqrt(number));
                        };"
            };
            await DocumentDBRepository<Famila>.CreateUserDefinedFunctionAsync(function);
        }

        private static async Task CreateDocument()
        {
            var document = new Famila
            {
                Apellido = "Apellido1 Apellido2",
                Padres = new List<Padre>
                {
                    new Padre
                    {
                        Nombre = "NombrePadre",
                        NombreFamilia = "Apellido1Padre"
                    },
                    new Padre
                    {
                        Nombre = "NombreMadre",
                        NombreFamilia = "Apellido1Madre"
                    }
                },
                Hijos = new List<Hijo>
                {
                    new Hijo
                    {
                        Nombre = "Hija1",
                        NombreFamilia = "Apellido1 Apellido2",
                        SexoGenero = "Mujer",
                        Edad = 0,
                        Mascota = new List<Mascota>
                        {
                            new Mascota
                            {
                                Nombre = "NNombreMascota"
                            }
                        }
                    }
                },
                Direccion = new Address
                {
                    Ciudad = "Barcelona",
                    Pais = "Espanya",
                    ComunidadAutonoma = "Catalunya",
                    Provincia = "Barcelona"
                }
            };

            await DocumentDBRepository<Famila>.CreateItemAsync(document);
        }
    }
}