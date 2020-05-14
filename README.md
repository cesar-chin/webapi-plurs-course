# webapi-plurs-course
Compilación net core 3 de curso web api plural.

## COURSE OVERVIEW

### ConfigureServices
Agrega servicios al contenedor de dependencias, cada uno que se\
agregue va ser accesado de forma comun

.AddMvc se puede usar pero es mejor utilizar solamente lo que se necesita

### Configure
Usa los servicios que estan registrados y configurados en 
previamente

Cada uno de los "Use" en la configuración se ejecutan en orden, por ejemplo que se 
ejecute antes de autorizar, en cualquier caso cada uno podría hacer que se
termine la ejecución del siguiente

## GETTING STARTED WITH REST

### Que es Rest
Representational State Transfer, como una aplicacion se comporta, pagibas a travez de una red,
donde un usuario avanza con links que resultan en una página siguiente, que representa
el siguiente estado, que se muestra al usaurio.

Es un estilo de arquitectura que implementa muchos estádares, teoricamente es agnostico a los 
protocolos, tal es el caso de json y http aunque en la practica no es despreciable el protocolo
ya que es http

### Otra definición
El cliente cambia de estado dependiendo de la representacion del recurso que esta accesando

### Constraints (desing desitions)
1. Uniform Interface

Compartir unica interfaz técnica (get, post, media type, etc)
	Subconstraint:
	Identification of resource: 
		Por ejemplo dos representaciones para el mismo recurso, json y xml
	Manipular recursos con representaciones:
		Cuando un cliente accesa una representación, incluyendo los metadatos
		debe tener suficiente información para modificar o borrar ese recurso		
		en el servidor, por ejemplo si se permite borrar se debe poder acceder
		el recurso.
	Self descriptive messages:
		Cada mensaje debe contener información suficiente para procesar el mensaje, el response
		debe tener sentido para el request, que se expresan en el header y content type 
	Hypermedia As The Engine of Aplication State (HATEOAS):
		Un link es hipertexto, hipermedia es una generalización de hipertexto, conduce hacia
		como consumir un API, le indica al cliente que puede hacer, permite que el API este
		autodocumentado, en el ejemplo el response tiene los links para borrar, con este subc 	
		el cliente puede de forma automatica mas sobre el API según el recurso que utilice

2. Client Server Constraint

El cliente y el servidor son completamente diferentes, en ningun caso ninguno sabe de la implementación del
otro, se pueden desarrollar de forma independiente.

3. Statelessness
 
El estado es contenido en el mismo request, que contiene todo lo que necesita el cliente.  No existen estados
a nivel del servidor ni del cliente

4. Layered System

Pueden existir multiples capas en la arquitectura, que se pueden agregar, modificar o eliminar, pero 
ninguna capa puede acceder una que esta detrás de la otra, es decir el cliente no puede 
decir con quien está finalmente conectado, REST restringe el conocimiento de una sola capa (layer, outer facing)

5. Cacheable

Permite indicar al cliente si el response es se puede manejar con cache o no, diferentes formas 
pero se previene uso incorrecto de datos en cache, todo con el response

6. Code on demand

El servidor puede extender una funcionalidad especifica del cliente, por ejemplo el server podria
transferir js al cliente si es una web app

UN SERVICIO SE CONSIDERA REST CUANDO SE ADHIERE A TODOS LOS CONSTRAINTS, lo que implica que la mayoria
de servicios restfull no lo son del todo, pero eso no quiere decir que sean malos APIs, tipicamente se
puede ignorar HATEOAS

### Richardson Maturity Model
Nivel 0
Se utiliza http para interacción remota,   host/api  es un post que hace todo, http se usa como un protocolo
de transporte

Nivel 1 - Resources
Se utiliza un URL diferente para cada interacción pero siempre todo es un POST

host/authors, host/authors/id, etc  pero no se usan los recursos de acuerdo al estándar.

Nivel 2 - Verbs
Para cada método se utiliza el verbo http correcto:   get, put, patch, delete.  En este nivel se utilizan los 
codigos de respuesta correctos según el verbo utilizado.

Nivel 3 -  Hipermedia controls
En este nivel se soporte HATEOAS, si se hace un get no solamente se retorna el datos, si no que se retornan 
links que indican como interactuar con el API (permite descubrimiento de servicios y autodocumentación) 

UN API PUEDE ESTAR EN CUALQUIER NIVEL, SIN EMBARGO SOLAMENTE CUANDO SE ESTA EN NIVEL 3 SE DICE QUE ES UN 
RESTFULL API

## STRUCTURING AND IMPLEMENTING THE OUTER FACING CONTRACT
Resource Identifier, la dirección donde esta el recurso
Metodo http, parte del estadar http 
Payload opcional, el contenido esta en el body pero el formato esta en los mediatypes, content negotiation

### Naming resources
No existe un estándar pero si mejores prácticas 
	Siempre debe ser  un pronombre, nunca una acción porque para eso está el método http
	Ayuda el cliente a interpretar como funciona el API
	Se retorna IActionResult  contrato que representa el resultado
	Siempre se debe utilizar atribute routing en cada acción

