using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Import SFML
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
//Import Phyics and XNA FrameWork
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using LD_29.Level;
using Microsoft.Xna.Framework;
namespace LD_29
{
    class Bullet
    {
        CapsuleShape C;
        PhysicsSprite X;
        public Bullet(int x,int y)
        {
          this.C = new CapsuleShape(0.01f, 0.01f, new PhysicsParams() { Static = false, Density = 20.0f, X = 6, Y = 56, IsSleeping = false, FixedRotation = true, Friction = 0.5f });
          C.Body.LinearVelocity += new Vector2(0.3f, 0.0f);
          X = new PhysicsSprite(C.Body, 20, 20);
          X.Texture = new Texture("Content/character.png");
          X.Draw();
        
        }

    }
}
