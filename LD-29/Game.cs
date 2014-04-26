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
        //Handle Window Close
            public static void OnClose(object sender, EventArgs e){
                ((RenderWindow)sender).Close();
            }

        public static string Title = "One Who Shall Not Be Named Yet";
        public static void Start()
        {
            //Find Screen boundries.
            Rectangle r = Screen.PrimaryScreen.Bounds;
            //Create Window
            RenderWindow window = new RenderWindow(new VideoMode((uint)(r.Width * 0.9f), (uint)(r.Height * 0.9f)), Title);
            window.Closed += new EventHandler(OnClose);
            window.SetFramerateLimit(120);
            //Load All Assets Here

            //Begin Main Loop
            while (window.IsOpen())
            {
                //Check For Window Events
                window.DispatchEvents();
                
                //Draw On Window
                Draw();
                //Update window
            }

        }

        //Method for drawing all assets
        public static void Draw()
        {

        }
    }
}
