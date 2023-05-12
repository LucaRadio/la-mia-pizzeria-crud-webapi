
using la_mia_pizzeria_static.Logger;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;


namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public IMyLogger _logger;

        public PizzaController(IMyLogger log)
        {
            _logger = log;
        }
        public IActionResult Index()
        {
            _logger.Print("Start Index Action");

            ViewData["name"] = "Pizzeria";
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas;
                pizzas = db.Pizzas.ToList();


                _logger.Print("End Index Action");
                return View(pizzas);

            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ModelPizza model = new ModelPizza();
            using (PizzaContext db = new PizzaContext())
            {

                getBackProp(db, model, false, null);
                _logger.Print("Redirecting to Create View...");
                return View(model);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ModelPizza data)
        {
            using (PizzaContext db = new PizzaContext())
            {
                _logger.Print("Saving new Pizza...");
                if (!ModelState.IsValid)
                {
                    getBackProp(db, data, false, null);

                    _logger.Print("Pizza didn't pass validation test");
                    return View(data);
                }

                Pizza pizzaToCreate = new Pizza()
                {
                    Name = data.Pizza.Name,
                    Description = data.Pizza.Description,
                    Img = data.Pizza.Img,
                    Price = data.Pizza.Price,
                    CategoryId = data.Pizza.CategoryId,

                };

                if (data.SelectedIngredientsIDs != null)
                {
                    foreach (int i in data.SelectedIngredientsIDs)
                    {
                        Ingredient ing = db.Ingredients.Where(m => m.Id == i).FirstOrDefault();
                        pizzaToCreate.Ingredients.Add(ing);
                    }
                }
                db.Pizzas.Add(pizzaToCreate);
                db.SaveChanges();
                _logger.Print("Saved Succesfull");
                return RedirectToAction("Index");

            }
        }

        public IActionResult Show(int id)
        {
            _logger.Print("Showing selected Pizza...");
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaFound = db.Pizzas.Include(cat => cat.Category).Include(ing => ing.Ingredients).FirstOrDefault(p => p.Id == id);
                if (pizzaFound != null)
                {
                    _logger.Print("Pizza Found...");
                    return View(pizzaFound);
                }
                else
                {
                    _logger.Print("Showing failed...");
                    return View("NotFound");
                }
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            _logger.Print("Redirectiong To Pizza's selected view...");
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaFound = db.Pizzas.Include(ing => ing.Ingredients).FirstOrDefault(p => p.Id == id);
                if (pizzaFound != null)
                {


                    ModelPizza model = new ModelPizza();

                    getBackProp(db, model, true, pizzaFound);

                    model.Pizza = pizzaFound;

                    return View(model);
                }
                else
                {
                    _logger.Print("Editing failed...");
                    return View("NotFound");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, ModelPizza data)
        {
            using (PizzaContext db = new PizzaContext())
            {
                _logger.Print("Updating Selected Pizza...");
                if (!ModelState.IsValid)
                {

                    getBackProp(db, data, false, null);
                    return View(data);
                }

                Pizza pizzaIntoDB = db.Pizzas.Include(ing => ing.Ingredients).FirstOrDefault(p => p.Id == id);
                if (pizzaIntoDB != null)
                {


                    pizzaIntoDB.Name = data.Pizza.Name;
                    pizzaIntoDB.Description = data.Pizza.Description;
                    pizzaIntoDB.Price = data.Pizza.Price;
                    pizzaIntoDB.Img = data.Pizza.Img;
                    pizzaIntoDB.CategoryId = data.Pizza.CategoryId;

                    pizzaIntoDB.Ingredients.Clear();
                    if (data.SelectedIngredientsIDs != null)
                    {
                        foreach (int num in data.SelectedIngredientsIDs)
                        {
                            Ingredient ing = db.Ingredients.Where(m => m.Id == num).FirstOrDefault();
                            pizzaIntoDB.Ingredients.Add(ing);
                        }
                    }
                    db.SaveChanges();
                    _logger.Print("Updating Succesfull...");
                    return RedirectToAction("Index");
                }
                else
                {

                    _logger.Print("Updating failed...");
                    return View("NotFound");
                }

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _logger.Print("Deleting Selected Pizza...");
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaFound = db.Pizzas.FirstOrDefault(p => p.Id == id);
                if (pizzaFound != null)
                {
                    db.Pizzas.Remove(pizzaFound);
                    db.SaveChanges();
                    _logger.Print("Deleting Succesfull...");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.Print("Deleting failed...");
                    return View("NotFound");
                }
            }
        }



        [Authorize(Roles = "Admin, User")]
        public IActionResult Privacy()
        {
            return View();
        }

        public void getBackProp(PizzaContext db, ModelPizza model, bool q, Pizza p)
        {
            List<Category> categories = db.Categories.ToList();
            model.Categories = categories;
            List<Ingredient> ingredients = db.Ingredients.ToList();
            List<SelectListItem> htmlEntityIngredients = new();
            foreach (Ingredient i in ingredients)
            {
                if (!q)
                {

                    htmlEntityIngredients.Add(new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    });

                }
                else
                {
                    htmlEntityIngredients.Add(new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                        Selected = p.Ingredients.Any(single => single.Id == i.Id)
                    });
                }
            }
            model.Ingredients = htmlEntityIngredients;
        }
    }


}
