using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFiguras2D.Views;
using System.Windows.Forms;

namespace ProyectoFiguras2D
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal. Configura el entorno de
        /// Windows Forms y lanza el formulario principal.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Activa estilos visuales del sistema operativo
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia la aplicación con el formulario principal
            Application.Run(new MainForm());
        }
    }
}
