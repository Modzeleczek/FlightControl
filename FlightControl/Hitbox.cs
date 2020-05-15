

namespace FlightControl
{
    public class Hitbox
    {
        readonly public double Width, Height;
        private Point Position;
        public Hitbox(double x, double y, double width, double height)
        {
            Position = new Point(x, y);
            Width = width;
            Height = height;
        }
        public void Move(double dx, double dy)
        {
            Position.X += dx;
            Position.Y += dy;
        }
        public bool CollidesWith(Hitbox hitbox)
        {
            if (hitbox.Position.X + hitbox.Width >= Position.X && hitbox.Position.X <= Position.X + Width)
            {
                if (hitbox.Position.Y + hitbox.Height >= Position.Y && hitbox.Position.Y <= Position.Y + Height)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