### Rountig
Liga un request URI a una acción en el controller 
Agregar UseRouting, UseEndPoints, en net core 3 se puede injectar endpoints en middleware 
de forma que se puede ejecutar uno especifico, potencialmente seleccionar otro.

-Para API es mejor usar Atribute-based Routing 
-Es mejor utilizar el routing independiente para cada método
-Siempre utilizar el tipo Guid para Ids
-Siempre utilizar status codes

### Status Codes
Son parte del estandar HTTP, si no se utilizan siempres retorna un codigo por defecto, los 
códigos se utilizan para que tanto cliente como servidor sepan actuar de la mejor forma 
ante cualquier eventualidad 
Success
	200 Ok, 201 Created, 204 No content
Client mistakes
	400 Bad request
	401 Unauthorized
	402 Forbiden (se autentico bien pero no tiene acceso)
	404 Not found
	405 Method not allowed (enviar post pero solo existe get)
	406 Not acceptable (request en un formato no soportado, pide xml pero solo soporta json) 
	409 Conflict por ejemplo al editar algo que no ha sido renovado, crear un recurso que ya existe.
	415 Unsupported media type 
	422 Unprocesable entity 
Server mistakes
	500 Internal Server Error 

### Error 
Cuando un cliente pasa datos invalidos y el API lo rechaza, puede ser parametros, credenciales, etc
Corresponde con errores 400 

### Faults
Cuando el API no puede responder correctamente a un request valido del cliente, corresponde con códigos 500
que indican la disponibilidad del API

### Formaters and Content Negotiation
Busca la mejor representación para una respuesta, cuando pueden haber varias
Para esto se usan los output formaters, net core soporta input y output formaters
Accept header le dice al API que retornar
	media type se pasa en el accept header de el request (json, xml, no default si no se acepta)
	se recomienda retornar 406 Not acceptable (modificar el Service.AddControllers(settup action ) en ConfigureServices)
	La otra forma es aceptar el formater(settup action addInputFormater)

 ## GETTING RESOURSES
### Outer Facing vs Entity Model 
No es lo mismo que esta en la base de datos a lo que se retorna.

Se deben mantener separados para tener un codigo mas flexible y robusto, la base de datos
debe evolucionar de forma independiente al API

Utilizar automapper y automapper extensions for dependency injection

En relaciones padre/hijo no se debe exponer la dependencia directamente

## FILTERING AND SEARCHING
Recepcion de parametros
FromBody para tipos complejos
FromFrom  tipos IFromFile y IFromFileCollection
FromRoutes  son los parametros que hacen match con routing
FromQuery para cualquier otros parametros

### Filtering
Limiting the collection resource
El filtro se aplica cuando ya se conecen los resultados
Solo filtrar por camspos que son parte del recurso

### Searching
Agregar items dependiendo de unas reglas
Se usa cuando no se sabe que se busca

En ambos casos siempre es Deferred Execution, usar Iqueryable
Se pasan ambos como parametros en el routing
Se puede hacer una clase que contenga los parametros para no estar modificando por cada uno,
la clase de parametros es un tipo complejo y debe tener el atributo FromQuery

## CREATING RESOURSES
### Safety and Idenpotency
Cual método utilizar para deteminada operación

Sguro si no cambia la representación del recurso (get, head)
Idempontente cuando se puede llamar muchas veces y se obtiene el mismo resultado

|Method  | Secure | Idemp |
|------  | ------ | ----- |
|GET     | yes    | yes   |
|OPTONS  | yes    | yes   |
|HEAD    | yes    | yes   |
|POST    | no     | no    |
|DELETE  | no     | yes   |
|PUT     | no     | yes   |  
|PATCH   | no     | no    | 

| 3 | 3 | 3 | 3 | 3 |
|---|---|---|---|---|
|   |   |   |   |   |
|   |   |   |   |   |
|   |   |   |   |   |

Importante:  Usar diferentes DTOs para cada operacion 

Cuando se crea siempre se debe retornar el url con el get resultante 

### Supporting options 
Especifica que es lo que puede hacerse para determinado recurso

### Media types
Al igual que en los get, se debe poder agregar algo en json para luego verlo en xml o al contrario


## Validating data and reporting validation errors

### Model State
Diccionario que contiene el estado de un modelo y el binding validation, ademas contiene un mensaje de
error para cada propiedad validada
Cuando la sintaxis esta bien se debe usar 422-unprocessable entity

### Data anotations
Permite validar de forma automatica las propiedades de los atributos
Se pueden agregar validaciones adicionales.

### Class Level Validation
Se hace una clase que sobreescribe los atributos a validar
es mejor porque se ejecuta antes que el metodo validate se ejecute y solo pasa cuando se pasan las validaciones de datos

### Customize error messages
Se puede agregar un error en cada dato que se valida

###Reporting validation errors
Son formatos de error comunes  que identifican problemas especificos de un API, esto se relaciona cuando el 
cliente del API es otro sistema

###Fluent Validation
Ver repositorio Git

##Updating Resources

-






















