using System;

namespace FlightControl
{
    public abstract class Collidable
    {
        protected Square Hitbox;
        protected abstract int Height { get; }

        protected Collidable(Point position, int size) => Hitbox = new Square(new Point(position), size);

        public double DistanceBetween(Collidable collidable) => this.Hitbox.DistanceBetween(collidable.Hitbox)
            + this.DistanceY(collidable.Height);

        public double DistanceBetween(Square square) => this.Hitbox.DistanceBetween(square);

        protected abstract int DistanceY(int y);//Odległość obiektu, na którym wywołujemy metodę, od punktu o wysokości y wzdłuż osi y.
    }
}
