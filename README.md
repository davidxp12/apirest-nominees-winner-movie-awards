# Onion Architecture In ASP.NET Core 

<p>Tecnologias:</p>
<!-- /wp:paragraph -->

<!-- wp:list -->
<ul><li>Onion Architecture</li><li>LiteDB - NOSql Cache local</li><li>.NET 8.0</li><li>Swagger</li><li> Mediator Pattern using MediatR Library</li><li>CRUD operations</li><li>(Dependências Invertidas)Inverted Dependencies</li><li>API Versionamento</li><li>CsvHelper</li><li>NUnit Test</li></ul>

<p>Instruções para executrar o projeto:</p>
<ul><li>É necessário ter o arquivo csv com o nome "movielist" para ter dados iniciais na aplicação ou inserilos via API , path do arquivo : \apirest-nominees-winner-movie-awards\apiRest-moview-awards\WebApi\File\csv </li></ul>
<ul><li>Ao abrir o projeto via visual studio 2022 escolher o projeto ApiRestMoviewAwards.BFF como 'set as startup project' e precionar F5 para abrir o swagger da api </li></ul>
<ul><li>A mesma irá ser executada no endereço http://localhost:49332 </li></ul>

<p>Instruções para executrar os testes integrados:</p>
<ul><li> executar o comando : dotnet test  no path do projeto ApiRestMoviewAwards.Test </li></ul>
<ul><li> ou via test explorer ApiRestMoviewAwards.Test </li></ul>
