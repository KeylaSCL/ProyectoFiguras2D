using System;
using System.Collections.Generic;
using System.Drawing;
using ProyectoFiguras2D.Models;
using ProyectoFiguras2D.Models.Shapes;
using ProyectoFiguras2D.Views;

namespace ProyectoFiguras2D.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la lógica de negocio del proyecto.
    /// Intermedia entre la Vista (MainForm) y los Modelos (Shapes).
    /// </summary>
    public class FigureController
    {
        // Referencia a los componentes de la vista para poder manipularlos
        private readonly DrawingCanvas _canvas;
        private readonly ParamsPanel _paramsPanel;

        // Lista de fábricas para instanciar las figuras dinámicamente
        private readonly List<Func<PointF, float, Shape>> _factories;

        // CAMBIO: Variable para rastrear la última figura seleccionada (-1 significa galería)
        private int _lastIndex = -1;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        public FigureController(DrawingCanvas canvas, ParamsPanel paramsPanel, Color[] palette)
        {
            _canvas = canvas;
            _paramsPanel = paramsPanel;
            _factories = InitializeFactories(palette);
        }

        /// <summary>
        /// Lógica para seleccionar y mostrar una figura individual.
        /// </summary>
        public void SeleccionarFigura(int index, string description)
        {
            if (index < 0 || index >= _factories.Count) return;

            // Guardamos el índice para poder refrescar si cambia el tamaño de la ventana
            _lastIndex = index;

            // 1. Calcular el centro del lienzo dinámicamente
            PointF center = new PointF(_canvas.ClientSize.Width / 2f,
                                       _canvas.ClientSize.Height / 2f);

            // 2. Instanciar la nueva figura usando la Factory (Escala base 2.0f)
            Shape newShape = _factories[index](center, 2.0f);
            newShape.ShowCalcs = true;

            // 3. Sincronización: Si el usuario ya tenía la misma figura, mantenemos sus valores
            Shape currentInPanel = _paramsPanel.CurrentShape;
            if (currentInPanel != null && currentInPanel.Name == newShape.Name)
            {
                newShape.SetParams(ObtenerValoresActualesDelPanel(currentInPanel));
            }

            // 4. Actualizar los componentes de la Vista
            _canvas.LoadSingle(newShape);
            _paramsPanel.Visible = true;

            // Solo cargamos la descripción si se proporciona una nueva
            if (!string.IsNullOrEmpty(description))
            {
                _paramsPanel.LoadShape(newShape, description);
            }
        }



        // CAMBIO: Método necesario para solucionar el error CS1061 en MainForm
        /// <summary>
        /// Redibuja el contenido actual. Se invoca desde el evento Resize del formulario.
        /// </summary>
        public void RefrescarLienzo()
        {
            if (_lastIndex >= 0)
            {
                // Si hay una figura activa, la vuelve a centrar (sin cambiar la descripción)
                SeleccionarFigura(_lastIndex, null);
            }
        }

        // CAMBIO: Método para que el MainForm sepa qué figura redibujar si es necesario
        public int GetLastIndex() => _lastIndex;

        /// <summary>
        /// Método privado para extraer los parámetros numéricos del panel.
        /// </summary>
        private double[] ObtenerValoresActualesDelPanel(Shape shape)
        {
            var parameters = shape.GetParams();
            double[] values = new double[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                values[i] = parameters[i].Value;
            }
            return values;
        }

        /// <summary>
        /// Define las fábricas de figuras.
        /// </summary>
        private List<Func<PointF, float, Shape>> InitializeFactories(Color[] palette)
        {
            return new List<Func<PointF, float, Shape>>
            {
                (c,s) => new Square(c, 60, palette[0], s),
                (c,s) => new Circle(c, 40, palette[1], s),
                (c,s) => new Triangle(c, 70, 60, palette[2], s),
                (c,s) => new ProyectoFiguras2D.Models.Shapes.Rectangle(c, 80, 50, palette[3], s),
                (c,s) => new ProyectoFiguras2D.Models.Shapes.Ellipse(c, 50, 30, palette[4], s),
                (c,s) => new Star(c, 45, 18, 5, palette[5], s),
                (c,s) => new Pentagon(c, 40, palette[6], s),
                (c,s) => new Parallelogram(c, 80, 45, 22, palette[7], s),
                (c,s) => new Scalene(c, 70, 55, 85, palette[8], s),
                (c,s) => new Trapeze(c, 50, 85, 40, palette[9], s),
                (c,s) => new Trefoil(c, 22, palette[10], s),
                (c,s) => new Hexagon(c, 40, palette[11], s),
                (c,s) => new Crescent(c, 38, palette[12], s),
                (c,s) => new Cross(c, 65, 22, palette[13], s),
                (c,s) => new Semicircle(c, 40, palette[14], s),
                (c,s) => new Kite(c, 55, 75, palette[15], s),
                (c,s) => new Octagon(c, 40, palette[16], s),
                (c,s) => new PieShape(c, 40, -30, 270, palette[17], s),
                (c,s) => new Heart(c, 42, palette[18], s),
                (c,s) => new Rhombus(c, 80, 50, palette[19], s),
            };
        }
    }
}