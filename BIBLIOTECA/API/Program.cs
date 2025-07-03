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

//Criar o PUT

app.MapPut("/api/livros/{id}", async ([FromServices] BibliotecaDbContext db, int id, [FromBody] Livro livroAtualizada)=>
{
    var livro = await db.Livros.FindAsync(id);
    if(livro is null)
        return Results.NotFound("Livro nao encontrado");
    if(livroAtualizada.Id != 0 && id != livroAtualizada.Id) {
        return Results.BadRequest("o ir do url nao corresponde ao id do livro no corpo");
    }

        livro.Titulo = livroAtualizada.Titulo;
        livro.CategoriaId = livroAtualizada.CategoriaId;

        await db.SaveChangesAsync();

        var livroSalvoComCategoria = await db.Livros
            .Include(l => l.Categoria)
            .FirstOrDefaultAsync(l => l.Id == livro.Id);

            return Results.Ok(livro);
});


//Criar o Delete por Id

app.MapDelete("/api/livros/{id}", async ([FromServices] BibliotecaDbContext db, int id) =>
{
    var livro = await db.Livros.FindAsync(id);
    if(livro is null) return Results.NotFound("livro nao encontrado");

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();

    var text = "Deletado com sucesso";
    return Results.Ok(text);
});
    


app.Run();
