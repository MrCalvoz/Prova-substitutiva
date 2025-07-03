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


app.Run();
