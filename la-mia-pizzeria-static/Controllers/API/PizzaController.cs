using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Pizzas(string? search)
        {


            // https://localhost:7013/api/Pizza/Pizzas


            using (PizzaContext db = new())
            {
                if (string.IsNullOrEmpty(search))
                {
                    List<Pizza> pizzas = db.Pizzas.Include(p => p.Category).Include(p => p.Ingredients).ToList();
                    if (pizzas != null)
                    {
                        return Ok(pizzas);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    List<Pizza> pizzas = db.Pizzas.Include(p => p.Category).Include(p => p.Ingredients).Where(p => p.Name.Contains(search)).ToList();
                    if (pizzas != null)
                    {
                        return Ok(pizzas);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

        }

        // https://localhost:7013/api/Pizza/Pizza/{id}

        [HttpGet("{id}")]
        public IActionResult Pizza(int id)
        {
            using (PizzaContext db = new())
            {
                Pizza pizzaFound = db.Pizzas.Where(item => item.Id == id).FirstOrDefault();
                if (pizzaFound != null)
                {
                    return Ok(pizzaFound);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ModelPizza model)
        {

            using (PizzaContext db = new())
            {
                Pizza pizzaToCreate = new()
                {
                    Name = model.Pizza.Name,
                    Description = model.Pizza.Description,
                    Img = model.Pizza.Img,
                    Price = model.Pizza.Price,
                    CategoryId = model.Pizza.CategoryId
                };


                if (model.SelectedIngredientsIDs != null)
                {
                    foreach (int i in model.SelectedIngredientsIDs)
                    {
                        Ingredient ing = db.Ingredients.Where(m => m.Id == i).FirstOrDefault();
                        pizzaToCreate.Ingredients.Add(ing);
                    }
                }

                db.Add(pizzaToCreate);
                db.SaveChanges();
                return Ok(pizzaToCreate);

            }
        }


        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromBody] ModelPizza model)
        {
            using (PizzaContext db = new())
            {
                Pizza pizzaToUpdate = db.Pizzas.Where(item => item.Id == id).FirstOrDefault();
                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Name = model.Pizza.Name;
                    pizzaToUpdate.Description = model.Pizza.Description;
                    pizzaToUpdate.Img = model.Pizza.Img;
                    pizzaToUpdate.Price = model.Pizza.Price;
                    pizzaToUpdate.CategoryId = model.Pizza.CategoryId;

                    pizzaToUpdate.Ingredients.Clear();
                    if (model.SelectedIngredientsIDs != null)
                    {
                        foreach (int i in model.SelectedIngredientsIDs)
                        {
                            Ingredient ing = db.Ingredients.Where(m => m.Id == i).FirstOrDefault();
                            pizzaToUpdate.Ingredients.Add(ing);
                        }
                    }
                    db.SaveChanges();

                    return Ok(pizzaToUpdate);
                }
                else
                {
                    return NotFound();
                }

            }

        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new())
            {
                Pizza pizzaToDelete = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();
                if (pizzaToDelete != null)
                {
                    db.Pizzas.Remove(pizzaToDelete);
                    db.SaveChanges();
                    return Ok("Succesful Delete");

                }
                else
                {
                    return NotFound();
                }
            }

        }








    }
}
