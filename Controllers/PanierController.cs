using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Help;
using WebApplication2.Models.Repositories;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class PanierController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<IdentityUser> userManager;
        public PanierController(IProductRepository productRepository,
        IOrderRepository orderRepository,
        UserManager<IdentityUser> userManager)
        {
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
        }
        // GET: /Order/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var cartItems = ListCart.Instance.Items.ToList();
            var totalAmount = ListCart.Instance.GetSubTotal();
            var viewModel = new OrderViewModel
            {
                CartItems = cartItems.Select(item => new CartItemViewModel
                {
                    ProductName = item.Prod.Name,
                    Quantity = item.quantite,
                    Price = item.Prod.Price
                }).ToList(),
                TotalAmount = totalAmount
            };
            return View(viewModel);
        }// POST : /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.GetUserAsync(User).Result;
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Utilisateur non authentifié.";
                    return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
                }
                // Recharger les articles du panier depuis la source (ListCart)
                var cartItems = ListCart.Instance.Items.ToList();
                model.CartItems = cartItems.Select(item => new CartItemViewModel
                {
                    ProductName = item.Prod.Name,
                    Quantity = item.quantite,
                    Price = item.Prod.Price
                }).ToList();
                model.TotalAmount = ListCart.Instance.GetSubTotal();
                var order = new Order
                {
                    CustomerName = user.UserName,
                    Email = user.Email,
                    Address = model.Address,
                    TotalAmount = model.TotalAmount,
                    OrderDate = DateTime.Now,
                    UserId = user.Id,
                    Items = model.CartItems.Select(item => new OrderItem
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
                orderRepository.Add(order);
                ListCart.Instance.Items.Clear();
                TempData["SuccessMessage"] = "Votre commande a été passée avec succès.";
                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }
            TempData["ErrorMessage"] = "Une erreur est survenue. Veuillez vérifier les informations.";
            return View(model);
        }
        // GET: /Order/Confirmation
        public IActionResult Confirmation(int orderId)
        {
            var order = orderRepository.GetById(orderId);
            return View(order);
        }
        public ActionResult Index()
        {
            ViewBag.Liste = ListCart.Instance.Items;
            ViewBag.total = ListCart.Instance.GetSubTotal();
            return View();
        }
        public ActionResult AddProduct(int id)
        {
            Product pp = productRepository.GetById(id);
            ListCart.Instance.AddItem(pp);
            ViewBag.Liste = ListCart.Instance.Items;
            ViewBag.total = ListCart.Instance.GetSubTotal();
            return View();
        }
        [HttpPost]
        public ActionResult PlusProduct(int id)
        {
            Product pp = productRepository.GetById(id);
            ListCart.Instance.AddItem(pp);
            Item trouve = null;
            foreach (Item a in ListCart.Instance.Items)
            {
                if (a.Prod.ProductId == pp.ProductId)
                    trouve = a;
            }
            var results = new
            {
                ct = 1,
                Total = ListCart.Instance.GetSubTotal(),
                Quatite = trouve.quantite,
                TotalRow = trouve.TotalPrice
            };
            return Json(results);
        }
        [HttpPost]
        public ActionResult MinusProduct(int id)
        {
            Product pp = productRepository.GetById(id);
            ListCart.Instance.SetLessOneItem(pp);
            Item trouve = null;
            foreach (Item a in ListCart.Instance.Items)
            {
                if (a.Prod.ProductId == pp.ProductId)
                    trouve = a;
            }
            if (trouve != null)
            {
                var results = new
                {
                    Total = ListCart.Instance.GetSubTotal(),
                    Quatite = trouve.quantite,
                    TotalRow = trouve.TotalPrice,
                    ct = 1
                };
                return Json(results);
            }
            else
            {
                var results = new
                {
                    Total = ListCart.Instance.GetSubTotal(),
                    ct = 0
                };
                return Json(results);
            }
        }
        [HttpPost]
        public ActionResult RemoveProduct(int id)
        {
            Product pp = productRepository.GetById(id);
            ListCart.Instance.RemoveItem(pp);
            var results = new
            {
                Total = ListCart.Instance.GetSubTotal(),
            };
            return Json(results);
        }
    }
}
