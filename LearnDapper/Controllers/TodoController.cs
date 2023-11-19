using System.Data.SqlClient;
using AutoMapper;
using Dapper;
using LearnDapper.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LearnDapper.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController  : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public TodoController(IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>>GetAll()
    {
        await using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        var todos = await connection.QueryAsync<TodoDto>("SELECT * FROM Todos");
        return Ok(_mapper.Map<List<TodoDto>>(todos));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FetchArticle([FromRoute] int todoId)
    {
        await using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        var todo = await connection.QueryFirstOrDefaultAsync<TodoDto>("SELECT * FROM Todos WHERE Id = @Id", new { Id = todoId });
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TodoDto>(todo));
    }
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TodoDto todoDto)
    {
        await using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        var todo = await connection.QueryFirstOrDefaultAsync<TodoDto>("INSERT INTO Todos (Title, Description, IsCompleted) VALUES (@Title, @Description, @IsCompleted) SELECT * FROM Todos WHERE Id = SCOPE_IDENTITY()", new { Title = todoDto.Title, Description = todoDto.Description, IsCompleted = todoDto.IsCompleted });
        return CreatedAtAction(nameof(FetchArticle), new { todoId = todo.Id }, _mapper.Map<TodoDto>(todo));
    }
    [HttpPut("{id}")]      
    public async Task<ActionResult>Update([FromRoute] int todoId, [FromBody] TodoDto todoDto)
    {
        await using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        var todo = await connection.QueryFirstOrDefaultAsync<TodoDto>("UPDATE Todos SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id SELECT * FROM Todos WHERE Id = @Id", new { Title = todoDto.Title, Description = todoDto.Description, IsCompleted = todoDto.IsCompleted, Id = todoId });
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TodoDto>(todo));
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult>Delete([FromRoute] int todoId)
    {
        await using var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:DefaultConnection").Value);
        var todo = await connection.QueryFirstOrDefaultAsync<TodoDto>("DELETE FROM Todos WHERE Id = @Id SELECT * FROM Todos WHERE Id = @Id", new { Id = todoId });
        if (todo == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TodoDto>(todo));
    }

}