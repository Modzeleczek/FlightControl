namespace FlightControl
{
    public abstract class Collidable
    {
        protected Square Hitbox;

        protected Collidable(Point position, int size) => Hitbox = new Square(new Point(position), size);

        public double DistanceBetween(Collidable collidable) => this.Hitbox.DistanceBetween(collidable.Hitbox);
    }
}
