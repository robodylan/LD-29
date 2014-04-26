using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
//Import SFML
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace LD_29
{
    class Game
    {


        public static string Title = "One Who Shall Not Be Named Yet";
        //Set Screen Boundries
        Rectangle r = Screen.PrimaryScreen.Bounds;
        //Create window for drawing
        public static RenderWindow window = new RenderWindow(new VideoMode((uint)(r.Width * 0.9f), (uint)(r.Height * 0.9f)), Title);
        public static void Start()
        {
            //Begin Main Loop
            while (window.IsOpen())
            {
                //Check For Window Events
                window.DispatchEvents();
                
                //Update all 
                Update();
                //Update window
                window.Display();
            }

        }

        //Method for drawing all assets
        public static void Draw()
        {
        }
    }
}
