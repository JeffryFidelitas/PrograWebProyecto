using Microsoft.AspNetCore.Mvc;

public class AccesosController : Controller
{
    [HttpGet]
    public IActionResult Error403()
    {
        return View(); // Vista para el acceso denegado
    }

    // Acción para manejar diferentes errores
    [HttpGet]
    public IActionResult Error(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                return View("Error404");  // Vista para error 404
            case 403:
                return View("Error403");  // Vista para acceso denegado
            case 500:
                return View("Error500");  // Vista para error interno del servidor
            default:
                return View("Error");  // Vista genérica para otros errores
        }
    }
}
