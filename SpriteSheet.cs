using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace IPCA.MonoGame
{
    public class SpriteSheet
    {
        private string _name;
        private Texture2D _sheet;
        private Dictionary<string, Rectangle> _sprites;

        public Texture2D Sheet => _sheet;

        public Rectangle this[string name]
        {
            get
            {
                if (_sprites.ContainsKey(name))
                    return _sprites[name];

                throw new IndexOutOfRangeException("Acesso a sprite inexistente");
            }
        }

        public SpriteSheet(Game game, string name)
        {
            _name = name;
            _sheet = game.Content.Load<Texture2D>(name);
            _sprites = new Dictionary<string, Rectangle>();

            using (StreamReader reader = File.OpenText("Content/TextureDiamondMiner.json"))
            {
                JObject json = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JObject frames = json["frames"] as JObject;
                foreach (JProperty frame in frames.Properties())
                {
                    string frameName = frame.Name;
                    JObject data = frame.Value as JObject;
                    Rectangle dims = new Rectangle(
                        int.Parse(data["frame"]["x"].ToString()),
                        int.Parse(data["frame"]["y"].ToString()),
                        int.Parse(data["frame"]["w"].ToString()),
                        int.Parse(data["frame"]["h"].ToString()));

                    _sprites[frameName] = dims;
                }

            }
        }
    }
}