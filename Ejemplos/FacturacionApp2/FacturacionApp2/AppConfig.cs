using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using FacturacionApp2.Data;

namespace FacturacionApp2
{
    public class AppConfig
    {

    //almacene las opciones del DbContext de forma global para que puedan ser accedidas desde cualquier parte de la aplicación
     public static DbContextOptions<FacturacionContext> DbOptions { get; set; }

    }
}
