using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BibliotecaDbContext>();

var app = builder.Build();

//Criar o POST

app.MapPost("/api/livros", async ([FromServices] BibliotecaDbContext db, [FromBody] Livro livro)=>
{

    db.Livros.Add(livro);
    await db.SaveChangesAsync();
    var livroSalvoComCategoria = await db.Livros
        .Include(l => l.Categoria)
        .FirstOrDefaultAsync(l => l.Id == livro. Id);

        return Results.Created($"/api/livros/{livroSalvoComCategoria.Id}", livroSalvoComCategoria);

});

//Criar o GET

app.MapGet("/api/livros", async ([FromServices] BibliotecaDbContext db) =>
{

    return await db.Livros
        .Include(l=>l.Categoria)
        .ToListAsync();
        
});

//Criar o GET por Id

app.MapGet("api/livros/{id}", async ([FromServices] BibliotecaDbContext db, int id)=>
{
    var livro = await db.Livros.Include(l => l.Categoria).FirstOrDefaultAsync(l => l.Id == id);
    return livro is not null? Results.Ok(livro): Results.NotFound ("livro nao encontrado");
});


app.Run();
