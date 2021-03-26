
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using IPCA.MonoGame;

namespace pigeonMiner
{
    enum Direction
    {
        up, Down, Left, Right //0,1,2,3
    }
    public class Player
    {

        private int diamonds, dinamites, dirt;
        private Point position;
        private Game1 game;
        private int delta = 0;
        private int speed = 2;
        private Direction direction = Direction.Down;
        private Vector2 directionVector;

        private float animeSpeed = 8f;

        public Player(Game1 g1, int x, int y)
        {
            position = new Point(x, y);
            game = g1;
            diamonds = 0;
            dinamites = 0;
            dirt = 0;
        }

        public Point getP() { return position; }




        public void Update(GameTime gameTime)
        {
            Point lastPosition = position;

            if (delta > 0)
            {
                delta = (delta + speed) % Game1.tileSize;
            }
            else
            {

                KeyboardState kState = Keyboard.GetState();



                if (kState.IsKeyDown(Keys.A)) { position.X--; direction = Direction.Left; delta = speed; directionVector = -Vector2.UnitX; }
                else if (kState.IsKeyDown(Keys.W)) { position.Y--; direction = Direction.up; delta = speed; directionVector = -Vector2.UnitY; }
                else if (kState.IsKeyDown(Keys.D)) { position.X++; direction = Direction.Right; delta = speed; directionVector = Vector2.UnitX; }
                else if (kState.IsKeyDown(Keys.S)) { position.Y++; direction = Direction.Down; delta = speed; directionVector = Vector2.UnitY; }


                switch (game.Tile(position.X, position.Y))
                {
                    case 'E': dinamites++; game.destroyTile(position.X, position.Y); break;
                    case 'D': diamonds++; game.destroyTile(position.X, position.Y); break;
                    case 'T': dirt++; game.destroyTile(position.X, position.Y); break;
                    case 'X': position = lastPosition; delta = 0; break;
                    case 'O': break;
                        //case 'S':  break;
                }

                /*
                                if (!game.FreeTile(position.X, position.Y))
                                {

                                }
                                else
                                {
                                    if (game.CollectTile(position.X, position.Y))
                                    {

                                    }
                                }


                                if (game.HasBox(position.X, position.Y))
                            {
                                int deltaX = position.X - lastPosition.X;
                                int deltaY = position.Y - lastPosition.Y;
                                Point boxTarget = new Point(deltaX + position.X, deltaY + position.Y);

                                if (game.FreeTile(boxTarget.X, boxTarget.Y))
                                {
                                    for (int i = 0; i < game.boxes.Count; i++)
                                    {
                                        if (game.boxes[i].X == position.X && game.boxes[i].Y == position.Y)
                                        {
                                            game.boxes[i] = boxTarget;
                                        }
                                    }
                                }
                                else
                                {
                                    position = lastPosition;
                                    delta = 0;
                        */

            }

        }

        public void Draw(SpriteBatch sb, SpriteSheet sp)
        {
            Vector2 pos = position.ToVector2() * Game1.tileSize;
            int frame = 0;

            if (delta > 0)
            {
                pos -= (Game1.tileSize - delta) * directionVector;
            }

            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, Game1.tileSize, Game1.tileSize);

            //sb.Draw(aux, rect, Color.White);
            sb.Draw(sp.Sheet, rect, sp["Player.png"], Color.White);
        }

    }
}
