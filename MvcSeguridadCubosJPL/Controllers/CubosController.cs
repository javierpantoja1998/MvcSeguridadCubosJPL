using Microsoft.AspNetCore.Mvc;
using MvcSeguridadCubosJPL.Filters;
using MvcSeguridadCubosJPL.Models;
using MvcSeguridadCubosJPL.Services;

namespace MvcSeguridadCubosJPL.Controllers
{
    public class CubosController : Controller
    {
        private ServiceApiCubos service;

        public CubosController(ServiceApiCubos service)
        {
            this.service = service;
        }

        //METODO PARA SACAR TODOS LOS CUBOS
        public async Task<IActionResult> Index()
        {
            
                List<Cubo> cubos =
                    await this.service.GetCubosAsync();
                return View(cubos);
            

        }
        public async Task<IActionResult> FindCuboAsync()
        {
            return View();
        }
        [HttpPost]
        //METODO PARA BUSCAR CUBO POR MARCA
        public async Task<IActionResult> FindCuboAsync(string marca)
        {
            Cubo cubo = await this.service.FindCuboAsync(marca);
            return View(cubo);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.Session.GetString("TOKEN");
            Usuario user = await
                this.service.GetPerfilUsuarioAsync(token);
            return View(user);
        }

        public async Task<IActionResult> CreateCubo()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCubo(Cubo cubo)
        {
            await this.service.InsertCuboAsync(cubo.IdCubo, cubo.Nombre,
                cubo.Marca, cubo.Imagen, cubo.Precio);
            return RedirectToAction("Index", "Cubos");
        }
    }
}
